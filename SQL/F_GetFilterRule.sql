USE [ksky]
GO
/****** Object:  UserDefinedFunction [dbo].[F_GetFilterRule]    Script Date: 05/07/2014 18:53:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kevin Chen
-- Create date: 2014/03/14
-- Description:	是否符合過濾規則
-- =============================================
ALTER FUNCTION [dbo].[F_GetFilterRule] 
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
			((PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]]%',@MsgSubject) < 1 AND
			  PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_B[0-9][0-9]]%',@MsgSubject) < 1 AND	
			  CHARINDEX(LTrim(RTrim(MsgSubject)), @MsgSubject) > 0) OR
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
