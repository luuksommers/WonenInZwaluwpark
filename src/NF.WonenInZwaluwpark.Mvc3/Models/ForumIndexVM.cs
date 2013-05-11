using System;
using System.Linq;

namespace NF.WonenInZwaluwpark.Mvc3.Models
{
    public class ForumIndexVM
    {
        public DateTime? LastVisit { get; private set; }
        public ForumCategory[] Categories { get; private set; }
        public ForumSubCategory[] SubCategories { get; private set; }
        public UserProfile[] UserProfiles { get; private set; }

        public ForumIndexVM(DataModelDataContext dc, DateTime? lastVisit)
        {
            LastVisit = lastVisit;
            Categories = dc.ForumCategories.ToArray();
            SubCategories = dc.ForumSubCategories.ToArray();
            UserProfiles = dc.UserProfiles.ToArray();
        }
    }
}