using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IFrameTestServer.Attribute
{
    // <summary>
    /// 允許嵌入iFrame的子網站Host
    /// </summary>
    public class IFrameAncestorsAttribute : ActionFilterAttribute
    {
        private readonly List<string> AllowIframeEmbedHostList = new List<string>() { "'self'", "https://localhost:44332/" };

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext.Current.Response.AppendHeader("Content-Security-Policy", "frame-ancestors " + AllowIframeEmbedHostList.Aggregate((hostList, next) => $"{hostList} {next}"));
        }
    }
}