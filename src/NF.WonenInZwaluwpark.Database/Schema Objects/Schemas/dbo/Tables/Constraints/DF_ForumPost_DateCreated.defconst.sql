ALTER TABLE [dbo].[ForumPost]
   ADD CONSTRAINT [DF_ForumPost_DateCreated] 
   DEFAULT CURRENT_TIMESTAMP
   FOR DateCreated


