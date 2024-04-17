using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YAPET.Areas.Adm.Controllers
{
    public class AdmHomeController : Controller
    {
        // GET: Adm/AdmHome
        public ActionResult Index()
        {
            return View();
        }
    }
}