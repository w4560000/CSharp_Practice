using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RabbitMQ;
using RabbitMQ.Client.Events;

namespace 測試RabbitMQ
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RabbitMQConfig.InitReceiver(null);
            HttpRuntime.Cache.Insert("mq", new List<string>());
        }

        /// <summary>
        /// 收到MQ訊息後
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            string msg = Encoding.UTF8.GetString(e.Body.ToArray());

            (HttpRuntime.Cache.Get("mq") as List<string>).Add(msg);
        }
    }
}
