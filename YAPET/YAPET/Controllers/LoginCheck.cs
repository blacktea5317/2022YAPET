using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YAPET.Controllers
{
    public class LoginCheck : ActionFilterAttribute
    {
        void LoginStatus(HttpContext context)
        {
            if (context.Session["user"] == null)
            {
                context.Response.Redirect("/Login/Login");
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            HttpContext context = HttpContext.Current;
            LoginStatus(context);
        }
    }
}