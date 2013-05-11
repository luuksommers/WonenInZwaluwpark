CREATE TRIGGER [TR_ForumPost_Insert]
    ON [dbo].[ForumPost]
    FOR INSERT 
    AS 
    BEGIN
    	SET NOCOUNT ON
		UPDATE FSC
			SET FSC.LastPostDate = I.DateCreated,
			    FSC.PostCount = FSC.PostCount + 1,
				FSC.LastPostByUserProfileId = I.UserProfileId
		FROM ForumSubCategory FSC 
			INNER JOIN inserted I ON FSC.Id = I.ForumSubCategoryId AND I.ForumSubCategoryId IS NOT NULL
    END
