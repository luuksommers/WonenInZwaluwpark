CREATE TABLE [dbo].[ForumCategory]
(
  Id INT NOT NULL IDENTITY(1,1),
  Title VARCHAR(64) NOT NULL,
  [Description] TEXT NULL,
  DateCreated DATETIME NOT NULL,
  Ip VARCHAR(20) NULL
)
