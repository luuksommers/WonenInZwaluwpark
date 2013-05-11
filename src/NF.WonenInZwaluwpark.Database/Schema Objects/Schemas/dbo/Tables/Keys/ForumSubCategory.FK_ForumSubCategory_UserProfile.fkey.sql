ALTER TABLE [dbo].[ForumSubCategory]
	ADD CONSTRAINT [FK_ForumSubCategory_UserProfile] 
	FOREIGN KEY (LastPostByUserProfileId)
	REFERENCES UserProfile (Id)	

