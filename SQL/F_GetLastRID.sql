USE [ksky]
GO
/****** Object:  UserDefinedFunction [dbo].[F_GetLastRID]    Script Date: 05/22/2014 12:47:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
ALTER FUNCTION [dbo].[F_GetLastRID]
(
	-- Add the parameters for the function here
	@Express VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ExpressLEN INT, @POS INT, @LASTPOS_REPLY INT, @LASTPOS_FORWARD INT;

	SELECT @POS = 0, @LASTPOS_REPLY = 0, @LASTPOS_FORWARD = 0, @ExpressLEN = LEN(@Express);
	
	-- Add the T-SQL statements to compute the return value here
	SELECT @POS = NULLIF(PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]]%',@Express), 0);
	WHILE(@POS > 0 AND @POS Is NOT NULL)
	BEGIN
		SELECT @LASTPOS_REPLY = @POS;
		SELECT @POS = NULLIF(PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]]%',SUBSTRING(@Express, @LASTPOS_REPLY+15, @ExpressLEN)), 0);
		SELECT @POS = @LASTPOS_REPLY + 14 + @POS;
	END
	
	SELECT @POS = NULLIF(PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_B[0-9][0-9]]%',@Express), 0);
	WHILE(@POS > 0 AND @POS Is NOT NULL)
	BEGIN
		SELECT @LASTPOS_FORWARD = @POS;
		SELECT @POS = NULLIF(PATINDEX('%[[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]_B[0-9][0-9]]%',SUBSTRING(@Express, @LASTPOS_FORWARD+19, @ExpressLEN)), 0);
		SELECT @POS = @LASTPOS_FORWARD + 18 + @POS;
	END
	
    DECLARE @RET VARCHAR(MAX);		
    SELECT @RET = CASE	WHEN @LASTPOS_REPLY > @LASTPOS_FORWARD THEN SUBSTRING(@Express,@LASTPOS_REPLY,13)
						WHEN @LASTPOS_REPLY < @LASTPOS_FORWARD THEN SUBSTRING(@Express,@LASTPOS_FORWARD,17)
						ELSE NULL
						END;
	RETURN @RET;

END
