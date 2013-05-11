ALTER TABLE [dbo].[ForumPollOption]
	ADD CONSTRAINT [FK_ForumPollOption_ForumPost] 
	FOREIGN KEY (ForumPostId)
	REFERENCES ForumPost (Id)	

