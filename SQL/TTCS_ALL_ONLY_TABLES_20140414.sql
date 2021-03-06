USE [ksky]
GO
/****** Object:  Table [dbo].[EmailRawBody]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[EmailFilterRule]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[EmailRawHeader]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[ServiceItem]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[ServiceGroup]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[RecordType]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[RecordStatus]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[EmailSentBox]    Script Date: 04/14/2014 03:03:27 ******/
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件副本收件者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentBox', @level2type=N'COLUMN',@level2name=N'Cc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件密件副本收件者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentBox', @level2type=N'COLUMN',@level2name=N'Bcc'
GO
/****** Object:  Table [dbo].[EmailSentAttachment]    Script Date: 04/14/2014 03:03:27 ******/
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'寄件備份附檔識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentAttachment', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'寄件備份附檔資訊: 郵件案件識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentAttachment', @level2type=N'COLUMN',@level2name=N'EmailRecordId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'寄件備份附檔資訊: 檔案名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentAttachment', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密件備份檔案資訊: 檔案MIME碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentAttachment', @level2type=N'COLUMN',@level2name=N'CntMedia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'寄件備份附檔資訊: 檔案內容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSentAttachment', @level2type=N'COLUMN',@level2name=N'CntBody'
GO
/****** Object:  Table [dbo].[EmailScheduleSetting]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[EmailSavedBody]    Script Date: 04/14/2014 03:03:27 ******/
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'郵件草稿資訊:副本收件者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedBody', @level2type=N'COLUMN',@level2name=N'Cc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'草稿郵件資訊: 密件副本收件者' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedBody', @level2type=N'COLUMN',@level2name=N'Bcc'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'草稿郵件資訊:是否夾帶簽名檔, 0=夾帶簽名檔, 1=不夾帶簽名檔' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedBody', @level2type=N'COLUMN',@level2name=N'MailSignature'
GO
/****** Object:  Table [dbo].[EmailSavedAttachment]    Script Date: 04/14/2014 03:03:27 ******/
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'草稿附檔識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedAttachment', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'草稿郵件附檔資訊: 郵件案件識別碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedAttachment', @level2type=N'COLUMN',@level2name=N'EmailRecordId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'草稿附檔資訊: 檔案名稱' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedAttachment', @level2type=N'COLUMN',@level2name=N'FileName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'草稿郵件附檔資訊: 檔案MIME碼' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedAttachment', @level2type=N'COLUMN',@level2name=N'CntMedia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'草稿郵件附檔資訊: 檔案內容' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'EmailSavedAttachment', @level2type=N'COLUMN',@level2name=N'CntBody'
GO
/****** Object:  Table [dbo].[EmailReplyCan]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[EmailRejectReason]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[MailMember]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[MailGroup]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[RecordCloseApproach]    Script Date: 04/14/2014 03:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordCloseApproach](
	[ApproachID] [int] IDENTITY(1,1) NOT NULL,
	[ApproachName] [nvarchar](50) NULL,
 CONSTRAINT [PK_RecordCloseApproach] PRIMARY KEY CLUSTERED 
(
	[ApproachID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailContacts]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[EmailAgentWeight]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[Customers]    Script Date: 04/14/2014 03:03:27 ******/
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
	[GroupID] [varchar](32) NULL,
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否為企業客戶' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Customers', @level2type=N'COLUMN',@level2name=N'GroupID'
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
/****** Object:  Table [dbo].[Agent]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[EmailDispatchRule]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[Records]    Script Date: 04/14/2014 03:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Records](
	[RID] [nvarchar](20) NOT NULL,
	[CustomerID] [uniqueidentifier] NULL,
	[TypeID] [int] NULL,
	[AgentID] [varchar](8) NULL,
	[StatusID] [int] NULL,
	[Comment] [nvarchar](max) NULL,
	[ServiceGroupID] [int] NULL,
	[ServiceItemID] [int] NULL,
	[CloseApproachID] [int] NULL,
	[IncomingDT] [datetime] NULL,
	[FinishDT] [datetime] NULL,
 CONSTRAINT [PK_Records] PRIMARY KEY CLUSTERED 
(
	[RID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
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
/****** Object:  Table [dbo].[EmailRecord]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  Table [dbo].[RecordServiceMap]    Script Date: 04/14/2014 03:03:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecordServiceMap](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RID] [nvarchar](20) NOT NULL,
	[GroupID] [int] NOT NULL,
	[ItemID] [int] NOT NULL,
 CONSTRAINT [PK_CustomerServiceMap] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhoneRecord]    Script Date: 04/14/2014 03:03:27 ******/
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
/****** Object:  ForeignKey [FK_EmailDispatchRule_Agent]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[EmailDispatchRule]  WITH CHECK ADD  CONSTRAINT [FK_EmailDispatchRule_Agent] FOREIGN KEY([AgentId1])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[EmailDispatchRule] CHECK CONSTRAINT [FK_EmailDispatchRule_Agent]
GO
/****** Object:  ForeignKey [FK_EmailDispatchRule_Agent1]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[EmailDispatchRule]  WITH CHECK ADD  CONSTRAINT [FK_EmailDispatchRule_Agent1] FOREIGN KEY([AgentId2])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[EmailDispatchRule] CHECK CONSTRAINT [FK_EmailDispatchRule_Agent1]
GO
/****** Object:  ForeignKey [FK_EmailDispatchRule_Agent2]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[EmailDispatchRule]  WITH CHECK ADD  CONSTRAINT [FK_EmailDispatchRule_Agent2] FOREIGN KEY([AgentId3])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[EmailDispatchRule] CHECK CONSTRAINT [FK_EmailDispatchRule_Agent2]
GO
/****** Object:  ForeignKey [FK_EmailDispatchRule_Customers]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[EmailDispatchRule]  WITH CHECK ADD  CONSTRAINT [FK_EmailDispatchRule_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([ID])
GO
ALTER TABLE [dbo].[EmailDispatchRule] CHECK CONSTRAINT [FK_EmailDispatchRule_Customers]
GO
/****** Object:  ForeignKey [FK_EmailRawBody_EmailRawHeader]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[EmailRawBody]  WITH CHECK ADD  CONSTRAINT [FK_EmailRawBody_EmailRawHeader] FOREIGN KEY([RawHeaderId])
REFERENCES [dbo].[EmailRawHeader] ([Id])
GO
ALTER TABLE [dbo].[EmailRawBody] CHECK CONSTRAINT [FK_EmailRawBody_EmailRawHeader]
GO
/****** Object:  ForeignKey [FK_EmailRecord_Agent]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[EmailRecord]  WITH CHECK ADD  CONSTRAINT [FK_EmailRecord_Agent] FOREIGN KEY([InitAgentId])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[EmailRecord] CHECK CONSTRAINT [FK_EmailRecord_Agent]
GO
/****** Object:  ForeignKey [FK_EmailRecord_Agent1]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[EmailRecord]  WITH CHECK ADD  CONSTRAINT [FK_EmailRecord_Agent1] FOREIGN KEY([CurrAgentId])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[EmailRecord] CHECK CONSTRAINT [FK_EmailRecord_Agent1]
GO
/****** Object:  ForeignKey [FK_EmailRecord_EmailRawHeader]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[EmailRecord]  WITH CHECK ADD  CONSTRAINT [FK_EmailRecord_EmailRawHeader] FOREIGN KEY([RawHeaderId])
REFERENCES [dbo].[EmailRawHeader] ([Id])
GO
ALTER TABLE [dbo].[EmailRecord] CHECK CONSTRAINT [FK_EmailRecord_EmailRawHeader]
GO
/****** Object:  ForeignKey [FK_EmailRecord_EmailSavedBody]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[EmailRecord]  WITH CHECK ADD  CONSTRAINT [FK_EmailRecord_EmailSavedBody] FOREIGN KEY([SaveCntIn])
REFERENCES [dbo].[EmailSavedBody] ([Id])
GO
ALTER TABLE [dbo].[EmailRecord] CHECK CONSTRAINT [FK_EmailRecord_EmailSavedBody]
GO
/****** Object:  ForeignKey [FK_EmailRecord_Records]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[EmailRecord]  WITH CHECK ADD  CONSTRAINT [FK_EmailRecord_Records] FOREIGN KEY([RID])
REFERENCES [dbo].[Records] ([RID])
GO
ALTER TABLE [dbo].[EmailRecord] CHECK CONSTRAINT [FK_EmailRecord_Records]
GO
/****** Object:  ForeignKey [FK_PhoneRecord_Records]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[PhoneRecord]  WITH CHECK ADD  CONSTRAINT [FK_PhoneRecord_Records] FOREIGN KEY([RID])
REFERENCES [dbo].[Records] ([RID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PhoneRecord] CHECK CONSTRAINT [FK_PhoneRecord_Records]
GO
/****** Object:  ForeignKey [FK_Records_Customers]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[Records]  WITH CHECK ADD  CONSTRAINT [FK_Records_Customers] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customers] ([ID])
GO
ALTER TABLE [dbo].[Records] CHECK CONSTRAINT [FK_Records_Customers]
GO
/****** Object:  ForeignKey [FK_Records_RecordCloseApproach]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[Records]  WITH CHECK ADD  CONSTRAINT [FK_Records_RecordCloseApproach] FOREIGN KEY([CloseApproachID])
REFERENCES [dbo].[RecordCloseApproach] ([ApproachID])
GO
ALTER TABLE [dbo].[Records] CHECK CONSTRAINT [FK_Records_RecordCloseApproach]
GO
/****** Object:  ForeignKey [FK_Records_Records]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[Records]  WITH CHECK ADD  CONSTRAINT [FK_Records_Records] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[Records] CHECK CONSTRAINT [FK_Records_Records]
GO
/****** Object:  ForeignKey [FK_Records_RecordStatus]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[Records]  WITH CHECK ADD  CONSTRAINT [FK_Records_RecordStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[RecordStatus] ([ID])
GO
ALTER TABLE [dbo].[Records] CHECK CONSTRAINT [FK_Records_RecordStatus]
GO
/****** Object:  ForeignKey [FK_Records_RecordType]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[Records]  WITH CHECK ADD  CONSTRAINT [FK_Records_RecordType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[RecordType] ([ID])
GO
ALTER TABLE [dbo].[Records] CHECK CONSTRAINT [FK_Records_RecordType]
GO
/****** Object:  ForeignKey [FK_Records_ServiceItem]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[Records]  WITH CHECK ADD  CONSTRAINT [FK_Records_ServiceItem] FOREIGN KEY([ServiceGroupID], [ServiceItemID])
REFERENCES [dbo].[ServiceItem] ([GroupID], [ItemID])
GO
ALTER TABLE [dbo].[Records] CHECK CONSTRAINT [FK_Records_ServiceItem]
GO
/****** Object:  ForeignKey [FK_RecordServiceMap_Records]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[RecordServiceMap]  WITH CHECK ADD  CONSTRAINT [FK_RecordServiceMap_Records] FOREIGN KEY([RID])
REFERENCES [dbo].[Records] ([RID])
GO
ALTER TABLE [dbo].[RecordServiceMap] CHECK CONSTRAINT [FK_RecordServiceMap_Records]
GO
/****** Object:  ForeignKey [FK_RecordServiceMap_ServiceItem]    Script Date: 04/14/2014 03:03:27 ******/
ALTER TABLE [dbo].[RecordServiceMap]  WITH CHECK ADD  CONSTRAINT [FK_RecordServiceMap_ServiceItem] FOREIGN KEY([GroupID], [ItemID])
REFERENCES [dbo].[ServiceItem] ([GroupID], [ItemID])
GO
ALTER TABLE [dbo].[RecordServiceMap] CHECK CONSTRAINT [FK_RecordServiceMap_ServiceItem]
GO
