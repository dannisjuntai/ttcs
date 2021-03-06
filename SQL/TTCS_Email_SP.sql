USE [ksky]
GO
/****** Object:  UserDefinedFunction [dbo].[F_GetFilterRule]    Script Date: 03/25/2014 21:28:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kevin Chen
-- Create date: 2014/03/14
-- Description:	是否符合過濾規則
-- =============================================
CREATE FUNCTION [dbo].[F_GetFilterRule] 
(
	-- Add the parameters for the function here
	@RawHeaderID int
)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @RET INT;

	-- Add the T-SQL statements to compute the return value here
	DECLARE @MsgSubject VARCHAR(MAX), @MsgFrom VARCHAR(MAX), @MsgReceivedBy VARCHAR(MAX), @MsgBody VARBINARY(MAX);

	SELECT	@MsgSubject = EmailRawHeader.MsgSubject, 
			@MsgFrom = EmailRawHeader.MsgFrom, 
			@MsgReceivedBy = EmailRawHeader.MsgReceivedBy,
			@MsgBody = EmailRawBody.BinaryCnt 
	FROM	EmailRawHeader JOIN
			EmailRawBody ON EmailRawHeader.Id = EmailRawBody.RawHeaderId AND LEN(EmailRawBody.FileName) < 1
	WHERE	EmailRawHeader.Id = @RawHeaderID;
	
	SELECT	@RET = Id
	FROM	EmailFilterRule
	WHERE	Active = 1 AND
			(PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]]%',@MsgSubject) < 1 OR
			 PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_B[0-9][0-9]]%',@MsgSubject) < 1) AND	
			(CHARINDEX(MsgSubject, @MsgSubject) > 0 OR
			 CHARINDEX(MsgFrom, @MsgFrom) > 0 OR
			 CHARINDEX(MsgReceivedBy, @MsgReceivedBy) > 0 OR
			 CHARINDEX(MsgBody, CONVERT(VARCHAR(MAX), @MsgBody, 2)) > 0);

	-- Return the result of the function
	IF @RET IS NOT NULL OR @RET > 0
		SELECT @RET = 1;
	ELSE
		SELECT @RET = NULL;
		
	RETURN @RET;

END
GO
/****** Object:  UserDefinedFunction [dbo].[F_GetSpecificRule]    Script Date: 03/25/2014 21:28:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kevin Chen
-- Create date: 2014/03/15
-- Description:	由專屬規則中找出客服ID
-- =============================================
CREATE FUNCTION [dbo].[F_GetSpecificRule]
(
	-- Add the parameters for the function here
	@EmailRawHeaderID INT
)
RETURNS VARCHAR(8)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @RuleID INT;
	DECLARE @MsgFrom VARCHAR(MAX), @MsgSubject VARCHAR(MAX);

	SELECT	TOP 1 @MsgFrom = MsgFrom, @MsgSubject = MsgSubject, @RuleID = NULL
	FROM	EmailRawHeader
	WHERE	Id = @EmailRawHeaderID
	
	-- Add the T-SQL statements to compute the return value here
	-- 1. 客戶	
	SELECT	TOP 1 @RuleID = EmailDispatchRule.Id
	FROM	EmailDispatchRule JOIN 
			Customers ON EmailDispatchRule.CustomerId = Customers.ID
	WHERE	ConditionType = 1 AND
			CharIndex(Customers.Contact1_Email1, @MsgFrom) > 0 OR
			CharIndex(Customers.Contact1_Email2, @MsgFrom) > 0 OR
			CharIndex(Customers.Contact2_Email1, @MsgFrom) > 0 OR
			CharIndex(Customers.Contact2_Email2, @MsgFrom) > 0 OR
			CharIndex(Customers.Contact3_Email1, @MsgFrom) > 0 OR
			CharIndex(Customers.Contact3_Email2, @MsgFrom) > 0;

	-- 2. Specific Email Address
	IF @RuleID IS NULL			
	BEGIN
		SELECT	TOP 1 @RuleID = EmailDispatchRule.Id 
		FROM	EmailDispatchRule
		WHERE	ConditionType = 2 AND
				CharIndex(EmailDispatchRule.Condition, @MsgFrom) > 0
	END
	
	-- 3. Domain
	IF @RuleID IS NULL			
	BEGIN
		SELECT	TOP 1 @RuleID = EmailDispatchRule.Id 
		FROM	EmailDispatchRule
		WHERE	ConditionType = 3 AND
				CharIndex(EmailDispatchRule.Condition, @MsgFrom) > 0
	END
	
	-- 4. MsgSubject
	IF @RuleID IS NULL			
	BEGIN
		SELECT	TOP 1 @RuleID = EmailDispatchRule.Id 
		FROM	EmailDispatchRule
		WHERE	ConditionType = 4 AND
				CharIndex(EmailDispatchRule.Condition, @MsgSubject) > 0
	END
	
	-- Return the result of the function
	RETURN @RuleID

END
GO
/****** Object:  StoredProcedure [dbo].[P_EmailDispatchRuleProcess]    Script Date: 03/25/2014 21:28:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kevin Chen
-- Create date: 2014/03/15
-- Description:	郵件派送規則
-- =============================================
CREATE PROCEDURE [dbo].[P_EmailDispatchRuleProcess]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    -- 0.0 找出派送的基數, 處理期限長度
    DECLARE @BaseValue INT, @ExpireLen INT;
    SELECT TOP 1 @BaseValue = ISNULL(Convert(INT, Value), 10) FROM EmailScheduleSetting WHERE Name = 'Base';
    IF @BaseValue IS NULL OR @BaseValue < 1
		SELECT @BaseValue = 10;
	
	SELECT TOP 1 @ExpireLen = ISNULL(Convert(INT,Value), 4) FROM EmailScheduleSetting WHERE Name='ExpireLen';
    IF @ExpireLen IS NULL OR @ExpireLen < 1
		SELECT @ExpireLen = 4;
		
    -- 0.1 目前郵件值機的客服派送數量記錄表 [行數][客服ID][目前派送數量][最大派送數量限制][客服所屬群組]
    DECLARE @DispatchedInfo TABLE (RowNumber INT, AgentID VARCHAR(8), CurCount INT, MaxCount INT, MailGroup VARCHAR(MAX)); 
    INSERT INTO @DispatchedInfo
    SELECT	ROW_NUMBER() OVER (ORDER BY Agent.AgentID) AS RowNumber,
			Agent.AgentID, 
			0, 
			FLOOR(@BaseValue*ISNULL(EmailAgentWeight.Weight, 1)), MailMember.GroupID
    FROM	Agent LEFT JOIN 
			EmailAgentWeight ON Agent.AgentID = EmailAgentWeight.AgentId LEFT JOIN
			MailMember ON Agent.CTILoginID = MailMember.CTILoginID
    WHERE	EmailIsLogin = 1;

	-------------------------------------------------------------        	
    -- 1. 掃描目前還未派送的郵件(不是回覆郵件): 專屬派送
    DECLARE @EmailRecordID INT, @EmailRawHeaderID INT, @MailGroup VARCHAR(MAX);
	DECLARE dataCursor CURSOR FAST_FORWARD FOR
	SELECT	EmailRecord.Id, 
			EmailRecord.RawHeaderId,
			MailGroup = CASE 
						 WHEN CharIndex('mailserver98@juntai.com.tw', MsgTo) > 0 THEN 'Enterprise'
						 ELSE 'Personal'
						END
	FROM	EmailRecord LEFT JOIN 
			EmailRawHeader ON EmailRecord.RawHeaderId = EmailRawHeader.Id
	WHERE	EmailRecord.RID Is NULL AND 
			(PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]]%',EmailRawHeader.MsgSubject) < 1 OR
			 PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_B[0-9][0-9]]%',EmailRawHeader.MsgSubject) < 1) AND				
			(EmailRecord.Garbage Is NULL OR EmailRecord.Garbage < 1);
	
	OPEN dataCursor;  	
	FETCH NEXT FROM dataCursor INTO @EmailRecordID, @EmailRawHeaderID, @MailGroup;
	
	DECLARE @RID BIGINT, @RID_RECORD VARCHAR(MAX);
    DECLARE @InitAgentID VARCHAR(8), @CurrAgentID VARCHAR(8), @OrderNo INT, @AssignCount INT, @ForwardCount INT;
    DECLARE @CurrCount INT;
    DECLARE @AgentID VARCHAR(30), @CustomerID uniqueidentifier, @AutoDispatch BIT;
    
    -- 1.1 開始逐步檢查
    DECLARE @CurIndexPersonal INT, @CurIndexEnterprise INT, @CurIndex INT, @AllDispatched INT;
	SELECT @CurIndexPersonal = 0, @CurIndexEnterprise = 0, @CurIndex = 0, @AllDispatched = 0;
	WHILE @@FETCH_STATUS = 0 AND @AllDispatched < 1
	BEGIN
		
		-- 1.1.1 新增案件ID
		SELECT TOP 1 @RID=IsNull(MAX(CONVERT(BIGINT, RID)),CONVERT(BIGINT, CONVERT(CHAR(8), GETDATE(), 112))*100000)+1 
		FROM (SELECT RID = CASE WHEN CHARINDEX('_B', RID) > 0 THEN '0' ELSE RID END FROM Records) AS T
		WHERE CONVERT(bigint, T.RID) > CONVERT(BIGINT, CONVERT(CHAR(8), GETDATE(), 112))*100000
	
		-- 1.1.2 有符合的專屬規則
		DECLARE @SpecificRuleID INT;
		SELECT @SpecificRuleID = [dbo].[F_GetSpecificRule](@EmailRawHeaderID);
		IF @SpecificRuleID IS NOT NULL
		BEGIN
			-- 1.1.2.1 檢查客服順位, 必需 a)有值機, b)派送數量未達最大限制
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
			
			-- 1.1.2.2 沒有可以指派的客服順位, 但是有設定自動派送
			DECLARE @NeedInsert INT;
			SELECT @NeedInsert = 1;
			IF @AgentID IS NULL AND (@AutoDispatch IS NOT NULL OR @AutoDispatch > 0)
			BEGIN
				-- 1.1.2.2.1 依郵件族群, 設定索引位置
				IF @MailGroup = 'Personal'
					SELECT @CurIndex = @CurIndexPersonal;
				ELSE
					SELECT @CurIndex = @CurIndexEnterprise;
					
				IF NOT EXISTS(SELECT 1 FROM @DispatchedInfo WHERE RowNumber > @CurIndex AND MailGroup = @MailGroup)
					SELECT @CurIndex = 0;
				
				-- 1.1.2.2.2 找出可以指派的客服人員, 必需 a) 派送數量未達最大限制, b) 客服和郵件族群是相同的
				SELECT	TOP 1 @AgentID = AgentID, @CurIndex=RowNumber
				FROM	@DispatchedInfo
				WHERE	RowNumber > @CurIndex AND 
						MailGroup = @MailGroup AND
						CurCount < MaxCount
						
				IF @AgentID IS NULL
					SELECT @NeedInsert = 0, @CurIndex = 0;
				ELSE
					SELECT @NeedInsert = 1;
				
				IF @MailGroup = 'Personal'
					SELECT @CurIndexPersonal = @CurIndex;
				ELSE
					SELECT @CurIndexEnterprise = @CurIndex;
			END
		END
		
		IF @NeedInsert > 0
		BEGIN
			INSERT INTO [dbo].[Records] (RID, CustomerID, TypeID, AgentID, StatusID, Comment, ServiceGroupID, ServiceItemID, IncomingDT, FinishDT)
			SELECT	RID = Convert(nvarchar(16),@RID), 
					CustomerID = @CustomerID,
					TypeID = 3,
					AgentID = @AgentID, 
					StatusID = 1,
					Comment = NULL,
					ServiceGroupID = NULL,
					ServiceItemID = NULL,
					IncomingDT = GETDATE(),
					FinishDT = NULL;
				
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
		END
		
		-- 1.1.2.3 所有客服人員都超出最大派送數量
		IF NOT EXISTS(SELECT 1 FROM @DispatchedInfo WHERE CurCount < MaxCount)
			SELECT @AllDispatched = 1;
			
		FETCH NEXT FROM dataCursor INTO @EmailRecordID, @EmailRawHeaderID, @MailGroup;
	END
	
	CLOSE dataCursor;
	DEALLOCATE dataCursor;
	
	DECLARE @MsgFrom VARCHAR(MAX);
	-- 2. 掃描目前還未派送的郵件(不是回覆郵件): 一般派送
	DECLARE dataCursor CURSOR FAST_FORWARD FOR
	SELECT	EmailRecord.Id, 
			EmailRecord.RawHeaderId,
			MailGroup = CASE 
						 WHEN CharIndex('mailserver98@juntai.com.tw', MsgTo) > 0 THEN 'Enterprise'
						 ELSE 'Personal'
						END,
			EmailRawHeader.MsgFrom
	FROM	EmailRecord LEFT JOIN 
			EmailRawHeader ON EmailRecord.RawHeaderId = EmailRawHeader.Id
	WHERE	EmailRecord.RID Is NULL AND 
			(PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]]%',EmailRawHeader.MsgSubject) < 1 OR
			 PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_B[0-9][0-9]]%',EmailRawHeader.MsgSubject) < 1) AND				
			(EmailRecord.Garbage Is NULL OR EmailRecord.Garbage < 1);
	
	OPEN dataCursor;  	
	FETCH NEXT FROM dataCursor INTO @EmailRecordID, @EmailRawHeaderID, @MailGroup, @MsgFrom;
	
	WHILE @@FETCH_STATUS = 0 AND @AllDispatched < 1
	BEGIN
		
		-- 2.1.1 新增案件ID
		SELECT TOP 1 @RID=IsNull(MAX(CONVERT(BIGINT, RID)),CONVERT(BIGINT, CONVERT(CHAR(8), GETDATE(), 112))*100000)+1 
		FROM (SELECT RID = CASE WHEN CHARINDEX('_B', RID) > 0 THEN '0' ELSE RID END FROM Records) AS T
		WHERE CONVERT(bigint, T.RID) > CONVERT(BIGINT, CONVERT(CHAR(8), GETDATE(), 112))*100000
	
		-- 2.1.2 沒有符合的專屬規則
		SELECT @SpecificRuleID = [dbo].[F_GetSpecificRule](@EmailRawHeaderID);
		IF @SpecificRuleID IS NULL
		BEGIN
			-- 2.1.2.2.1 依郵件族群, 設定索引位置
			IF @MailGroup = 'Personal'
				SELECT @CurIndex = @CurIndexPersonal;
			ELSE
				SELECT @CurIndex = @CurIndexEnterprise;
					
			IF NOT EXISTS(SELECT 1 FROM @DispatchedInfo WHERE RowNumber > @CurIndex AND MailGroup = @MailGroup)
				SELECT @CurIndex = 0;
				
			-- 2.1.2.2.2 找出可以指派的客服人員, 必需 a) 派送數量未達最大限制, b) 客服和郵件族群是相同的
			SELECT	TOP 1 @AgentID = AgentID, @CurIndex=RowNumber
			FROM	@DispatchedInfo
			WHERE	RowNumber > @CurIndex AND 
					MailGroup = @MailGroup AND
					CurCount < MaxCount
						
			IF @AgentID IS NULL
				SELECT @NeedInsert = 0, @CurIndex = 0;				
			ELSE
				SELECT @NeedInsert = 1;
				
			IF @MailGroup = 'Personal'
				SELECT @CurIndexPersonal = @CurIndex;
			ELSE
				SELECT @CurIndexEnterprise = @CurIndex;
			IF @NeedInsert > 0
			BEGIN
				SELECT	TOP 1 @CustomerID = ID
				FROM	Customers
				WHERE	CHARINDEX(Contact1_Email1, @MsgFrom) > 0 OR CHARINDEX(Contact1_Email2, @MsgFrom) > 0 OR
						CHARINDEX(Contact2_Email1, @MsgFrom) > 0 OR CHARINDEX(Contact2_Email2, @MsgFrom) > 0 OR
						CHARINDEX(Contact3_Email1, @MsgFrom) > 0 OR CHARINDEX(Contact3_Email2, @MsgFrom) > 0
						
				INSERT INTO [dbo].[Records] (RID, CustomerID, TypeID, AgentID, StatusID, Comment, ServiceGroupID, ServiceItemID, IncomingDT, FinishDT)
				SELECT	RID = Convert(nvarchar(16),@RID), 
						CustomerID = @CustomerID,
						TypeID = 3,
						AgentID = @AgentID, 
						StatusID = 1,
						Comment = NULL,
						ServiceGroupID = NULL,
						ServiceItemID = NULL,
						IncomingDT = GETDATE(),
						FinishDT = NULL;
				
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
GO
/****** Object:  StoredProcedure [dbo].[P_ReplyDispatchRuleProcess]    Script Date: 03/25/2014 21:28:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kevin Chen
-- Create date: 2014/03/15
-- Description:	回覆信件的派送規則
-- =============================================
CREATE PROCEDURE [dbo].[P_ReplyDispatchRuleProcess]
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
    
	DECLARE dataCursor CURSOR FAST_FORWARD FOR
	SELECT	EmailRecord.Id,
			SUBSTRING(EmailRawHeader.MsgSubject,NULLIF(PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]]%',EmailRawHeader.MsgSubject),0),13),
			SUBSTRING(MsgSubject,NULLIF(PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_B[0-9][0-9]]%',EmailRawHeader.MsgSubject),0),17),
			MsgSubject,
			EmailRecord.RawHeaderId
	FROM	EmailRecord LEFT JOIN 
			EmailRawHeader ON EmailRecord.RawHeaderId = EmailRawHeader.Id
	WHERE	EmailRecord.RID Is NULL AND 
			(PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]]%',EmailRawHeader.MsgSubject) > 0 OR
			 PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_B[0-9][0-9]]%',EmailRawHeader.MsgSubject) > 0) AND				
			(EmailRecord.Garbage Is NULL OR EmailRecord.Garbage < 1);
	
	OPEN dataCursor;  	
	FETCH NEXT FROM dataCursor INTO @EmailRecordID, @RID_REPLY, @RID_FORWARD, @MsgSubject, @RawHeaderID;
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- 1.1 找出RID
		SELECT @RID_RECORD = CASE 
								WHEN @RID_REPLY IS NOT NULL AND LEN(@RID_REPLY) > 0 THEN @RID_REPLY
								ELSE @RID_FORWARD
							 END;
		
		-- 1.2 找出處理期限參數
		SELECT TOP 1 @ExpireLen = ISNULL(Convert(INT,Value), 4) FROM EmailScheduleSetting WHERE Name='ExpireLen';
		IF @ExpireLen IS NULL OR @ExpireLen < 1
		SELECT @ExpireLen = 4;
		
		-- 1.3 找出其它需同步的參數
		DECLARE @CustomerID uniqueidentifier, @StatusID INT;
		SELECT TOP 1 @CustomerID = CustomerID, @StatusID=StatusID FROM Records WHERE CHARINDEX(RID, @RID_RECORD) > 0;
		SELECT		TOP 1 @CurrAgentID = CurrAgentId,
					@InitAgentID = InitAgentId,
					@AssignCount = AssignCount,
					@ForwardCount = ForwardCount
		FROM		EmailRecord 
		WHERE		CHARINDEX(RID, @RID_RECORD) > 0
		ORDER BY	OrderNo DESC;		
		
		SELECT	TOP 1 @OrderNo = ISNULL(OrderNo, -1)+1 FROM EmailRecord WHERE RID = @RID_RECORD ORDER BY OrderNo DESC;
		IF @OrderNo IS NULL
			SELECT @OrderNo = 0;
		
		IF EXISTS(SELECT 1 FROM Records WHERE CHARINDEX(RID, @RID_RECORD) > 0 AND StatusID < 3)
		BEGIN
			IF @OrderNo < 1
			BEGIN
				INSERT INTO [dbo].[Records] (RID, CustomerID, TypeID, AgentID, StatusID, Comment, ServiceGroupID, ServiceItemID, IncomingDT, FinishDT)
				SELECT	RID = @RID_RECORD, 
						CustomerID = @CustomerID,
						TypeID = 3,
						AgentID = @CurrAgentID, 
						StatusID = @StatusID,
						Comment = NULL,
						ServiceGroupID = NULL,
						ServiceItemID = NULL,
						IncomingDT = GETDATE(),
						FinishDT = NULL;
			END
			
			
			IF EXISTS(	SELECT 1 
						FROM Records LEFT JOIN Agent ON Records.AgentID = Agent.AgentID 
						WHERE RID=@RID_RECORD AND Records.AgentID IS NOT NULL AND Agent.EmailIsLogin = 0)
			BEGIN
				UPDATE	Records
				SET		AgentID = NULL
				WHERE	CHARINDEX(RID, @RID_RECORD) > 0;
			END
								
			
			
			UPDATE	EmailRecord 
			SET		RID = @RID_RECORD, 
					ExpireOn = dateadd(hh,@ExpireLen,GETDATE()), 
					IncomeOn = GETDATE(),
					AssignOn = GETDATE(),
					InitAgentID = @InitAgentID,
					CurrAgentID = @CurrAgentID,
					AssignCount = @AssignCount,
					ForwardCount = @ForwardCount,
					OrderNo = @OrderNo,
					UnRead = 1
			WHERE	Id = @EmailRecordId;		
			
		END
		ELSE
		BEGIN
			DECLARE @RID BIGINT;
			SELECT TOP 1 @RID=IsNull(MAX(CONVERT(BIGINT, RID)),CONVERT(BIGINT, CONVERT(CHAR(8), GETDATE(), 112))*100000)+1 
			FROM (SELECT RID = CASE WHEN CHARINDEX('_B', RID) > 0 THEN '0' ELSE RID END FROM Records) AS T
			WHERE CONVERT(bigint, T.RID) > CONVERT(BIGINT, CONVERT(CHAR(8), GETDATE(), 112))*100000;
			
			INSERT INTO [dbo].[Records] (RID, CustomerID, TypeID, AgentID, StatusID, Comment, ServiceGroupID, ServiceItemID, IncomingDT, FinishDT)
			SELECT	RID = Convert(nvarchar(MAX),@RID), 
					CustomerID = @CustomerID,
					TypeID = 3,
					AgentID = @CurrAgentID, 
					StatusID = 1,
					Comment = NULL,
					ServiceGroupID = NULL,
					ServiceItemID = NULL,
					IncomingDT = GETDATE(),
					FinishDT = NULL;
					
			UPDATE	EmailRecord 
			SET		RID = Convert(nvarchar(MAX),@RID), 
					ExpireOn = dateadd(hh,@ExpireLen,GETDATE()), 
					IncomeOn = GETDATE(),
					AssignOn = GETDATE(),
					InitAgentID = @InitAgentID,
					CurrAgentID = @CurrAgentID,
					AssignCount = @AssignCount,
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
		FETCH NEXT FROM dataCursor INTO @EmailRecordID, @RID_REPLY, @RID_FORWARD, @MsgSubject, @RawHeaderID;
	END
	
	CLOSE dataCursor;
	DEALLOCATE dataCursor;
END
GO
/****** Object:  StoredProcedure [dbo].[P_EmailDispatchForSchedule]    Script Date: 03/25/2014 21:28:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE PROCEDURE  [dbo].[P_EmailDispatchForSchedule]
AS
BEGIN

	EXEC [dbo].[P_ReplyDispatchRuleProcess];
	
	EXEC [dbo].[P_EmailDispatchRuleProcess];
	
END
GO
