using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YAPET.Areas.Adm.Controllers
{
    public class AdmLoginCheckController : ActionFilterAttribute
    {
        // GET: LoginCheck
        void LoginStatus(HttpContext context)
        {
            if (context.Session["adm"] == null)
            {
                context.Response.Redirect("/AdmLogin/AdmLogin");
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext context = HttpContext.Current;
            LoginStatus(context);
        }
    }
}