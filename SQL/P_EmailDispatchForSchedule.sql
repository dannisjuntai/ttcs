USE [ksky]
GO
/****** Object:  StoredProcedure [dbo].[P_EmailDispatchForSchedule]    Script Date: 07/31/2014 22:57:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
ALTER PROCEDURE  [dbo].[P_EmailDispatchForSchedule]
AS
BEGIN

	EXEC [dbo].[P_ReplyDispatchRuleProcess];
	
	EXEC [dbo].[P_EmailDispatchRuleProcess];
	
END
