ALTER TABLE [dbo].[ForumPost]
	ADD CONSTRAINT [FK_ForumPost_UserProfileId] 
	FOREIGN KEY (UserProfileId)
	REFERENCES UserProfile (Id)	

