using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YAPET.Models;
using System.Net;

namespace YAPET.Areas.Adm.Controllers
{
    public class AdmLoginController : Controller
    {
        Hw_MyPetsEntities db = new Hw_MyPetsEntities();
        // GET: Login
        public ActionResult AdmLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdmLogin(Login login)
        {
            var adm = db.Adm.Where(u => u.AdmId == login.Account && u.AdmPwd == login.Password).FirstOrDefault();

            if (adm == null)
            {

                ViewBag.ErrMsg = "帳號或密碼有錯誤!!";
                return View();
            }
            Session["adm"] = adm;

            return RedirectToAction("Index", "Activities");
        }
        public ActionResult AdmLogout()
        {
            Session["adm"] = null;
            return RedirectToAction("Login");
        }



    }
}