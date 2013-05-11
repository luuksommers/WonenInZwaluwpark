ALTER TABLE [dbo].[ForumPollOption]
   ADD CONSTRAINT [DF_ForumPollOption_DateCreated] 
   DEFAULT CURRENT_TIMESTAMP
   FOR DateCreated


