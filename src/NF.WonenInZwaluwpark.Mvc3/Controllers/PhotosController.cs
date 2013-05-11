using System.Web.Mvc;

namespace NF.WonenInZwaluwpark.Mvc3.Controllers
{
    public class PhotosController : Controller
    {
        [OutputCache(Duration = 3600, VaryByParam = "None", CacheProfile = "CacheHour")]
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 3600, VaryByParam = "id", CacheProfile = "CacheHour")]
        public ActionResult Page(int? id)
        {
            ViewBag.Page = id;
            return View();
        }
    }
}
