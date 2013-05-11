ALTER TABLE [dbo].[ForumCategory]
   ADD CONSTRAINT [DF_ForumCategory_DateCreated] 
   DEFAULT CURRENT_TIMESTAMP
   FOR DateCreated


