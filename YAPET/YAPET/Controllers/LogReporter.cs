using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.IO;
using System.Text;
using YAPET.Models;

namespace YAPET.Controllers
{
    public class LogReporter : ActionFilterAttribute
    {
        public bool IsLog { get; set; }
        void LogRouteValues(RouteData routeData)
        {
            var controller = routeData.Values["controller"];
            var action = routeData.Values["action"];
            var parameter = routeData.Values["id"] == null ? "N/A" : routeData.Values["id"];

            var logTimeStamp = DateTime.Now;

            HttpContext context = HttpContext.Current;

            var Role = context.Session["user"] == null ? "Visiter" : ((User)context.Session["user"]).UserNo + ((User)context.Session["user"]).UserId;

            StreamWriter sw = new StreamWriter(context.Server.MapPath("\\LogRouteValues.csv"), true, Encoding.Default);

            sw.WriteLine(logTimeStamp + "," + Role + "," + controller + "," + action + "," + parameter);
            sw.Close();
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (IsLog)
            {
                LogRouteValues(filterContext.RouteData);
            }

        }
    }
}