using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YAPET.Models;

namespace YAPET.Areas.Adm.Controllers
{
     [AdmLoginCheckController]
    public class ReportsController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: Adm/Reports
        public ActionResult Index()
        {
            var report = db.Report.Include(r => r.Post).Include(r => r.User);
            return View(report.ToList());
        }

        // GET: Adm/Reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Report.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: Adm/Reports/Create
        public ActionResult Create()
        {
            ViewBag.PostNo = new SelectList(db.Post, "PostNo", "Title");
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId");
            return View();
        }

        // POST: Adm/Reports/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReportNo,PostNo,UserNo,Time,Reason,State")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Report.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostNo = new SelectList(db.Post, "PostNo", "Title", report.PostNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", report.UserNo);
            return View(report);
        }

        // POST: Adm/Reports/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int userNo, int days, string reason, int postid)
        {
            User user = db.User.Where(n => n.UserNo == userNo).FirstOrDefault();
            if (user != null)
            {
                YAPET.Models.Adm adm = (YAPET.Models.Adm)Session["adm"];

                //建立編號
                Models.GetNo getnoService = new Models.GetNo();

                DateTime date = DateTime.Now;
                Freeze freeze = new Freeze();
                freeze.FreezeNo = getnoService.GetNO("Freeze"); ;
                freeze.UserNo = user.UserNo;
                freeze.StartDays = date;
                freeze.EndDays = date.AddDays(days);
                freeze.Reason = reason;
                freeze.AdmNo = adm.AdmNo;
                freeze.State = true;

                Post post = new Post();
                post = db.Post.Where(p => p.PostNo == postid).FirstOrDefault();
                post.State = "E";
                db.Entry(post).State = EntityState.Modified;
                db.Freeze.Add(freeze);
                user.State = true;
                db.SaveChanges();
            }

            return RedirectToAction("Index");

        }
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report Report = db.Report.Find(id);

            if (Report == null)
            {
                return HttpNotFound();
            }
            return View(Report);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Report.Find(id );
            report.State = true;

            var a = report.PostNo;


            var post = db.Post.Where(p => p.PostNo == a).ToList();
            post.ForEach(ps => ps.State ="E");

            var photos = db.Photo.Where(p => p.PostNo == a).ToList();
            photos.ForEach(ps => ps.State = false);

            var messages = db.Message.Where(m => m.PostNo == a).ToList();
            messages.ForEach(ms => ms.State = false);

            db.Entry(report).State = EntityState.Modified;
          
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
