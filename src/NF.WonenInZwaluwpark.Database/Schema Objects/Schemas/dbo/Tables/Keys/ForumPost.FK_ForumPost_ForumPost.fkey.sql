ALTER TABLE [dbo].[ForumPost]
	ADD CONSTRAINT [FK_ForumPost_ForumPost] 
	FOREIGN KEY (ParentForumPostId)
	REFERENCES ForumPost (Id)	

