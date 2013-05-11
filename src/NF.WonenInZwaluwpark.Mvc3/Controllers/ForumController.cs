using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NF.WonenInZwaluwpark.Mvc3.Models;

namespace NF.WonenInZwaluwpark.Mvc3.Controllers
{
    public class ForumController : Controller
    {
        //[OutputCache(Duration = 3600, VaryByParam = "None", VaryByCustom = "authentication", CacheProfile = "CacheHour")]
        public ActionResult Index()
        {
            if (Request.Cookies != null && (Request.Cookies["currentSession"] == null || string.IsNullOrEmpty(Request.Cookies["currentSession"].Value)))
            {
                var pCookie = new HttpCookie("prevVisit")
                {
                    Value = Request.Cookies["lastVisit"] != null ? Request.Cookies["lastVisit"].Value : "",
                    Expires = DateTime.Now.AddMonths(6)
                };
                Response.Cookies.Add(pCookie);

                var aCookie = new HttpCookie("lastVisit")
                                    {
                                        Value = DateTime.Now.ToString(),
                                        Expires = DateTime.Now.AddMonths(6)
                                    };
                Response.Cookies.Add(aCookie);

                var cCookie = new HttpCookie("currentSession")
                {
                    Value = GetMd5Hash(DateTime.Now.Ticks.ToString())
                };
                Response.Cookies.Add(cCookie);
            }

            DateTime? lastVisit = GetLastVisitDate();
            using (var dc = new DataModelDataContext())
            {
                return View(new ForumIndexVM(dc, lastVisit));
            }
        }

        private DateTime? GetLastVisitDate()
        {
            DateTime? lastVisit = null;
            DateTime lastVisitValue;
            if (Request.Cookies != null && Request.Cookies["prevVisit"] != null && DateTime.TryParse(Request.Cookies["prevVisit"].Value, out lastVisitValue))
            {
                lastVisit = lastVisitValue;
            }
            return lastVisit;
        }

        static string GetMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        //[OutputCache(Duration = 3600, VaryByParam = "id", CacheProfile = "CacheHour")]
        public ActionResult Cat(int id, int? page)
        {
            DateTime? lastVisit = GetLastVisitDate();
            using (var dc = new DataModelDataContext())
            {
                return View(new CatIndexVM(dc, id, lastVisit,page, 10));
            }
        }

        [Authorize]
        [OutputCache(Duration = 3600, VaryByParam = "id;replyTo", CacheProfile = "CacheHour")]
        public ActionResult CreatePost(int id, int? replyTo)
        {
            using (var dc = new DataModelDataContext())
            {
                var post = new ForumPost
                {
                    ForumSubCategoryId = id,
                    ParentForumPostId = replyTo,
                };

                if (replyTo.HasValue)
                {
                    var orgPost = dc.ForumPosts.Where(a => a.Id == replyTo).First();
                    post.Title = "Reply: " + orgPost.Title;
                }
                return View(post);
            }
        }

        [HttpPost]
        [Authorize]
        [OutputCache(Duration = 3600, VaryByParam = "id;replyTo", CacheProfile = "CacheHour")]
        public ActionResult CreatePost(int id, int? replyTo, FormCollection collection)
        {
            using (var dc = new DataModelDataContext())
            {
                var post = new ForumPost
                {
                    ForumSubCategoryId = id,
                    ParentForumPostId = replyTo,
                };

                if (replyTo.HasValue)
                {
                    var orgPost = dc.ForumPosts.Where(a => a.Id == replyTo).First();
                    post.Title = "Reply: " + orgPost.Title;
                }

                if (ModelState.IsValid && TryUpdateModel(post))
                {
                    post.IsPoll = false;
                    post.UserProfileId = dc.UserProfiles.Where(a => a.UniqueName == User.Identity.Name).First().Id;
                    post.Ip = Request.UserHostAddress;
                    post.DateCreated = DateTime.Now;

                    dc.ForumPosts.InsertOnSubmit(post);
                    dc.SubmitChanges();
                    if (post.ParentForumPostId == null)
                    {
                        return RedirectToAction("Post", new { id = post.Id });
                    }
                    return RedirectToAction("Post", new { id = post.ParentForumPostId });
                }

                return View(post);
            }
        }

        //[OutputCache(Duration = 3600, VaryByParam = "id", CacheProfile = "CacheHour")]
        public ActionResult Post(int id)
        {
            using (var dc = new DataModelDataContext())
            {
                return View(new PostIndexVM(dc, id));
            }
        }
    }
}
