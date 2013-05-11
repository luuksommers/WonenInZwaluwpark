using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NF.WonenInZwaluwpark.Mvc3.Models
{
    public class PostIndexVM
    {
        public ForumSubCategory SubCatagory { get; set; }
        public ForumPost Post { get; set; }
        public ForumPost[] Replies { get; set; }
        public UserProfile[] UserProfiles { get; set; }

        public PostIndexVM(DataModelDataContext dc, int postId)
        {
            Post = dc.ForumPosts
                .Where(a => a.Id == postId)
                .First();

            Replies = dc.ForumPosts
                .Where(a => a.ParentForumPostId == postId)
                .OrderBy(a => a.Id)
                .ToArray();

            SubCatagory = dc.ForumSubCategories
                .Where(a => a.Id == Post.ForumSubCategoryId)
                .First();

            UserProfiles = dc.UserProfiles
                .ToArray();
        }
    }
}