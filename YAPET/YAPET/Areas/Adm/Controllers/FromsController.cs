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
    public class FromsController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: Adm/Froms
        public ActionResult Index()
        {
            var from = db.From.Include(f => f.Problem);
            return View(from.ToList());
        }

        // GET: Adm/Froms/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            From from = db.From.Find(id);
            if (from == null)
            {
                return HttpNotFound();
            }
            return View(from);
        }

        // GET: Adm/Froms/Create
        public ActionResult Create()
        {
            ViewBag.ProblemNo = new SelectList(db.Problem, "ProblemNo", "ProblemName");
            return View();
        }

        // POST: Adm/Froms/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FromNo,ProblemNo,Mail,Content,State")] From from)
        {
            if (ModelState.IsValid)
            {
                db.From.Add(from);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProblemNo = new SelectList(db.Problem, "ProblemNo", "ProblemName", from.ProblemNo);
            return View(from);
        }

        // GET: Adm/Froms/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            From from = db.From.Find(id);
            if (from == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProblemNo = new SelectList(db.Problem, "ProblemNo", "ProblemName", from.ProblemNo);
            return View(from);
        }

        // POST: Adm/Froms/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FromNo,ProblemNo,Mail,Content,State")] From from)
        {
            if (ModelState.IsValid)
            {
                db.Entry(from).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProblemNo = new SelectList(db.Problem, "ProblemNo", "ProblemName", from.ProblemNo);
            return View(from);
        }

        // GET: Adm/Froms/Delete/5
        public ActionResult Delete(string id)
        {
            From from = db.From.Find(id);
            from.State = true;



            db.Entry(from).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = from.FromNo });
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
