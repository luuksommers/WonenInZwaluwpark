using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using NF.WonenInZwaluwpark.Mvc3.Models;

namespace NF.WonenInZwaluwpark.Mvc3.Controllers
{
    public class UserController : Controller
    {
        public const int CurrentProfileVersion = 3;
        private string FacebookAppId
        {
            get
            {
                if (Request.IsLocal)
                {
                    return ConfigurationManager.AppSettings["facebookAppIdLocalHost"];
                }

                return ConfigurationManager.AppSettings["facebookAppId"];
            }
        }

        private string FacebookAppSecret
        {
            get
            {
                if (Request.IsLocal)
                {
                    return ConfigurationManager.AppSettings["facebookAppSecretLocalHost"];
                }

                return ConfigurationManager.AppSettings["facebookAppSecret"];
            }
        }

        private string LiveIdAppId
        {
            get
            {
                return ConfigurationManager.AppSettings["liveIdAppId"];
            }
        }

        private string LiveIdAppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings["liveIdAppSecret"];
            }
        }
        private static OpenIdRelyingParty openid = new OpenIdRelyingParty();

        [Authorize]
        public ActionResult Edit()
        {
            using (var dc = new DataModelDataContext())
            {
                var profile = dc.UserProfiles
                    .Where(a => a.UniqueName == User.Identity.Name)
                    .FirstOrDefault() ?? new UserProfile();
                ViewData.Model = profile;
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(FormCollection collection)
        {
            using (var dc = new DataModelDataContext())
            {
                var profile = dc.UserProfiles
                    .Where(a => a.UniqueName == User.Identity.Name)
                    .FirstOrDefault() ?? new UserProfile();

                if (TryUpdateModel(profile))
                {
                    profile.ProfileVersion = CurrentProfileVersion;
                    if (string.IsNullOrEmpty(profile.FriendlyName))
                    {
                        ModelState.AddModelError("FriendlyName", "Uw naam mag niet leeg zijn");
                    }
                    if (string.IsNullOrEmpty(profile.EmailAddress))
                    {
                        ModelState.AddModelError("EmailAddress", "Uw e-mailadres mag niet leeg zijn");
                    }
                    if (profile.HouseNbr < 1 || profile.HouseNbr > 104)
                    {
                        ModelState.AddModelError("HouseNbr", "Uw huisnummer moet tussen 1 en 104 liggen");
                    }

                    if (ModelState.IsValid)
                    {
                        Session["FriendlyName"] = profile.FriendlyName;
                        if (profile.Id == 0)
                        {
                            profile.UniqueName = User.Identity.Name;
                            dc.UserProfiles.InsertOnSubmit(profile);
                        }
                        dc.SubmitChanges();
                        return RedirectToAction("Index", "Home");
                    }
                }

                return View(profile);
            }
        }

        [OutputCache(Duration = 3600, VaryByParam = "None", CacheProfile = "CacheHour")]
        public ActionResult Login()
        {
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return Redirect("/");
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            using (var dc = new DataModelDataContext())
            {
                return View(dc.UserProfiles
                    .Where(a => a.Id == id)
                    .FirstOrDefault());
            }
        }

        [Authorize]
        [OutputCache(Duration = 3600, VaryByParam = "None", CacheProfile = "CacheHour")]
        public ActionResult List()
        {
            using (var dc = new DataModelDataContext())
            {
                return View(dc.UserProfiles.ToArray());
            }
        }

        private string GetLiveIdCallbackUrl()
        {
            return "http://www.woneninzwaluwpark.nl/User/LiveIdCallBack";// +"?returnUrl=" + returnUrl;
        }

        public ActionResult LiveId(string returnUrl, string wrap_verification_code)
        {
            string redirectTo = string.Format(
                "https://consent.live.com/Connect.aspx?wrap_client_id={0}&wrap_callback={1}&wrap_scope=WL_Profiles.View",
                Uri.EscapeDataString(LiveIdAppId),
                Uri.EscapeDataString(GetLiveIdCallbackUrl()));

            return Redirect(redirectTo);
        }

        public ActionResult LiveIdCallBack(string returnUrl, string wrap_verification_code)
        {
            if (wrap_verification_code != null)
            {
                try
                {
                    // Construct a request for an access token.
                    WebRequest tokenRequest = WebRequest.Create("https://consent.live.com/AccessToken.aspx");
                    tokenRequest.ContentType = "application/x-www-form-urlencoded";
                    tokenRequest.Method = "POST";
                    using (StreamWriter writer = new StreamWriter(tokenRequest.GetRequestStream()))
                    {
                        writer.Write(string.Format(
                        "wrap_client_id={0}&wrap_client_secret={1}&wrap_callback={2}&wrap_verification_code={3}",
                        Uri.EscapeDataString(LiveIdAppId),
                        Uri.EscapeDataString(LiveIdAppSecret),
                        Uri.EscapeDataString(GetLiveIdCallbackUrl()),
                        Uri.EscapeDataString(wrap_verification_code)));
                    }
                    // Send the request and get the response.
                    WebResponse tokenResponse = tokenRequest.GetResponse();

                    // Read the first line of the response body.
                    string tokenResponseText = new StreamReader(tokenResponse.GetResponseStream()).ReadLine();

                    // Parse the response body as being in the format of 'x-www-form-urlencoded'.
                    NameValueCollection tokenResponseData = HttpUtility.ParseQueryString(tokenResponseText);

                    return ValidateLogin(returnUrl, "liveid:" + tokenResponseData["uid"]);
                }
                catch (System.Net.WebException webException)
                {
                    string responseBody = null;
                    if (webException.Response != null)
                    {
                        using (StreamReader sr = new StreamReader(webException.Response.GetResponseStream(), Encoding.UTF8))
                        {
                            responseBody = sr.ReadToEnd();
                        }
                    }
                    ViewData["Message"] = string.Format("Failure occurred contacting consent service: Response=\r\n\r\n----\r\n{0}\r\n----\r\n\r\n", responseBody);
                    return View("Login");
                }
                catch
                {
                    ViewData["Message"] = "Failed to get access token. Ensure that the verifier token provided is valid.";
                    return View("Login");
                }
            }

            ViewData["Message"] = "Dat ging in ieder geval niet goed.";
            return View("Login");
        }


        private string GetFacebookCallbackUrl(string returnUrl)
        {
            if (Request.IsLocal)
            {
                return "http://localhost:" + Request.Url.Port + "/User/FacebookCallBack";// +"?returnUrl=" + returnUrl;
            }
            else
            {
                return "http://www.woneninzwaluwpark.nl/User/FacebookCallBack";// +"?returnUrl=" + returnUrl;
            }
        }

        public ActionResult FacebookCallBack(string returnUrl, string code, string error_reason, string fb_session_id)
        {
            if (code != null)
            {
                try
                {

                    // Construct a request for an access token.
                    WebRequest tokenRequest = WebRequest.Create(string.Format(
                        "https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}",
                        Uri.EscapeDataString(FacebookAppId),
                        Uri.EscapeDataString(GetFacebookCallbackUrl(returnUrl)),
                        Uri.EscapeDataString(FacebookAppSecret),
                        Uri.EscapeDataString(code)));
                    tokenRequest.Method = "GET";


                    // Send the request and get the response.
                    WebResponse tokenResponse = tokenRequest.GetResponse();
                    string tokenResponseText = new StreamReader(tokenResponse.GetResponseStream()).ReadLine();
                    NameValueCollection tokenResponseData = HttpUtility.ParseQueryString(tokenResponseText);

                    var request = WebRequest.Create("https://graph.facebook.com/me?access_token=" + Uri.EscapeDataString(tokenResponseData["access_token"]));
                    using (var response = request.GetResponse())
                    {
                        using (var responseStream = response.GetResponseStream())
                        {
                            var graph = FacebookGraph.Deserialize(responseStream);
                            return ValidateLogin(returnUrl, "facebook:" + graph.Id);
                        }
                    }
                }
                catch (System.Net.WebException webException)
                {
                    string responseBody = null;
                    if (webException.Response != null)
                    {
                        using (StreamReader sr = new StreamReader(webException.Response.GetResponseStream(), Encoding.UTF8))
                        {
                            responseBody = sr.ReadToEnd();
                        }
                    }
                    ViewData["Message"] = string.Format("Failure occurred contacting graph service: Response=", responseBody);
                    return View("Login");
                }
                catch
                {
                    ViewData["Message"] = "Failed to get access token. Ensure that the verifier token provided is valid.";
                    return View("Login");
                }
            }

            ViewData["Message"] = "Dat ging in ieder geval niet goed: " + error_reason;
            return View("Login");
        }

        public ActionResult Facebook(string returnUrl)
        {
            string redirectTo = string.Format(
                "https://graph.facebook.com/oauth/authorize?client_id={0}&redirect_uri={1}",
                Uri.EscapeDataString(FacebookAppId),
                Uri.EscapeDataString(GetFacebookCallbackUrl(returnUrl)));

            return Redirect(redirectTo);
        }

        public ActionResult OpenId(string returnUrl, FormCollection collection)
        {
            var response = openid.GetResponse();
            if (response == null)
            {
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(Request.Form["openid_identifier"], out id))
                {
                    try
                    {
                        return openid.CreateRequest(Request.Form["openid_identifier"], new Realm(Request.IsLocal ? Request.Url.ToString() : "http://www.woneninzwaluwpark.nl")).RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        ViewData["Message"] = ex.Message;
                        return View("Login");
                    }
                }
                else
                {
                    ViewData["Message"] = "Ongeldige openID provider";
                    return View("Login");
                }
            }
            else
            {
                // Stage 3: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:
                        return ValidateLogin(returnUrl, response.ClaimedIdentifier.ToString());
                    case AuthenticationStatus.Canceled:
                        ViewData["Message"] = "Geannuleerd bij provider";
                        return View("Login");
                    case AuthenticationStatus.Failed:
                        ViewData["Message"] = response.Exception.Message;
                        return View("Login");
                }
            }
            return new EmptyResult();
        }

        private ActionResult ValidateLogin(string returnUrl, string uniqueUserId)
        {
            Session["FriendlyIdentifier"] = uniqueUserId;
            FormsAuthentication.SetAuthCookie(uniqueUserId, false);
            using (var dc = new DataModelDataContext())
            {
                var log = new LoginLog();
                log.LoginDate = DateTime.Now;
                log.UserProfileId = uniqueUserId;
                log.UserIp = Request.UserHostAddress;
                dc.LoginLogs.InsertOnSubmit(log);
                dc.SubmitChanges();
                var profile = dc.UserProfiles
                    .Where(a => a.UniqueName == uniqueUserId)
                    .FirstOrDefault();

                if (profile == null)
                {
                    return RedirectToAction("Edit");
                }
                else
                {
                    Session["FriendlyName"] = profile.FriendlyName;
                    Session["UserId"] = profile.Id;
                    if (profile.EmailAddress == null || profile.ProfileVersion != CurrentProfileVersion)
                    {
                        return RedirectToAction("Edit");
                    }
                }
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
