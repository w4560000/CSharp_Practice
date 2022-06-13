using IFrameTestServer.Attribute;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IFrameTestServer.Controllers
{
    [IFrameAncestors]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.testData = "未登入狀態";

            try
            {
                if (System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value);
                    ViewBag.testData = HttpUtility.ParseQueryString(ticket.UserData)["TestData"];
                }
            }
            catch(Exception)
            {
                FormsAuthentication.SignOut();
            }

            ViewBag.sessionData = System.Web.HttpContext.Current.Session["SessionData"];

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "Test", DateTime.UtcNow, DateTime.UtcNow.AddMinutes(30), true, "TestData=已登入狀態");

            HttpCookie Cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                Expires = ticket.Expiration,
                HttpOnly = true,
                Path = string.IsNullOrWhiteSpace(ticket.CookiePath) ? FormsAuthentication.FormsCookiePath : ticket.CookiePath,
                Secure = FormsAuthentication.RequireSSL
            };

            System.Web.HttpContext.Current.Response.Cookies.Add(Cookie);
            System.Web.HttpContext.Current.Session["SessionData"] = "123";

            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            System.Web.HttpContext.Current.Session["SessionData"] = null;

            return Json("Success", JsonRequestBehavior.AllowGet);
        }
    }
}