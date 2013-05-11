CREATE TABLE [dbo].[UserProfile](
	Id INT NOT NULL IDENTITY(1,1),
	UniqueName [varchar](200) NOT NULL,
	[FriendlyName] [varchar](100) NOT NULL,
	[HouseNbr] [int] NOT NULL,
	[EmailAddress] [varchar](200) NULL,
	[HyvesUrl] [varchar](200) NULL,
	[FacebookUrl] [varchar](200) NULL,
	[LinkedInUrl] [varchar](200) NULL,
	[Share] BIT NOT NULL CONSTRAINT DF_UserProfile_Share DEFAULT(0),
	[Gender] INT NULL,
	[ProfileVersion] [int] NOT NULL CONSTRAINT DF_UserProfile_ProfileVersion DEFAULT(0),
 CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED 
(
	Id ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


