﻿CREATE TABLE [dbo].[LoginLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserProfileId] [varchar](200) NOT NULL,
	[LoginDate] [datetime] NOT NULL,
	[UserIp] VARCHAR(40) NULL,
 CONSTRAINT [PK_LoginLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


