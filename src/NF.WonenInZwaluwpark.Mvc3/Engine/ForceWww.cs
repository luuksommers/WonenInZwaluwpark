
namespace NF.WonenInZwaluwpark.Mvc3.Engine
{
    using System.Text.RegularExpressions;
    using System.Web;

    public class ForceWww : System.Web.UI.Page
    {
        public static void ForceWWW()
        {
            if (!HttpContext.Current.Request.IsLocal && !GetServerDomain().StartsWith("www."))
            {
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", "http://www." + GetServerDomain() + HttpContext.Current.Request.RawUrl);
            }
        }

        public static string GetServerDomain()
        {
            string myURL = HttpContext.Current.Request.Url.ToString();
            Regex re = new Regex("^(?:(?:https?\\:)?(?:\\/\\/)?)?([^\\/]+)");
            Match m = re.Match(myURL);
            return m.Groups[1].Value;
        }
    }
}