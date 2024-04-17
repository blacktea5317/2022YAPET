using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YAPET.Models;
using PagedList;
using PagedList.Mvc;

namespace YAPET.Areas.Adm.Controllers
{
    [AdmLoginCheckController]
    public class FreezesController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: Adm/Freezes
        public ActionResult Index(int page = 1)
        {
            //var freeze = db.Freeze.ToList();

            //int pageSize = 15;
            //int currentPage = pageSize < 1 ? 1 : page;

            //var pagedFreeze = freeze.ToPagedList(currentPage, pageSize);

            return View(db.Freeze.ToList());


        }

        // GET: Adm/Freezes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freeze freeze = db.Freeze.Find(id);
            if (freeze == null)
            {
                return HttpNotFound();
            }
            return View(freeze);
        }

        // GET: Adm/Freezes/Create
        public ActionResult Create()
        {
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId");
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId");
            return View();
        }

        // POST: Adm/Freezes/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FreezeNo,UserNo,StartDays,EndDays,Reason,AdmNo,State")] Freeze freeze)
        {
            if (ModelState.IsValid)
            {
                db.Freeze.Add(freeze);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", freeze.AdmNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", freeze.UserNo);
            return View(freeze);
        }

        // GET: Adm/Freezes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freeze freeze = db.Freeze.Find(id);
            if (freeze == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", freeze.AdmNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", freeze.UserNo);
            return View(freeze);
        }

        // POST: Adm/Freezes/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FreezeNo,UserNo,StartDays,EndDays,Reason,AdmNo,State")] Freeze freeze)
        {
            if (ModelState.IsValid)
            {
                db.Entry(freeze).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", freeze.AdmNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", freeze.UserNo);
            return View(freeze);
        }

        // GET: Adm/Freezes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Freeze freeze = db.Freeze.Find(id);
            if (freeze == null)
            {
                return HttpNotFound();
            }
            return View(freeze);
        }

        // POST: Adm/Freezes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Freeze freeze = db.Freeze.Find(id);
            db.Freeze.Remove(freeze);
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
