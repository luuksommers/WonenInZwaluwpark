/****** Object:  Default [DF_ELMAH_Error_ErrorId]    Script Date: 09/26/2010 11:17:02 ******/
ALTER TABLE [dbo].[ELMAH_Error] ADD  CONSTRAINT [DF_ELMAH_Error_ErrorId]  DEFAULT (newid()) FOR [ErrorId]


