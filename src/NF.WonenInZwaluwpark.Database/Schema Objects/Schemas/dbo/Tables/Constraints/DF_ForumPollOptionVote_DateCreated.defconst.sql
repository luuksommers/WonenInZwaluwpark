ALTER TABLE [dbo].[ForumPollOptionVote]
   ADD CONSTRAINT [DF_ForumPollOptionVote_DateCreated] 
   DEFAULT CURRENT_TIMESTAMP
   FOR DateCreated