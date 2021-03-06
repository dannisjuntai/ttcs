USE [ksky]
GO
/****** Object:  Table [dbo].[RecordType]    Script Date: 04/08/2014 23:49:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON

GO
INSERT [dbo].[RecordType] ([ID], [Name]) VALUES (1, N'來電')
INSERT [dbo].[RecordType] ([ID], [Name]) VALUES (2, N'外撥')
INSERT [dbo].[RecordType] ([ID], [Name]) VALUES (3, N'郵件')
GO
INSERT [dbo].[RecordStatus] ([ID], [Name]) VALUES (1, N'未處理')
INSERT [dbo].[RecordStatus] ([ID], [Name]) VALUES (2, N'處理中')
INSERT [dbo].[RecordStatus] ([ID], [Name]) VALUES (3, N'已結案')
GO

SET IDENTITY_INSERT [dbo].[EmailReplyCan] ON
INSERT [dbo].[EmailReplyCan] ([Id], [Name], [TempCnt], [Active]) VALUES (1, N'個人版簽名檔', 0x, 1)
INSERT [dbo].[EmailReplyCan] ([Id], [Name], [TempCnt], [Active]) VALUES (2, N'企業版簽名檔', 0x, 1)
SET IDENTITY_INSERT [dbo].[EmailReplyCan] OFF
GO

INSERT [dbo].[EmailAgentWeight]
SELECT AgentID, 1 FROM [dbo].[Agent]
GO