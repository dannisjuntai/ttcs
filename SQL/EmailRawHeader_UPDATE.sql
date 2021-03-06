USE [ksky]
GO
/****** Object:  Trigger [dbo].[EmailRawHeader_UPDATE]    Script Date: 07/31/2014 22:56:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Kevin
-- Create date: 2014/03/15
-- Description:	郵件被成功收進來的後續流程
-- =============================================
ALTER TRIGGER [dbo].[EmailRawHeader_UPDATE] 
   ON  [dbo].[EmailRawHeader] 
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

    -- Insert statements for trigger here
    DECLARE @RawHeaderID INT, @MsgSubject NVARCHAR(MAX), @MsgSentOn DATETIME;
    DECLARE @MaxCount INT;
    SELECT @MaxCount = 0;
    DECLARE dataCursor2 CURSOR FAST_FORWARD FOR
	SELECT	Id, MsgSubject, MsgSentOn
	FROM	inserted
	WHERE	Completed = 1;
	
	OPEN dataCursor2;  	
	FETCH NEXT FROM dataCursor2 INTO @RawHeaderID, @MsgSubject, @MsgSentOn;	
	WHILE @@FETCH_STATUS = 0 AND @MaxCount < 1
	BEGIN
		IF NOT EXISTS(SELECT 1 FROM EmailRawHeader WHERE Id <> @RawHeaderID AND MsgSubject = @MsgSubject AND MsgSentOn = @MsgSentOn) AND
		   NOT EXISTS(SELECT 1 FROM EmailRecord WHERE RawHeaderID = @RawHeaderID)
			BEGIN
				IF (PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]]%',@MsgSubject) > 0 OR
					PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_B[0-9][0-9]]%',@MsgSubject) > 0)
				BEGIN
					INSERT INTO [dbo].[EmailRecord] (RawHeaderId,Garbage, UnRead) VALUES (@RawHeaderID, NULL, 1);
				END
				ELSE
				BEGIN
					INSERT INTO [dbo].[EmailRecord] (RawHeaderId,Garbage) VALUES (@RawHeaderID, [dbo].[F_GetFilterRule](@RawHeaderID));
				END
			END
		SELECT @MaxCount = @MaxCount + 1;
		FETCH NEXT FROM dataCursor2 INTO @RawHeaderID, @MsgSubject, @MsgSentOn;
	END
	
	CLOSE dataCursor2;
	DEALLOCATE dataCursor2;
	
END