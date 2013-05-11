using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace NF.WonenInZwaluwpark.Mvc3.Helpers
{
    public static class LinkHelpers
    {
        public static MvcHtmlString ActiveActionLinkHelper<T>(this HtmlHelper<T> html, string linkText, string actionName, string controlName)
        {
            return html.ActiveActionLinkHelper(linkText, actionName, controlName, false);
        }

        public static MvcHtmlString ActiveActionLinkHelper<T>(this HtmlHelper<T> html, string linkText, string actionName, string controlName, bool validateControllerOnly)
        {
            if ((validateControllerOnly || html.ViewContext.RouteData.Values["action"].ToString() == actionName) &&
                html.ViewContext.RouteData.Values["controller"].ToString() == controlName)
            {
                return html.ActionLink(linkText, actionName, controlName, null, new { @class = "active" });
            }

            return html.ActionLink(linkText, actionName, controlName, null, new { @class = "inactive" });
        }

        public static MvcHtmlString ImpressionImage<T>(this HtmlHelper<T> html, string group, string image)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);
            return MvcHtmlString.Create(string.Format("<a rel=\"{0}\" href=\"{1}\" title=\"\"><img alt=\"\" src=\"{2}\" /></a>",
                group, url.Content("~/Images/Impression/" + image), url.Content("~/Images/Impression/Thumbnails/" + image)));
        }

        public static MvcHtmlString GenderToImage<T>(this HtmlHelper<T> html, int? gender)
        {
            var url = new UrlHelper(html.ViewContext.RequestContext);
            string image = "user-silhouette-question.png";
            string alt = "Onbekend";
            switch (gender)
            {
                case 1:
                    image = "user.png";
                    alt = "man";
                    break;
                case 2:
                    image = "user-green-female.png";
                    alt = "vrouw";
                    break;
                case 3:
                    image = "users.png";
                    alt = "samen";
                    break;
            }
            return MvcHtmlString.Create("<img src=\"" + url.Content("~/Images/" + image) + "\" alt=\"" + alt + "\" />");
        }
    }
}