ALTER TABLE [dbo].[ForumPost]
	ADD CONSTRAINT [FK_ForumPost_ForumSubCategoryId] 
	FOREIGN KEY (ForumSubCategoryId)
	REFERENCES ForumSubCategory (Id)	

