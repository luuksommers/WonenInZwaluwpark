CREATE TABLE [dbo].[ForumSubCategory]
(
  Id INT NOT NULL IDENTITY(1,1),
  ForumCategoryId INT NOT NULL,
  Title VARCHAR(64) NOT NULL,
  [Description] TEXT NULL,
  DateCreated DATETIME NOT NULL,
  Ip VARCHAR(20) NULL,
  LastPostDate DATETIME NULL,
  PostCount INT NOT NULL DEFAULT 0,
  LastPostByUserProfileId INT NULL
)
