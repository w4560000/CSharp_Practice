using RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace 測試RabbitMQ.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult PublishMQ(string message)
        {
            string result = "success";

            try
            {
                RabbitMQConfig.GetPublisherModel().BasicPublish("leo", "hello", null, Encoding.UTF8.GetBytes(message));
            }
            catch (Exception ex)
            {
                result = "fail";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetCache()
        {
            return Json(HttpRuntime.Cache.Get("mq") as List<string>, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ClearCache()
        {
            HttpRuntime.Cache.Remove("mq");
            HttpRuntime.Cache.Insert("mq", new List<string>());
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}