ALTER TABLE [dbo].[ForumPollOptionVote]
	ADD CONSTRAINT [FK_ForumPollOptionVote_UserProfile] 
	FOREIGN KEY (UserProfileId)
	REFERENCES UserProfile (Id)	

