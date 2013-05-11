using System.Web.Mvc;

namespace NF.WonenInZwaluwpark.Mvc3.Controllers
{
    public class ContactController : Controller
    {
        [OutputCache(Duration = 3600, VaryByParam = "None", CacheProfile = "CacheHour")]
        public ActionResult Index()
        {
            return View();
        }
    }
}
