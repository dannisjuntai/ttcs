USE [ksky]
GO
/****** Object:  Table [dbo].[ServiceGroup]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceGroup](
	[GroupID] [int] NOT NULL,
	[GroupDesc] [nvarchar](32) NOT NULL,
	[DispNo] [int] NULL,
	[DeptID] [nvarchar](32) NULL,
 CONSTRAINT [PK_ServiceGroup] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Resellers]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Resellers](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Address] [nvarchar](256) NULL,
	[Comment] [nvarchar](max) NULL,
	[Contact1_Name] [nvarchar](256) NULL,
	[Contact1_Title] [nvarchar](256) NULL,
	[Contact1_Tel] [nvarchar](256) NULL,
	[Contact1_Email] [nvarchar](256) NULL,
	[Contact2_Name] [nvarchar](256) NULL,
	[Contact2_Title] [nvarchar](256) NULL,
	[Contact2_Tel] [nvarchar](256) NULL,
	[Contact2_Email] [nvarchar](256) NULL,
	[Contact3_Name] [nvarchar](256) NULL,
	[Contact3_Title] [nvarchar](256) NULL,
	[Contact3_Tel] [nvarchar](256) NULL,
	[Contact3_Email] [nvarchar](256) NULL,
 CONSTRAINT [PK_Resellers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecordType]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordType](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](16) NOT NULL,
 CONSTRAINT [PK_RecordType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件類型識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RecordType', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件類型名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RecordType', @level2type=N'COLUMN',@level2name=N'Name'
GO
/****** Object:  Table [dbo].[RecordStatus]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordStatus](
	[ID] [int] NOT NULL,
	[Name] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_RecordStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件狀態識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RecordStatus', @level2type=N'COLUMN',@level2name=N'ID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件狀態名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RecordStatus', @level2type=N'COLUMN',@level2name=N'Name'
GO
/****** Object:  Table [dbo].[EmailSentBox]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailSentBox](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RID] [nvarchar](20) NOT NULL,
	[OrderNo] [int] NOT NULL,
	[FileName] [nvarchar](max) NULL,
	[CodePage] [int] NULL,
	[BodyCnt] [varbinary](max) NULL,
	[SendTo] [nvarchar](max) NULL,
	[SentOn] [datetime] NULL,
	[SentBy] [nvarchar](max) NULL,
	[MsgSubject] [nvarchar](max) NULL,
	[Cc] [nvarchar](max) NULL,
	[Bcc] [nvarchar](max) NULL,
 CONSTRAINT [PK_EmailSentBox] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'寄件備份識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentBox', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentBox', @level2type=N'COLUMN',@level2name=N'RID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'同一個案件中,所有相關郵件的進來順序值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentBox', @level2type=N'COLUMN',@level2name=N'OrderNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件的附件檔名: NULL=此筆記錄為內文, 並非附加檔案' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentBox', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件內容編碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentBox', @level2type=N'COLUMN',@level2name=N'CodePage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件內容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentBox', @level2type=N'COLUMN',@level2name=N'BodyCnt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件收件者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentBox', @level2type=N'COLUMN',@level2name=N'SendTo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件寄送日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentBox', @level2type=N'COLUMN',@level2name=N'SentOn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件寄送者: Agent識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentBox', @level2type=N'COLUMN',@level2name=N'SentBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件主旨' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentBox', @level2type=N'COLUMN',@level2name=N'MsgSubject'
GO
/****** Object:  Table [dbo].[EmailSentAttachment]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailSentAttachment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmailRecordId] [int] NOT NULL,
	[FileName] [nvarchar](max) NOT NULL,
	[CntMedia] [nvarchar](max) NULL,
	[CntBody] [varbinary](max) NULL,
 CONSTRAINT [PK_EmailSentAttachment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmailScheduleSetting]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailScheduleSetting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Value] [varchar](50) NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_EmailScheduleSetting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統參數識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailScheduleSetting', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統參數名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailScheduleSetting', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統參數值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailScheduleSetting', @level2type=N'COLUMN',@level2name=N'Value'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系統參數是否啟用: 1=啟用, 0=不啟用 (此值僅作用於部份系統參數)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailScheduleSetting', @level2type=N'COLUMN',@level2name=N'Active'
GO
/****** Object:  Table [dbo].[EmailSavedBody]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailSavedBody](
	[Id] [nvarchar](50) NOT NULL,
	[FileName] [nvarchar](max) NULL,
	[CodePage] [int] NULL,
	[BodyCnt] [varbinary](max) NULL,
	[SendTo] [nvarchar](max) NULL,
	[Cc] [nvarchar](max) NULL,
	[Bcc] [nvarchar](max) NULL,
	[MailSignature] [int] NULL,
 CONSTRAINT [PK_EmailSavedBody] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件草稿識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedBody', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件草稿附件檔名稱: 無作用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedBody', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件草稿內容編碼代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedBody', @level2type=N'COLUMN',@level2name=N'CodePage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件草稿內容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedBody', @level2type=N'COLUMN',@level2name=N'BodyCnt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件草稿資訊: 即將寄送的電子信箱位址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedBody', @level2type=N'COLUMN',@level2name=N'SendTo'
GO
/****** Object:  Table [dbo].[EmailSavedAttachment]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailSavedAttachment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmailRecordId] [int] NOT NULL,
	[FileName] [nvarchar](max) NOT NULL,
	[CntMedia] [nvarchar](max) NULL,
	[CntBody] [varbinary](max) NULL,
 CONSTRAINT [PK_EmailSavedAttachment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'草稿附件識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedAttachment', @level2type=N'COLUMN',@level2name=N'Id'
GO
/****** Object:  Table [dbo].[EmailReplyCan]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailReplyCan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[TempCnt] [varbinary](max) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_EmailReplyCan] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件回覆範本識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailReplyCan', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件回覆範本名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailReplyCan', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件回覆範本內容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailReplyCan', @level2type=N'COLUMN',@level2name=N'TempCnt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件回覆範本是否啟用: 1=啟用, 0=不啟用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailReplyCan', @level2type=N'COLUMN',@level2name=N'Active'
GO
/****** Object:  Table [dbo].[EmailRejectReason]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailRejectReason](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Reason] [varchar](50) NOT NULL,
 CONSTRAINT [PK_EmailRejectReason] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件退回原因識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRejectReason', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件退回原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRejectReason', @level2type=N'COLUMN',@level2name=N'Reason'
GO
/****** Object:  Table [dbo].[MailMember]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MailMember](
	[CTILoginID] [varchar](32) NOT NULL,
	[GroupID] [varchar](32) NOT NULL,
 CONSTRAINT [PK_MailMember] PRIMARY KEY CLUSTERED 
(
	[CTILoginID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客服人員簽進代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailMember', @level2type=N'COLUMN',@level2name=N'CTILoginID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件群組代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailMember', @level2type=N'COLUMN',@level2name=N'GroupID'
GO
/****** Object:  Table [dbo].[MailGroup]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MailGroup](
	[GroupID] [varchar](32) NOT NULL,
	[GroupName] [nvarchar](32) NULL,
 CONSTRAINT [PK_MailGroup] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件群組代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailGroup', @level2type=N'COLUMN',@level2name=N'GroupID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件群組名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MailGroup', @level2type=N'COLUMN',@level2name=N'GroupName'
GO
/****** Object:  Table [dbo].[EmailFilterRule]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailFilterRule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MsgSubject] [nvarchar](max) NULL,
	[MsgBody] [nvarchar](max) NULL,
	[MsgFrom] [nvarchar](max) NULL,
	[MsgReceivedBy] [nvarchar](max) NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_EmailFilter] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件過濾識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailFilterRule', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'過濾條件:郵件主旨' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailFilterRule', @level2type=N'COLUMN',@level2name=N'MsgSubject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'過濾條件:郵件內文' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailFilterRule', @level2type=N'COLUMN',@level2name=N'MsgBody'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'過濾條件:郵件寄件者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailFilterRule', @level2type=N'COLUMN',@level2name=N'MsgFrom'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'過濾條件:寄件者IP' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailFilterRule', @level2type=N'COLUMN',@level2name=N'MsgReceivedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'過濾規則是否啟用: 1=啟用, 0=不啟用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailFilterRule', @level2type=N'COLUMN',@level2name=N'Active'
GO
/****** Object:  Table [dbo].[EmailContacts]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailContacts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ContactGroup] [nvarchar](max) NULL,
	[ContactName] [nvarchar](max) NULL,
	[ContactEmail] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_EmailContacts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方連絡人識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailContacts', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方連絡人群組' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailContacts', @level2type=N'COLUMN',@level2name=N'ContactGroup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方連絡人名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailContacts', @level2type=N'COLUMN',@level2name=N'ContactName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'第三方連絡人電子信箱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailContacts', @level2type=N'COLUMN',@level2name=N'ContactEmail'
GO
/****** Object:  Table [dbo].[EmailAgentWeight]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailAgentWeight](
	[AgentId] [varchar](8) NOT NULL,
	[Weight] [float] NOT NULL,
 CONSTRAINT [PK_EmailAgentWeight] PRIMARY KEY CLUSTERED 
(
	[AgentId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Agent識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailAgentWeight', @level2type=N'COLUMN',@level2name=N'AgentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'派送數量權重' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailAgentWeight', @level2type=N'COLUMN',@level2name=N'Weight'
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customers](
	[ID] [uniqueidentifier] NOT NULL,
	[CName] [nvarchar](64) NOT NULL,
	[EName] [nvarchar](128) NULL,
	[Category] [nvarchar](16) NULL,
	[ID2] [nvarchar](32) NULL,
	[Region] [nvarchar](256) NULL,
	[City] [nvarchar](256) NULL,
	[Address] [nvarchar](256) NULL,
	[Comment] [nvarchar](max) NULL,
	[Marketer] [varchar](64) NULL,
	[AgentID1] [nvarchar](64) NULL,
	[AgentID2] [nvarchar](64) NULL,
	[Enterprise] [int] NULL,
	[Contact1_Name] [nvarchar](16) NULL,
	[Contact1_Title] [nvarchar](16) NULL,
	[Contact1_Tel1] [nvarchar](16) NULL,
	[Contact1_Tel2] [nvarchar](16) NULL,
	[Contact1_Email1] [nvarchar](64) NULL,
	[Contact1_Email2] [nvarchar](64) NULL,
	[Contact1_DoSend] [char](1) NULL,
	[Contact2_Name] [nvarchar](16) NULL,
	[Contact2_Title] [nvarchar](16) NULL,
	[Contact2_Tel1] [nvarchar](16) NULL,
	[Contact2_Tel2] [nvarchar](16) NULL,
	[Contact2_Email1] [nvarchar](64) NULL,
	[Contact2_Email2] [nvarchar](64) NULL,
	[Contact2_DoSend] [char](1) NULL,
	[Contact3_Name] [nvarchar](16) NULL,
	[Contact3_Title] [nvarchar](16) NULL,
	[Contact3_Tel1] [nvarchar](16) NULL,
	[Contact3_Tel2] [nvarchar](16) NULL,
	[Contact3_Email1] [nvarchar](64) NULL,
	[Contact3_Email2] [nvarchar](64) NULL,
	[Contact3_DoSend] [char](1) NULL,
	[Modifier] [nvarchar](64) NULL,
	[ModifiedDT] [datetime] NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'中文名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'CName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'英文名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'EName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶分類' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'Category'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'身分證字號 / 營利事業登記證' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'ID2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶區域' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'Region'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶所在城市' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'City'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'Address'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'Comment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'業務員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'Marketer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'負責客服人員1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'AgentID1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'負責客服人員2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'AgentID2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為企業客戶' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'Enterprise'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'聯絡人1姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'Contact1_Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'聯絡人1稱謂' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'Contact1_Title'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'聯絡人1電話1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'Contact1_Tel1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'聯絡人1電話2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'Contact1_Tel2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'聯絡人1郵件1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'Contact1_Email1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'聯絡人1 郵件2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'Contact1_Email2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'Modifier'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後更新時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'ModifiedDT'
GO
/****** Object:  Table [dbo].[Agent]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Agent](
	[AgentID] [varchar](8) NOT NULL,
	[AgentName] [nvarchar](32) NULL,
	[AgentPWD] [varchar](8) NULL,
	[VMPWD] [varchar](8) NULL,
	[HasVoiceMail] [int] NULL,
	[Email] [varchar](64) NULL,
	[GW32] [varchar](6) NULL,
	[Authority] [int] NULL,
	[CTILoginID] [varchar](20) NOT NULL,
	[CTILoginPWD] [varchar](50) NULL,
	[Name] [nvarchar](20) NULL,
	[AutoLogin] [int] NULL,
	[AutoLogout] [int] NULL,
	[DefaultExten] [varchar](10) NULL,
	[AutoLoginTime] [datetime] NULL,
	[AutoLogoutTime] [datetime] NULL,
	[LocalDistance] [tinyint] NULL,
	[LongDistance] [tinyint] NULL,
	[Mobil] [tinyint] NULL,
	[International] [tinyint] NULL,
	[DialContext] [varchar](32) NULL,
	[Active] [tinyint] NULL,
	[DeptID] [varchar](32) NULL,
	[SMSEnable] [tinyint] NULL,
	[FaxEnable] [tinyint] NULL,
	[EmailEnable] [tinyint] NULL,
	[ChatEnable] [tinyint] NULL,
	[GrpRecBoxEnable] [tinyint] NULL,
	[EmailIsLogin] [tinyint] NULL,
 CONSTRAINT [PK_Agent] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'電子郵件位址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'權限: 0=一般客服員,1=一般管理員,2=系統管理員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'Authority'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客服人員姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'在職狀態: 1=在職,0=離職' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'Active'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件值機簽進狀態: 1=值機, 0=簽退' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Agent', @level2type=N'COLUMN',@level2name=N'EmailIsLogin'
GO
/****** Object:  Table [dbo].[EmailRawHeader]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailRawHeader](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MsgId] [nvarchar](max) NULL,
	[MsgFrom] [nvarchar](max) NULL,
	[MsgTo] [nvarchar](max) NULL,
	[MsgCc] [nvarchar](max) NULL,
	[MsgBcc] [nvarchar](max) NULL,
	[MsgReplyTo] [nvarchar](max) NULL,
	[MsgReceivedBy] [nvarchar](max) NULL,
	[MsgInReplyTo] [nvarchar](max) NULL,
	[MsgSubject] [nvarchar](max) NULL,
	[MsgSentOn] [datetime] NOT NULL,
	[Completed] [int] NOT NULL,
 CONSTRAINT [PK_EmailRecords_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件標頭檔識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawHeader', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件標頭資訊: 郵件代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawHeader', @level2type=N'COLUMN',@level2name=N'MsgId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件標頭資訊: 郵件寄件者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawHeader', @level2type=N'COLUMN',@level2name=N'MsgFrom'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件標頭資訊: 郵件收件者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawHeader', @level2type=N'COLUMN',@level2name=N'MsgTo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件標頭資訊: 郵件副本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawHeader', @level2type=N'COLUMN',@level2name=N'MsgCc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件標頭資訊: 郵件密件副本' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawHeader', @level2type=N'COLUMN',@level2name=N'MsgBcc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件標頭資訊: 郵件回覆的郵件代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawHeader', @level2type=N'COLUMN',@level2name=N'MsgReplyTo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件標頭資訊: 郵件來源, 通常為IP或SERVER NAME' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawHeader', @level2type=N'COLUMN',@level2name=N'MsgReceivedBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件標頭資訊: 郵件回覆之內部郵件代碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawHeader', @level2type=N'COLUMN',@level2name=N'MsgInReplyTo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件標頭資訊: 郵件主旨' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawHeader', @level2type=N'COLUMN',@level2name=N'MsgSubject'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件標頭資訊: 郵件寄送日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawHeader', @level2type=N'COLUMN',@level2name=N'MsgSentOn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否下載完全: 0=下載完全, 1=下載不全完' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawHeader', @level2type=N'COLUMN',@level2name=N'Completed'
GO
/****** Object:  Table [dbo].[EmailRawBody]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailRawBody](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RawHeaderId] [int] NOT NULL,
	[PlaceHolder] [nvarchar](2) NULL,
	[CntMedia] [nvarchar](20) NULL,
	[CodePage] [int] NOT NULL,
	[FileName] [nvarchar](256) NULL,
	[BinaryCnt] [varbinary](max) NULL,
 CONSTRAINT [PK_EmailBodys] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件內文區塊識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawBody', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件標頭識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawBody', @level2type=N'COLUMN',@level2name=N'RawHeaderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件內文區塊順序編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawBody', @level2type=N'COLUMN',@level2name=N'PlaceHolder'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件內文區塊之MIME值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawBody', @level2type=N'COLUMN',@level2name=N'CntMedia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件內文區塊編碼代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawBody', @level2type=N'COLUMN',@level2name=N'CodePage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件內文區塊檔案名稱: 若區塊內容為附件, 則此欄位有值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawBody', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件區塊內容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRawBody', @level2type=N'COLUMN',@level2name=N'BinaryCnt'
GO
/****** Object:  Table [dbo].[EmailDispatchRule]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailDispatchRule](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ConditionType] [int] NULL,
	[CustomerId] [uniqueidentifier] NULL,
	[Condition] [nvarchar](max) NULL,
	[AgentId1] [varchar](8) NOT NULL,
	[AgentId2] [varchar](8) NULL,
	[AgentId3] [varchar](8) NULL,
	[ModifyOn] [datetime] NULL,
	[ModifyBy] [nchar](8) NULL,
	[AutoDispatch] [bit] NULL,
 CONSTRAINT [PK_EmailDispatchRule] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件派送規則識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailDispatchRule', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'派送依據類型: 1=客戶, 2=電子信箱, 3=郵件網域, 4=郵件主旨' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailDispatchRule', @level2type=N'COLUMN',@level2name=N'ConditionType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶識別碼: 僅作用於派送依據類型為1之時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailDispatchRule', @level2type=N'COLUMN',@level2name=N'CustomerId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'派送條件: 作用於派送類型為2, 3, 4之時' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailDispatchRule', @level2type=N'COLUMN',@level2name=N'Condition'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客服順位1: Agent識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailDispatchRule', @level2type=N'COLUMN',@level2name=N'AgentId1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客服順位2: Agent識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailDispatchRule', @level2type=N'COLUMN',@level2name=N'AgentId2'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客服順位3: Agent識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailDispatchRule', @level2type=N'COLUMN',@level2name=N'AgentId3'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailDispatchRule', @level2type=N'COLUMN',@level2name=N'ModifyOn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最後修改人員: Agent識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailDispatchRule', @level2type=N'COLUMN',@level2name=N'ModifyBy'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'全部客服人員未值機時, 是否啟動自動派送: 1=啟動, 0=不啟動' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailDispatchRule', @level2type=N'COLUMN',@level2name=N'AutoDispatch'
GO
/****** Object:  Table [dbo].[Trades]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trades](
	[ID] [uniqueidentifier] NOT NULL,
	[CustomerID] [uniqueidentifier] NOT NULL,
	[ResellerID] [uniqueidentifier] NOT NULL,
	[EmployeeID] [nvarchar](30) NULL,
	[Comment] [nvarchar](max) NULL,
	[ProductName] [nvarchar](256) NULL,
	[Category] [nvarchar](64) NULL,
	[TradeDate] [date] NULL,
	[ExpireDate] [date] NULL,
	[OrderNo] [nvarchar](64) NULL,
	[LicenseNum] [nvarchar](64) NULL,
	[UnitPrice] [int] NULL,
	[Price] [int] NULL,
	[Discount] [int] NULL,
	[PreOrder] [nvarchar](64) NULL,
	[NextOrder] [nvarchar](64) NULL,
	[PreVendor] [nvarchar](64) NULL,
	[ReNewEmailDT] [datetime] NULL,
	[ReNewCallDT] [datetime] NULL,
	[ReNewMailDT] [datetime] NULL,
	[ReNewComment] [nvarchar](max) NULL,
 CONSTRAINT [PK_Trades] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceItem]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ServiceItem](
	[GroupID] [int] NOT NULL,
	[ItemID] [int] NOT NULL,
	[ItemDesc] [varchar](64) NULL,
	[DispNo] [int] NULL,
	[CRM_ID] [varbinary](8) NULL,
 CONSTRAINT [PK_ServiceItem] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC,
	[ItemID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Records]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Records](
	[RID] [nvarchar](20) NOT NULL,
	[CustomerID] [uniqueidentifier] NULL,
	[TypeID] [int] NULL,
	[AgentID] [nvarchar](64) NULL,
	[StatusID] [int] NULL,
	[Comment] [nvarchar](max) NULL,
	[ServiceGroupID] [int] NULL,
	[ServiceItemID] [int] NULL,
	[IncomingDT] [datetime] NULL,
	[FinishDT] [datetime] NULL,
 CONSTRAINT [PK_Records] PRIMARY KEY CLUSTERED 
(
	[RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件編號: 西元年+月+日+流水號, ex, 2014032100001' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Records', @level2type=N'COLUMN',@level2name=N'RID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客戶識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Records', @level2type=N'COLUMN',@level2name=N'CustomerID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件類型識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Records', @level2type=N'COLUMN',@level2name=N'TypeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'處理案件的Agent識別碼: 郵件類型案件, 會出現NULL值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Records', @level2type=N'COLUMN',@level2name=N'AgentID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件狀態識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Records', @level2type=N'COLUMN',@level2name=N'StatusID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Records', @level2type=N'COLUMN',@level2name=N'Comment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件小結群組識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Records', @level2type=N'COLUMN',@level2name=N'ServiceGroupID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件小結選項識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Records', @level2type=N'COLUMN',@level2name=N'ServiceItemID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件進件時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Records', @level2type=N'COLUMN',@level2name=N'IncomingDT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件結案時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Records', @level2type=N'COLUMN',@level2name=N'FinishDT'
GO
/****** Object:  UserDefinedFunction [dbo].[F_GetSpecificRule]    Script Date: 03/23/2014 23:40:22 ******/
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
/****** Object:  UserDefinedFunction [dbo].[F_GetFilterRule]    Script Date: 03/23/2014 23:40:22 ******/
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
/****** Object:  Table [dbo].[EmailRecord]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailRecord](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RID] [nvarchar](20) NULL,
	[RawHeaderId] [int] NULL,
	[ExpireOn] [datetime] NULL,
	[ReplyOn] [datetime] NULL,
	[IncomeOn] [datetime] NULL,
	[AssignOn] [datetime] NULL,
	[InitAgentId] [varchar](8) NULL,
	[CurrAgentId] [varchar](8) NULL,
	[AssignCount] [int] NULL,
	[Garbage] [int] NULL,
	[RejectReason] [nvarchar](max) NULL,
	[ProcessStatus] [nvarchar](max) NULL,
	[OrderNo] [int] NULL,
	[SaveCntIn] [nvarchar](50) NULL,
	[ForwardCount] [int] NULL,
	[UnRead] [int] NULL,
	[MsgSubjectModifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_EmailRecord] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件內部識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件編號: NULL=未指派' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'RID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'原始郵件標頭識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'RawHeaderId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 逾期時間, NULL=未指派' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'ExpireOn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 回覆時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'ReplyOn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 進件時間, NULL=未指派' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'IncomeOn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 指派或轉派時間, 僅記錄最後一次行為時間, NULL=未指派' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'AssignOn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 第一個處理的Agent識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'InitAgentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 目前或者最後處理的Agent識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'CurrAgentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 指派次數(包含轉派)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'AssignCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 垃圾郵件標示, 1=垃圾郵件, 其它=非垃圾郵件(包含0及NULL)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'Garbage'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 退回原因, 僅記錄最後一次退回原因' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'RejectReason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 郵件處理狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'ProcessStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 同屬相同案件的所有郵件進來順序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'OrderNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 草稿儲存識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'SaveCntIn'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 轉寄次數' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'ForwardCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 是否被閱讀, 1=已被點擊閱讀, 其它=還未被點擊閱讀' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'UnRead'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件案件資訊: 郵件主旨修改人員, NULL=還未被修改過' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailRecord', @level2type=N'COLUMN',@level2name=N'MsgSubjectModifiedBy'
GO
/****** Object:  Table [dbo].[PhoneRecord]    Script Date: 03/23/2014 23:40:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhoneRecord](
	[RID] [nvarchar](20) NOT NULL,
	[PhoneNum] [varchar](32) NOT NULL,
	[QueueName] [varchar](32) NOT NULL,
	[IndexID] [varchar](32) NOT NULL,
 CONSTRAINT [PK_PhoneRecord] PRIMARY KEY CLUSTERED 
(
	[RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'案件編號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PhoneRecord', @level2type=N'COLUMN',@level2name=N'RID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'聯絡電話' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PhoneRecord', @level2type=N'COLUMN',@level2name=N'PhoneNum'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'查詢群組名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PhoneRecord', @level2type=N'COLUMN',@level2name=N'QueueName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'unique ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PhoneRecord', @level2type=N'COLUMN',@level2name=N'IndexID'
GO
/****** Object:  StoredProcedure [dbo].[P_ReplyDispatchRuleProcess]    Script Date: 03/23/2014 23:40:20 ******/
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
/****** Object:  StoredProcedure [dbo].[P_EmailDispatchRuleProcess]    Script Date: 03/23/2014 23:40:20 ******/
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
						 WHEN CharIndex('mailserver99@gmail.com', MsgTo) > 0 THEN 'Enterprise'
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
			IF @AgentID IS NOT NULL AND (@AutoDispatch IS NOT NULL OR @AutoDispatch > 0)
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
	
	-- 2. 掃描目前還未派送的郵件(不是回覆郵件): 一般派送
	DECLARE dataCursor CURSOR FAST_FORWARD FOR
	SELECT	EmailRecord.Id, 
			EmailRecord.RawHeaderId,
			MailGroup = CASE 
						 WHEN CharIndex('mailserver99@gmail.com', MsgTo) > 0 THEN 'Enterprise'
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
				INSERT INTO [dbo].[Records] (RID, CustomerID, TypeID, AgentID, StatusID, Comment, ServiceGroupID, ServiceItemID, IncomingDT, FinishDT)
				SELECT	RID = Convert(nvarchar(16),@RID), 
						CustomerID = NULL,
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
		FETCH NEXT FROM dataCursor INTO @EmailRecordID, @EmailRawHeaderID, @MailGroup;
	END
	
	CLOSE dataCursor;
	DEALLOCATE dataCursor;
		
END
GO
/****** Object:  StoredProcedure [dbo].[P_EmailDispatchForSchedule]    Script Date: 03/23/2014 23:40:20 ******/
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
/****** Object:  Default [DF_Agent_HasVoiceMail]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_HasVoiceMail]  DEFAULT ((0)) FOR [HasVoiceMail]
GO
/****** Object:  Default [DF_Agent_Authority]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_Authority]  DEFAULT ((0)) FOR [Authority]
GO
/****** Object:  Default [DF_Agent_AutoLogin]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_AutoLogin]  DEFAULT ((0)) FOR [AutoLogin]
GO
/****** Object:  Default [DF_Agent_AutoLogout]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_AutoLogout]  DEFAULT ((0)) FOR [AutoLogout]
GO
/****** Object:  Default [DF_Agent_DialLocal]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_DialLocal]  DEFAULT ((0)) FOR [LocalDistance]
GO
/****** Object:  Default [DF_Agent_DialLong]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_DialLong]  DEFAULT ((0)) FOR [LongDistance]
GO
/****** Object:  Default [DF_Agent_DialMobil]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_DialMobil]  DEFAULT ((0)) FOR [Mobil]
GO
/****** Object:  Default [DF_Agent_DialInternational]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_DialInternational]  DEFAULT ((0)) FOR [International]
GO
/****** Object:  Default [DF_Agent_Active]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_Active]  DEFAULT ((1)) FOR [Active]
GO
/****** Object:  Default [DF_Agent_SMSEnable]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_SMSEnable]  DEFAULT ((0)) FOR [SMSEnable]
GO
/****** Object:  Default [DF_Agent_FaxEnable]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_FaxEnable]  DEFAULT ((0)) FOR [FaxEnable]
GO
/****** Object:  Default [DF_Agent_EmailEnable]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_EmailEnable]  DEFAULT ((0)) FOR [EmailEnable]
GO
/****** Object:  Default [DF_Agent_ChatEnable]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_ChatEnable]  DEFAULT ((0)) FOR [ChatEnable]
GO
/****** Object:  Default [DF_Agent_GrpRecBoxEnable]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_GrpRecBoxEnable]  DEFAULT ((0)) FOR [GrpRecBoxEnable]
GO
/****** Object:  Default [DF_Agent_EmailLogon]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_EmailLogon]  DEFAULT ((0)) FOR [EmailIsLogin]
GO
/****** Object:  ForeignKey [FK_EmailDispatchRule_Agent]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[EmailDispatchRule]  WITH CHECK ADD  CONSTRAINT [FK_EmailDispatchRule_Agent] FOREIGN KEY([AgentId1])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[EmailDispatchRule] CHECK CONSTRAINT [FK_EmailDispatchRule_Agent]
GO
/****** Object:  ForeignKey [FK_EmailDispatchRule_Agent1]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[EmailDispatchRule]  WITH CHECK ADD  CONSTRAINT [FK_EmailDispatchRule_Agent1] FOREIGN KEY([AgentId2])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[EmailDispatchRule] CHECK CONSTRAINT [FK_EmailDispatchRule_Agent1]
GO
/****** Object:  ForeignKey [FK_EmailDispatchRule_Agent2]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[EmailDispatchRule]  WITH CHECK ADD  CONSTRAINT [FK_EmailDispatchRule_Agent2] FOREIGN KEY([AgentId3])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[EmailDispatchRule] CHECK CONSTRAINT [FK_EmailDispatchRule_Agent2]
GO
/****** Object:  ForeignKey [FK_EmailDispatchRule_Customers]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[EmailDispatchRule]  WITH CHECK ADD  CONSTRAINT [FK_EmailDispatchRule_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([ID])
GO
ALTER TABLE [dbo].[EmailDispatchRule] CHECK CONSTRAINT [FK_EmailDispatchRule_Customers]
GO
/****** Object:  ForeignKey [FK_EmailRawBody_EmailRawHeader]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[EmailRawBody]  WITH CHECK ADD  CONSTRAINT [FK_EmailRawBody_EmailRawHeader] FOREIGN KEY([RawHeaderId])
REFERENCES [dbo].[EmailRawHeader] ([Id])
GO
ALTER TABLE [dbo].[EmailRawBody] CHECK CONSTRAINT [FK_EmailRawBody_EmailRawHeader]
GO
/****** Object:  ForeignKey [FK_EmailRecord_Agent]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[EmailRecord]  WITH CHECK ADD  CONSTRAINT [FK_EmailRecord_Agent] FOREIGN KEY([InitAgentId])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[EmailRecord] CHECK CONSTRAINT [FK_EmailRecord_Agent]
GO
/****** Object:  ForeignKey [FK_EmailRecord_Agent1]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[EmailRecord]  WITH CHECK ADD  CONSTRAINT [FK_EmailRecord_Agent1] FOREIGN KEY([CurrAgentId])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[EmailRecord] CHECK CONSTRAINT [FK_EmailRecord_Agent1]
GO
/****** Object:  ForeignKey [FK_EmailRecord_EmailRawHeader]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[EmailRecord]  WITH CHECK ADD  CONSTRAINT [FK_EmailRecord_EmailRawHeader] FOREIGN KEY([RawHeaderId])
REFERENCES [dbo].[EmailRawHeader] ([Id])
GO
ALTER TABLE [dbo].[EmailRecord] CHECK CONSTRAINT [FK_EmailRecord_EmailRawHeader]
GO
/****** Object:  ForeignKey [FK_EmailRecord_EmailSavedBody]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[EmailRecord]  WITH CHECK ADD  CONSTRAINT [FK_EmailRecord_EmailSavedBody] FOREIGN KEY([SaveCntIn])
REFERENCES [dbo].[EmailSavedBody] ([Id])
GO
ALTER TABLE [dbo].[EmailRecord] CHECK CONSTRAINT [FK_EmailRecord_EmailSavedBody]
GO
/****** Object:  ForeignKey [FK_EmailRecord_Records]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[EmailRecord]  WITH CHECK ADD  CONSTRAINT [FK_EmailRecord_Records] FOREIGN KEY([RID])
REFERENCES [dbo].[Records] ([RID])
GO
ALTER TABLE [dbo].[EmailRecord] CHECK CONSTRAINT [FK_EmailRecord_Records]
GO
/****** Object:  ForeignKey [FK_PhoneRecord_Records]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[PhoneRecord]  WITH CHECK ADD  CONSTRAINT [FK_PhoneRecord_Records] FOREIGN KEY([RID])
REFERENCES [dbo].[Records] ([RID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PhoneRecord] CHECK CONSTRAINT [FK_PhoneRecord_Records]
GO
/****** Object:  ForeignKey [FK_Records_Customers]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Records]  WITH CHECK ADD  CONSTRAINT [FK_Records_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([ID])
GO
ALTER TABLE [dbo].[Records] CHECK CONSTRAINT [FK_Records_Customers]
GO
/****** Object:  ForeignKey [FK_Records_RecordStatus]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Records]  WITH CHECK ADD  CONSTRAINT [FK_Records_RecordStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[RecordStatus] ([ID])
GO
ALTER TABLE [dbo].[Records] CHECK CONSTRAINT [FK_Records_RecordStatus]
GO
/****** Object:  ForeignKey [FK_Records_RecordType]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Records]  WITH CHECK ADD  CONSTRAINT [FK_Records_RecordType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[RecordType] ([ID])
GO
ALTER TABLE [dbo].[Records] CHECK CONSTRAINT [FK_Records_RecordType]
GO
/****** Object:  ForeignKey [FK_Records_ServiceItem]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Records]  WITH CHECK ADD  CONSTRAINT [FK_Records_ServiceItem] FOREIGN KEY([ServiceGroupID], [ServiceItemID])
REFERENCES [dbo].[ServiceItem] ([GroupID], [ItemID])
GO
ALTER TABLE [dbo].[Records] CHECK CONSTRAINT [FK_Records_ServiceItem]
GO
/****** Object:  ForeignKey [FK_ServiceItem_ServiceGroup]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[ServiceItem]  WITH CHECK ADD  CONSTRAINT [FK_ServiceItem_ServiceGroup] FOREIGN KEY([GroupID])
REFERENCES [dbo].[ServiceGroup] ([GroupID])
GO
ALTER TABLE [dbo].[ServiceItem] CHECK CONSTRAINT [FK_ServiceItem_ServiceGroup]
GO
/****** Object:  ForeignKey [FK_Trades_Customers]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Trades]  WITH CHECK ADD  CONSTRAINT [FK_Trades_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([ID])
GO
ALTER TABLE [dbo].[Trades] CHECK CONSTRAINT [FK_Trades_Customers]
GO
/****** Object:  ForeignKey [FK_Trades_Resellers]    Script Date: 03/23/2014 23:40:21 ******/
ALTER TABLE [dbo].[Trades]  WITH CHECK ADD  CONSTRAINT [FK_Trades_Resellers] FOREIGN KEY([ResellerID])
REFERENCES [dbo].[Resellers] ([ID])
GO
ALTER TABLE [dbo].[Trades] CHECK CONSTRAINT [FK_Trades_Resellers]
GO
