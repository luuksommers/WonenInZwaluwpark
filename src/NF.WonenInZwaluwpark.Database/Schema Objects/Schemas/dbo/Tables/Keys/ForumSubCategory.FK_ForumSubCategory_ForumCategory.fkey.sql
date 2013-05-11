ALTER TABLE [dbo].[ForumSubCategory]
	ADD CONSTRAINT [FK_ForumSubCategory_ForumCategory] 
	FOREIGN KEY (ForumCategoryId)
	REFERENCES ForumCategory (Id)	

