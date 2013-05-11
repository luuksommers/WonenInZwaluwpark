CREATE TABLE [dbo].[ForumPollOptionVote]
(
  Id INT NOT NULL IDENTITY(1,1),
  ForumPollOptionId INT NOT NULL,
  UserProfileId INT NOT NULL,
  DateCreated DATETIME NOT NULL,
  Ip VARCHAR(20) NULL,
)
