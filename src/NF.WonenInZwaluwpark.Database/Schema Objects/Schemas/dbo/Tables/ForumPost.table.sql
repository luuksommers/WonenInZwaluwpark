CREATE TABLE [dbo].[ForumPost]
(
  Id INT NOT NULL IDENTITY(1,1),
  ForumSubCategoryId INT NOT NULL ,
  UserProfileId INT NOT NULL,
  ParentForumPostId INT NULL,
  Title VARCHAR(90) NOT NULL,
  Content TEXT NOT NULL,
  IsPoll BIT NOT NULL,
  DateCreated DATETIME NOT NULL,
  Ip VARCHAR(20) NULL,
)
