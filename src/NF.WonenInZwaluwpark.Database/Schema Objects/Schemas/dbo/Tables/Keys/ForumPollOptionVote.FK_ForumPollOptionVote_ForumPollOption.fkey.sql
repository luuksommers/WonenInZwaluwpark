ALTER TABLE [dbo].[ForumPollOptionVote]
	ADD CONSTRAINT [FK_ForumPollOptionVote_ForumPollOption] 
	FOREIGN KEY (ForumPollOptionId)
	REFERENCES ForumPollOption (Id)	

