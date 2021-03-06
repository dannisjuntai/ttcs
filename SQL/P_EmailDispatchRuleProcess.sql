USE [ksky]
GO
/****** Object:  StoredProcedure [dbo].[P_EmailDispatchRuleProcess]    Script Date: 07/31/2014 22:58:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kevin Chen
-- Create date: 2014/03/15
-- Description:	郵件派送規則
-- =============================================
ALTER PROCEDURE [dbo].[P_EmailDispatchRuleProcess]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    -- 找出派送的基數, 處理期限長度
    DECLARE @BaseValue INT, @ExpireLen INT;
    SELECT TOP 1 @BaseValue = ISNULL(Convert(INT, Value), 10) FROM EmailScheduleSetting WHERE Name = 'Base';
    IF @BaseValue IS NULL OR @BaseValue < 1
		SELECT @BaseValue = 10;
	
	SELECT TOP 1 @ExpireLen = ISNULL(Convert(INT,Value), 4) FROM EmailScheduleSetting WHERE Name='ExpireLen';
    IF @ExpireLen IS NULL OR @ExpireLen < 1
		SELECT @ExpireLen = 4;
		
    -- 目前郵件值機的客服派送數量記錄表 [行數][客服ID][目前派送數量][最大派送數量限制][客服所屬群組]
    DECLARE @DispatchedInfo TABLE (RowNumber INT, AgentID VARCHAR(8), CurCount INT, MaxCount INT, MailGroup VARCHAR(MAX)); 
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
			0, 
			FLOOR(@BaseValue*ISNULL(EmailAgentWeight.Weight, 1)), MailMember.GroupID
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
    FROM	MailGroup
	
	-------------------------------------------------------------        	
    -- 掃描目前還未派送的郵件(不是回覆郵件): 專屬派送
    DECLARE @EmailRecordID INT, @EmailRawHeaderID INT, @MailGroup VARCHAR(MAX);
	DECLARE dataCursor CURSOR FAST_FORWARD FOR
	SELECT	EmailRecord.Id, 
			EmailRecord.RawHeaderId,
			MailGroup = (SELECT TOP 1 GroupID FROM MailGroup WHERE CharIndex(MsgTo, Mailbox) > 0 OR CharIndex(Mailbox, MsgTo) > 0)
	FROM	EmailRecord LEFT JOIN 
			EmailRawHeader ON EmailRecord.RawHeaderId = EmailRawHeader.Id
	WHERE	EmailRecord.RID Is NULL AND 
			(PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]]%',EmailRawHeader.MsgSubject) < 1 AND
			 PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_B[0-9][0-9]]%',EmailRawHeader.MsgSubject) < 1) AND				
			(EmailRecord.Garbage Is NULL OR EmailRecord.Garbage < 1);
	
	OPEN dataCursor;  	
	FETCH NEXT FROM dataCursor INTO @EmailRecordID, @EmailRawHeaderID, @MailGroup;
	
	DECLARE @RID BIGINT, @RID_RECORD VARCHAR(MAX);
    DECLARE @InitAgentID VARCHAR(8), @CurrAgentID VARCHAR(8), @OrderNo INT, @AssignCount INT, @ForwardCount INT;
    DECLARE @CurrCount INT;
    DECLARE @AgentID VARCHAR(30), @CustomerID uniqueidentifier, @AutoDispatch BIT;
    
    -- 開始逐步檢查
    DECLARE @CurIndex INT, @AllDispatched INT;
	SELECT @CurIndex = 0, @AllDispatched = 0;
	WHILE @@FETCH_STATUS = 0 AND @AllDispatched < 1
	BEGIN
		-- 有符合的專屬規則
		DECLARE @SpecificRuleID INT;
		SELECT @SpecificRuleID = [dbo].[F_GetSpecificRule](@EmailRawHeaderID);
		IF @SpecificRuleID IS NOT NULL
		BEGIN
			-- 檢查客服順位, 必需 a)有值機, b)派送數量未達最大限制
			SELECT	TOP 1 
					@AgentID = CASE
								WHEN EXISTS (SELECT	1
											 FROM	@DispatchedInfo 
											 WHERE	AgentID= EmailDispatchRule.AgentId1 AND
													CurCount < MaxCount) THEN AgentId1
								WHEN EXISTS (SELECT	1
											 FROM	@DispatchedInfo 
											 WHERE	AgentID= EmailDispatchRule.AgentId2 AND
													CurCount < MaxCount) THEN AgentId2
								WHEN EXISTS (SELECT	1
											 FROM	@DispatchedInfo 
											 WHERE	AgentID= EmailDispatchRule.AgentId3 AND
													CurCount < MaxCount) THEN AgentId3
								ELSE NULL
							   END,
					@CustomerID = EmailDispatchRule.CustomerId,
					@AutoDispatch = EmailDispatchRule.AutoDispatch
			FROM	EmailDispatchRule
			WHERE	EmailDispatchRule.Id = @SpecificRuleID;
			
			-- 沒有可以指派的客服順位, 但是有設定自動派送
			DECLARE @NeedInsert INT; SELECT @NeedInsert = 1;
			IF @AgentID IS NULL AND (@AutoDispatch IS NOT NULL OR @AutoDispatch > 0)
			BEGIN
								
				-- 依郵件族群, 設定索引位置
				SELECT @CurIndex = CurrIndex FROM @MailGroupInfo WHERE GroupID = @MailGroup;
				
				IF NOT EXISTS(SELECT 1 FROM @DispatchedInfo WHERE RowNumber > @CurIndex AND MailGroup = @MailGroup) OR @CurIndex IS NULL
					SELECT @CurIndex = 0;
				
				-- 找出可以指派的客服人員, 必需 a) 派送數量未達最大限制, b) 客服和郵件族群是相同的
				SELECT	TOP 1 @AgentID = AgentID, @CurIndex=RowNumber
				FROM	@DispatchedInfo
				WHERE	RowNumber > @CurIndex AND 
						MailGroup = @MailGroup AND
						CurCount < MaxCount
						
				IF @AgentID IS NULL
					SELECT @NeedInsert = 0, @CurIndex = 0;
				ELSE
					SELECT @NeedInsert = 1;
				
				UPDATE @MailGroupInfo SET CurrIndex = @CurIndex WHERE GroupID = @MailGroup
			END
		END
		
		IF @NeedInsert > 0
		BEGIN
			-- 新增案件ID
			SELECT TOP 1 @RID=IsNull(MAX(CONVERT(BIGINT, RID)),CONVERT(BIGINT, CONVERT(CHAR(8), GETDATE(), 112))*100000)+1 
			FROM (SELECT RID = CASE WHEN CHARINDEX('_B', RID) > 0 THEN '0' ELSE RID END FROM Records) AS T
			WHERE CONVERT(bigint, T.RID) > CONVERT(BIGINT, CONVERT(CHAR(8), GETDATE(), 112))*100000
	
			INSERT INTO [dbo].[Records] (RID, CustomerID, TypeID, AgentID, StatusID, Comment, ServiceGroupID, ServiceItemID, IncomingDT, FinishDT, GroupID)
			SELECT	RID = Convert(nvarchar(16),@RID), 
					CustomerID = @CustomerID,
					TypeID = 3,
					AgentID = @AgentID, 
					StatusID = 1,
					Comment = NULL,
					ServiceGroupID = NULL,
					ServiceItemID = NULL,
					IncomingDT = GETDATE(),
					FinishDT = NULL,
					GroupID = @MailGroup;
				
			UPDATE  [dbo].[EmailRecord]
			SET		RID = Convert(nvarchar(16),@RID),
					ExpireOn = dateadd(hh,@ExpireLen,GETDATE()), 
					IncomeOn = GETDATE(),
					AssignOn = GETDATE(),
					InitAgentID = @AgentID,
					CurrAgentID = @AgentID,
					AssignCount = 0,
					ForwardCount = 0,
					OrderNo = 0,
					UnRead = 1
			WHERE	Id = @EmailRecordID;

			UPDATE	@DispatchedInfo
			SET		CurCount = CurCount+1
			WHERE	RowNumber = @CurIndex;

		END
		
		-- 所有客服人員都超出最大派送數量
		IF NOT EXISTS(SELECT 1 FROM @DispatchedInfo WHERE CurCount < MaxCount)
			SELECT @AllDispatched = 1;
			
		FETCH NEXT FROM dataCursor INTO @EmailRecordID, @EmailRawHeaderID, @MailGroup;
	END
	
	CLOSE dataCursor;
	DEALLOCATE dataCursor;
	
	DECLARE @MsgFrom VARCHAR(MAX);
	-- 掃描目前還未派送的郵件(不是回覆郵件): 一般派送
	DECLARE dataCursor CURSOR FAST_FORWARD FOR
	SELECT	EmailRecord.Id, 
			EmailRecord.RawHeaderId,
			MailGroup = (SELECT TOP 1 GroupID FROM MailGroup WHERE CharIndex(MsgTo, Mailbox) > 0 OR CharIndex(Mailbox, MsgTo) > 0),
			EmailRawHeader.MsgFrom
	FROM	EmailRecord LEFT JOIN 
			EmailRawHeader ON EmailRecord.RawHeaderId = EmailRawHeader.Id
	WHERE	EmailRecord.RID Is NULL AND 
			(PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]]%',EmailRawHeader.MsgSubject) < 1 AND
			 PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_B[0-9][0-9]]%',EmailRawHeader.MsgSubject) < 1) AND				
			(EmailRecord.Garbage Is NULL OR EmailRecord.Garbage < 1);
	
	OPEN dataCursor;  	
	FETCH NEXT FROM dataCursor INTO @EmailRecordID, @EmailRawHeaderID, @MailGroup, @MsgFrom;
	
	WHILE @@FETCH_STATUS = 0 AND @AllDispatched < 1
	BEGIN
		-- 沒有符合的專屬規則
		SELECT @SpecificRuleID = [dbo].[F_GetSpecificRule](@EmailRawHeaderID);
		IF @SpecificRuleID IS NULL
		BEGIN
			-- 依郵件族群, 設定索引位置
			SELECT @CurIndex = CurrIndex FROM @MailGroupInfo WHERE GroupID = @MailGroup;				
					
			IF NOT EXISTS(SELECT 1 FROM @DispatchedInfo WHERE RowNumber > @CurIndex AND MailGroup = @MailGroup) OR @CurIndex IS NULL
				SELECT @CurIndex = 0;
				
			-- 找出可以指派的客服人員, 必需 a) 派送數量未達最大限制, b) 客服和郵件族群是相同的
			SELECT	TOP 1 @AgentID = AgentID, @CurIndex=RowNumber
			FROM	@DispatchedInfo
			WHERE	RowNumber > @CurIndex AND 
					MailGroup = @MailGroup AND
					CurCount < MaxCount
						
			IF @AgentID IS NULL
				SELECT @NeedInsert = 0, @CurIndex = 0;				
			ELSE
				SELECT @NeedInsert = 1;
				
			UPDATE @MailGroupInfo SET CurrIndex = @CurIndex WHERE GroupID = @MailGroup
			
			IF @NeedInsert > 0
			BEGIN		
				SELECT	TOP 1 @CustomerID = ID
				FROM	Customers
				WHERE	CHARINDEX(Contact1_Email1, @MsgFrom) > 0 OR CHARINDEX(Contact1_Email2, @MsgFrom) > 0 OR
						CHARINDEX(Contact2_Email1, @MsgFrom) > 0 OR CHARINDEX(Contact2_Email2, @MsgFrom) > 0 OR
						CHARINDEX(Contact3_Email1, @MsgFrom) > 0 OR CHARINDEX(Contact3_Email2, @MsgFrom) > 0
						
				-- 新增案件ID
				SELECT TOP 1 @RID=IsNull(MAX(CONVERT(BIGINT, RID)),CONVERT(BIGINT, CONVERT(CHAR(8), GETDATE(), 112))*100000)+1 
				FROM (SELECT RID = CASE WHEN CHARINDEX('_B', RID) > 0 THEN '0' ELSE RID END FROM Records) AS T
				WHERE CONVERT(bigint, T.RID) > CONVERT(BIGINT, CONVERT(CHAR(8), GETDATE(), 112))*100000
	
				INSERT INTO [dbo].[Records] (RID, CustomerID, TypeID, AgentID, StatusID, Comment, ServiceGroupID, ServiceItemID, IncomingDT, FinishDT, GroupID)
				SELECT	RID = Convert(nvarchar(16),@RID), 
						CustomerID = @CustomerID,
						TypeID = 3,
						AgentID = @AgentID, 
						StatusID = 1,
						Comment = NULL,
						ServiceGroupID = NULL,
						ServiceItemID = NULL,
						IncomingDT = GETDATE(),
						FinishDT = NULL,
						GroupID = @MailGroup;
				
				UPDATE  [dbo].[EmailRecord]
				SET		RID = Convert(nvarchar(16),@RID),
						ExpireOn = dateadd(hh,@ExpireLen,GETDATE()), 
						IncomeOn = GETDATE(),
						AssignOn = GETDATE(),
						InitAgentID = @AgentID,
						CurrAgentID = @AgentID,
						AssignCount = 0,
						ForwardCount = 0,
						OrderNo = 0,
						UnRead = 1
				WHERE	Id = @EmailRecordID;
				
				UPDATE	@DispatchedInfo
				SET		CurCount = CurCount+1
				WHERE	RowNumber = @CurIndex;
			END
		-- 2.1.2.3 所有客服人員都超出最大派送數量
		IF NOT EXISTS(SELECT 1 FROM @DispatchedInfo WHERE CurCount < MaxCount)
			SELECT @AllDispatched = 1;
		
		END		
		FETCH NEXT FROM dataCursor INTO @EmailRecordID, @EmailRawHeaderID, @MailGroup, @MsgFrom;
	END
	
	CLOSE dataCursor;
	DEALLOCATE dataCursor;
		
END
