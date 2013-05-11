CREATE TABLE [dbo].[ForumPollOption]
(
  Id INT NOT NULL IDENTITY(1,1),
  ForumPostId INT NOT NULL ,
  Title VARCHAR(64) NOT NULL,
  DateCreated DATETIME NOT NULL,
)
