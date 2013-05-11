using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using LinqToTwitter;

namespace NF.WonenInZwaluwpark.Mvc3.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [OutputCache(Duration = 3600, VaryByParam = "None", VaryByCustom = "authentication", CacheProfile = "CacheHour", Order = 2)]
        public ActionResult Index()
        {
            Response.AppendHeader(
                "X-XRDS-Location",
                new Uri(Request.Url, Response.ApplyAppPathModifier("~/Home/xrds")).AbsoluteUri);

            var publicTweets = HttpContext.Cache["tweets"] as List<Status>;
            if (publicTweets == null)
            {
                var twitterCtx = new TwitterContext();
                publicTweets =
                    (from tweet in twitterCtx.Status
                     where tweet.Type == StatusType.User && tweet.ID == "zwaluwpark"
                     select tweet).ToList();
                HttpContext.Cache.Add("tweets", publicTweets, null, DateTime.Now.AddHours(1), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }

            ViewData["tweets"] = publicTweets;

            return View();
        }

        [OutputCache(Duration = 3600, VaryByParam = "None", CacheProfile = "CacheHour")]
        public ActionResult Xrds()
        {
            Response.ContentType = "application/xrds+xml";
            return View("Xrds");
        }
    }
}
