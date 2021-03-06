USE [ksky]
GO
/****** Object:  StoredProcedure [dbo].[P_ReplyDispatchRuleProcess]    Script Date: 07/31/2014 22:59:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kevin Chen
-- Create date: 2014/03/15
-- Description:	回覆信件的派送規則
-- =============================================
ALTER PROCEDURE [dbo].[P_ReplyDispatchRuleProcess]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    -- 掃描目前還未派送的郵件, 找出回覆信件類型的郵件
    DECLARE @EmailRecordID INT, @RID_REPLY VARCHAR(MAX), @RID_FORWARD VARCHAR(MAX), @MsgSubject VARCHAR(MAX), @RawHeaderID INT;
    DECLARE @RID_RECORD VARCHAR(MAX), @ExpireLen INT;
    DECLARE @InitAgentID VARCHAR(8), @CurrAgentID VARCHAR(8), @OrderNo INT, @AssignCount INT, @ForwardCount INT;
    DECLARE @CurrCount INT;
    
    -- 目前郵件值機的客服記錄表 [行數][客服ID][客服所屬群組]
    DECLARE @DispatchedInfo TABLE (RowNumber INT, AgentID VARCHAR(8), MailGroup VARCHAR(MAX)); 
    WITH RecentRecordDT AS(
		SELECT	AgentID AS AgentID,
				Max(IncomingDT) AS IncomingDT
		FROM	Records 
		WHERE	TypeID = 3 	
		GROUP BY AgentID
	)
    INSERT INTO @DispatchedInfo
    SELECT	ROW_NUMBER() OVER (ORDER BY RecentRecordDT.IncomingDT) AS RowNumber,
			Agent.AgentID, 
			MailMember.GroupID
    FROM	Agent LEFT JOIN 
			EmailAgentWeight ON Agent.AgentID = EmailAgentWeight.AgentId LEFT JOIN
			MailMember ON Agent.CTILoginID = MailMember.CTILoginID LEFT JOIN
			RecentRecordDT ON Agent.AgentID = RecentRecordDT.AgentID
    WHERE	EmailIsLogin = 1
    ORDER BY RecentRecordDT.IncomingDT;
	
	DECLARE @MailGroupInfo TABLE (RowNumber INT, GroupID VARCHAR(32), CurrIndex INT); 
    INSERT INTO @MailGroupInfo
    SELECT	ROW_NUMBER() OVER (ORDER BY GroupID) AS RowNumber,
			GroupID, 
			0
    FROM	MailGroup;
    
    DECLARE @CurIndex INT, @MailGroup VARCHAR(MAX);
	SELECT @CurIndex = 0;
    
	DECLARE dataCursor CURSOR FAST_FORWARD FOR
	SELECT	EmailRecord.Id,
			[dbo].[F_GetLastRID](EmailRawHeader.MsgSubject),
			MsgSubject,
			EmailRecord.RawHeaderId,
			MailGroup = (SELECT TOP 1 GroupID FROM MailGroup WHERE CharIndex(MsgTo, Mailbox) > 0 OR CharIndex(Mailbox, MsgTo) > 0)
	FROM	EmailRecord LEFT JOIN 
			EmailRawHeader ON EmailRecord.RawHeaderId = EmailRawHeader.Id
	WHERE	EmailRecord.RID Is NULL AND 
			(PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]]%',EmailRawHeader.MsgSubject) > 0 OR
			 PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_B[0-9][0-9]]%',EmailRawHeader.MsgSubject) > 0) AND				
			(EmailRecord.Garbage Is NULL OR EmailRecord.Garbage < 1);
	
	OPEN dataCursor;  	
	FETCH NEXT FROM dataCursor INTO @EmailRecordID, @RID_RECORD, @MsgSubject, @RawHeaderID, @MailGroup;
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- 1.1 找出RID
		/*SELECT @RID_RECORD = CASE 
								WHEN @RID_REPLY IS NOT NULL AND LEN(@RID_REPLY) > 0 THEN @RID_REPLY
								ELSE @RID_FORWARD
							 END;*/
		
		-- 1.2 找出處理期限參數
		SELECT TOP 1 @ExpireLen = ISNULL(Convert(INT,Value), 4) FROM EmailScheduleSetting WHERE Name='ExpireLen';
		IF @ExpireLen IS NULL OR @ExpireLen < 1
		SELECT @ExpireLen = 4;
		
		-- 1.3 找出其它需同步的參數
		DECLARE @CustomerID uniqueidentifier, @StatusID INT, @GroupID VARCHAR(32), @AgentID VARCHAR(8);
		SELECT		TOP 1 @CustomerID = CustomerID, 
					@StatusID=StatusID, 
					@GroupID=GroupID, 
					@AgentID = AgentID 
		FROM		Records 
		WHERE		CHARINDEX(RID, @RID_RECORD) > 0;
		
		SELECT		TOP 1 @CurrAgentID = CurrAgentId,
					@InitAgentID = InitAgentId,
					@AssignCount = AssignCount,
					@ForwardCount = ForwardCount
		FROM		EmailRecord 
		WHERE		CHARINDEX(RID, @RID_RECORD) > 0
		ORDER BY	OrderNo DESC;		
		
		-- 檢查原先客服人員狀態, 如果未執機
		IF EXISTS(	SELECT 1 
					FROM Records LEFT JOIN Agent ON Records.AgentID = Agent.AgentID 
					WHERE RID=@RID_RECORD AND Records.AgentID IS NOT NULL AND Agent.EmailIsLogin = 0)
			BEGIN
				SELECT @AgentID = NULL;
				DECLARE @AutoDispatch BIT;
				-- 有符合的專屬規則
				DECLARE @SpecificRuleID INT;
				SELECT @SpecificRuleID = [dbo].[F_GetSpecificRule](@RawHeaderID);
				IF @SpecificRuleID IS NOT NULL
				BEGIN
					-- 檢查客服順位, 必需 a)有值機,
					SELECT	TOP 1 
							@AgentID = CASE
										WHEN EXISTS (SELECT	1
													 FROM	Agent
													 WHERE	AgentID= EmailDispatchRule.AgentId1 AND
															EmailIsLogin = 1) THEN AgentId1
										WHEN EXISTS (SELECT	1
													 FROM	Agent 
													 WHERE	AgentID= EmailDispatchRule.AgentId2 AND
															EmailIsLogin = 1) THEN AgentId2
										WHEN EXISTS (SELECT	1
													 FROM	Agent 
													 WHERE	AgentID= EmailDispatchRule.AgentId3 AND
															EmailIsLogin = 1) THEN AgentId3
										ELSE NULL
									   END,
							@AutoDispatch = EmailDispatchRule.AutoDispatch
					FROM	EmailDispatchRule
					WHERE	EmailDispatchRule.Id = @SpecificRuleID;
			
					-- 沒有可以指派的客服順位, 但是有設定自動派送
					IF @AgentID IS NULL AND (@AutoDispatch IS NOT NULL OR @AutoDispatch > 0)
					BEGIN
								
						-- 依郵件族群, 設定索引位置
						SELECT @CurIndex = CurrIndex FROM @MailGroupInfo WHERE GroupID = @MailGroup;
				
						IF NOT EXISTS(SELECT 1 FROM @DispatchedInfo WHERE RowNumber > @CurIndex AND MailGroup = @MailGroup) OR @CurIndex IS NULL
							SELECT @CurIndex = 0;
				
						-- 找出可以指派的客服人員, 必需 a) 客服和郵件族群是相同的
						SELECT	TOP 1 @AgentID = AgentID, @CurIndex=RowNumber
						FROM	@DispatchedInfo
						WHERE	RowNumber > @CurIndex AND 
								MailGroup = @MailGroup
				
						UPDATE @MailGroupInfo SET CurrIndex = @CurIndex WHERE GroupID = @MailGroup
					END
				END
				ELSE
				BEGIN
					-- 依郵件族群, 設定索引位置
					SELECT @CurIndex = CurrIndex FROM @MailGroupInfo WHERE GroupID = @MailGroup;				
					
					IF NOT EXISTS(SELECT 1 FROM @DispatchedInfo WHERE RowNumber > @CurIndex AND MailGroup = @MailGroup) OR @CurIndex IS NULL
						SELECT @CurIndex = 0;
				
					-- 找出可以指派的客服人員, 必需 a) 客服和郵件族群是相同的
					SELECT	TOP 1 @AgentID = AgentID, @CurIndex=RowNumber
					FROM	@DispatchedInfo
					WHERE	RowNumber > @CurIndex AND 
							MailGroup = @MailGroup
				END
			END
		
		IF @AgentID IS NOT NULL
		BEGIN
		SELECT	TOP 1 @OrderNo = ISNULL(OrderNo, -1)+1 FROM EmailRecord WHERE RID = @RID_RECORD ORDER BY OrderNo DESC;
		IF @OrderNo IS NULL
			SELECT @OrderNo = 0;
		
		IF EXISTS(SELECT 1 FROM Records WHERE CHARINDEX(RID, @RID_RECORD) > 0 AND StatusID < 3)
		BEGIN
			-- 發生在轉寄第三方的回信
			IF @OrderNo < 1
			BEGIN
				INSERT INTO [dbo].[Records] (RID, CustomerID, TypeID, AgentID, StatusID, Comment, ServiceGroupID, ServiceItemID, IncomingDT, FinishDT,GroupID)
				SELECT	RID = @RID_RECORD, 
						CustomerID = @CustomerID,
						TypeID = 3,
						AgentID = @AgentID, 
						StatusID = @StatusID,
						Comment = NULL,
						ServiceGroupID = NULL,
						ServiceItemID = NULL,
						IncomingDT = GETDATE(),
						FinishDT = NULL,
						GroupID = @GroupID;
			END
			
			-- 回信類型的郵件, 若遇到客服人員未值機, 則進到待轉派狀態, 由管理者進行轉派動作
			--IF EXISTS(	SELECT 1 
			--			FROM Records LEFT JOIN Agent ON Records.AgentID = Agent.AgentID 
			--			WHERE RID=@RID_RECORD AND Records.AgentID IS NOT NULL AND Agent.EmailIsLogin = 0)
			--BEGIN
				UPDATE	Records
				SET		AgentID = @AgentID
				WHERE	CHARINDEX(RID, @RID_RECORD) > 0;
			--END
			
			UPDATE	EmailRecord 
			SET		RID = @RID_RECORD, 
					ExpireOn = dateadd(hh,@ExpireLen,GETDATE()), 
					IncomeOn = GETDATE(),
					AssignOn = GETDATE(),
					InitAgentID = @InitAgentID,
					CurrAgentID = @AgentID,--@CurrAgentID,
					AssignCount = @AssignCount,
					ForwardCount = @ForwardCount,
					OrderNo = @OrderNo,
					UnRead = 1
			WHERE	Id = @EmailRecordId;		
			
		END
		-- 案件已被結案, 重新給予新案件編號
		ELSE		
		BEGIN
			
			DECLARE @RID BIGINT;
			SELECT TOP 1 @RID=IsNull(MAX(CONVERT(BIGINT, RID)),CONVERT(BIGINT, CONVERT(CHAR(8), GETDATE(), 112))*100000)+1 
			FROM (SELECT RID = CASE WHEN CHARINDEX('_B', RID) > 0 THEN '0' ELSE RID END FROM Records) AS T
			WHERE CONVERT(bigint, T.RID) > CONVERT(BIGINT, CONVERT(CHAR(8), GETDATE(), 112))*100000;
			
			INSERT INTO [dbo].[Records] (RID, CustomerID, TypeID, AgentID, StatusID, Comment, ServiceGroupID, ServiceItemID, IncomingDT, FinishDT,GroupID)
			SELECT	RID = Convert(nvarchar(MAX),@RID), 
					CustomerID = @CustomerID,
					TypeID = 3,
					AgentID = @AgentID, 
					StatusID = 1,
					Comment = NULL,
					ServiceGroupID = NULL,
					ServiceItemID = NULL,
					IncomingDT = GETDATE(),
					FinishDT = NULL,
					GroupID = @GroupID;
					
			UPDATE	EmailRecord 
			SET		RID = Convert(nvarchar(MAX),@RID), 
					ExpireOn = dateadd(hh,@ExpireLen,GETDATE()), 
					IncomeOn = GETDATE(),
					AssignOn = GETDATE(),
					InitAgentID = @AgentID,
					CurrAgentID = @AgentID,
					AssignCount = 0,
					ForwardCount = 0,
					OrderNo = 0,
					UnRead = 1
			WHERE	Id = @EmailRecordId;
			
			-- 新主旨			
			DECLARE @FORWARD_POS INT;
			DECLARE @REPLY_POS INT;
			DECLARE @POS INT;

			SELECT @FORWARD_POS = CHARINDEX(UPPER(':wF'),UPPER(REVERSE(@MsgSubject)))-1;
			SELECT @REPLY_POS = CHARINDEX(UPPER(':eR'),UPPER(REVERSE(@MsgSubject)))-1;

			SELECT @POS = @REPLY_POS;

			IF @FORWARD_POS > 0
			BEGIN
				SELECT @POS = @FORWARD_POS;
	
				IF @REPLY_POS > 0 AND @REPLY_POS < @FORWARD_POS
				SELECT @POS = @REPLY_POS;
			END
			DECLARE @MsbSubject_NEW VARCHAR(MAX);
			SELECT @MsbSubject_NEW = RIGHT(@MsgSubject, @POS);
			SELECT @MsbSubject_NEW = LEFT(@MsbSubject_NEW, CHARINDEX('['+@RID_RECORD,@MsbSubject_NEW)-1);

			Update	[dbo].[EmailRawHeader]
			SET		MsgSubject = @MsbSubject_NEW
			WHERE	Id = @RawHeaderID;	
			
		END
		END
		FETCH NEXT FROM dataCursor INTO @EmailRecordID, @RID_RECORD, @MsgSubject, @RawHeaderID, @MailGroup;
	END
	
	CLOSE dataCursor;
	DEALLOCATE dataCursor;
END
