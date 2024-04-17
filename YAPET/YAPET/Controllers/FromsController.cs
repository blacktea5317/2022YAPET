using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YAPET.Models;

namespace YAPET.Controllers
{
    [LoginCheck]
    public class FromsController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        public ActionResult Create()
        {
            ViewBag.ProblemNo = new SelectList(db.Problem, "ProblemNo", "ProblemName");
            return View();
        }

        [HttpPost]
        public string Create(string ProblemNo, string Mail, string Content)
        {
            if (Mail == "" || Content == "")
            {
                return "表單送出失敗，請再試一次";
            }

            string fno = "";
            From from = db.From.Where(n => n.ProblemNo == ProblemNo).OrderByDescending(f => f.FromNo).FirstOrDefault();
            if (from == null)
            {
                fno = ProblemNo + "000000001";
            }
            else
            {
                string temp = from.FromNo.Replace(ProblemNo, "");
                fno = string.Format(ProblemNo + "{0:000000000}", Convert.ToInt32(temp) + 1);
            }

            From form = new From();
            form.FromNo = fno;
            form.ProblemNo = ProblemNo;
            form.Mail = Mail;
            form.Content = Content;
            form.State = false;
            if (ModelState.IsValid)
            {
                db.From.Add(form);
                db.SaveChanges();
            }
            return "表單已送出";
        }
    }
}
