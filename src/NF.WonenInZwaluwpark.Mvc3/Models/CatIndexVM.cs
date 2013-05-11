using System;
using System.Collections.Generic;
using System.Linq;

namespace NF.WonenInZwaluwpark.Mvc3.Models
{
    public class CatIndexVM
    {
        public DateTime? LastVisit { get; private set; }
        public ForumSubCategory SubCatagory { get; set; }
        public ForumPost[] ForumPosts { get; set; }
        public ForumPost[] LastPosts { get; set; }
        public UserProfile[] UserProfiles { get; set; }
        public Dictionary<int, DateTime> ForumPostOrder { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPrevPage { get; set; }
        public int CurrentPage { get; set; }

        public CatIndexVM(DataModelDataContext dc, int subCatogoryId, DateTime? lastVisit, int? pageId, int pageSize)
        {
            ForumPostOrder = new Dictionary<int, DateTime>();
            LastVisit = lastVisit;

            SubCatagory = dc.ForumSubCategories
                .Where(a => a.Id == subCatogoryId)
                .First();

            var postCount = dc.ForumPosts
                .Count(a => a.ForumSubCategoryId == subCatogoryId && a.ParentForumPostId == null);

            if (postCount > ((pageId ?? 0) + 1) * pageSize)
                HasNextPage = true;
            if ((pageId ?? 0) > 0)
                HasPrevPage = true;
            CurrentPage = pageId ?? 0;

            ForumPosts = dc.ForumPosts
                .Where(a => a.ForumSubCategoryId == subCatogoryId && a.ParentForumPostId == null)
                .OrderByDescending(a => a.Id)
                .ToArray();

            LastPosts = ForumPosts
                .Select(post => dc.ForumPosts.Where(a => a.ParentForumPostId == post.Id)
                                             .OrderByDescending(a => a.Id)
                                             .Take(1).FirstOrDefault())
                .Where(lastPost => lastPost != null)
                .ToArray();

            foreach (var forumPost in ForumPosts)
            {
                var lastPost = LastPosts.FirstOrDefault(a => a.ParentForumPostId == forumPost.Id);
                if( lastPost != null && lastPost.DateCreated > forumPost.DateCreated )
                {
                    ForumPostOrder.Add(forumPost.Id, lastPost.DateCreated);
                }
                else
                {
                    ForumPostOrder.Add(forumPost.Id, forumPost.DateCreated);
                }
            }

            ForumPostOrder = ForumPostOrder.OrderByDescending(a => a.Value)
                .Skip((pageId ?? 0) * pageSize)
                .Take(pageSize)
                .ToDictionary(a=>a.Key, a=>a.Value);

            var forumPostIds = ForumPostOrder.Select(a => a.Key);

            ForumPosts = ForumPosts.Where(a => forumPostIds.Contains(a.Id)).ToArray();

            UserProfiles = dc.UserProfiles
                .ToArray();
        }
    }
}