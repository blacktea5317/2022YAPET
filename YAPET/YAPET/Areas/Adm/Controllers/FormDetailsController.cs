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
    public class FormDetailsController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: Adm/FormDetails
        public ActionResult Index(int page = 1)
        {
            var formDetail = db.FormDetail.ToList();

            int pageSize = 15;
            int currentPage = pageSize < 1 ? 1 : page;

            var pagedFormDetail = formDetail.ToPagedList(currentPage, pageSize);

            return View(pagedFormDetail);


        }

        // GET: Adm/FormDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormDetail formDetail = db.FormDetail.Find(id);
            if (formDetail == null)
            {
                return HttpNotFound();
            }
            return View(formDetail);
        }

        // GET: Adm/FormDetails/Create
        public ActionResult Create()
        {
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId");
            ViewBag.FormNo = new SelectList(db.From, "FromNo", "ProblemNo");
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId");
            return View();
        }

        // POST: Adm/FormDetails/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DetailNo,FormNo,UserNo,State,AdmNo")] FormDetail formDetail)
        {
            if (ModelState.IsValid)
            {
                db.FormDetail.Add(formDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", formDetail.AdmNo);
            ViewBag.FormNo = new SelectList(db.From, "FromNo", "ProblemNo", formDetail.FormNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", formDetail.UserNo);
            return View(formDetail);
        }

        // GET: Adm/FormDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormDetail formDetail = db.FormDetail.Find(id);
            if (formDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", formDetail.AdmNo);
            ViewBag.FormNo = new SelectList(db.From, "FromNo", "ProblemNo", formDetail.FormNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", formDetail.UserNo);
            return View(formDetail);
        }

        // POST: Adm/FormDetails/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DetailNo,FormNo,UserNo,State,AdmNo")] FormDetail formDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(formDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", formDetail.AdmNo);
            ViewBag.FormNo = new SelectList(db.From, "FromNo", "ProblemNo", formDetail.FormNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", formDetail.UserNo);
            return View(formDetail);
        }

        // GET: Adm/FormDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormDetail formDetail = db.FormDetail.Find(id);
            if (formDetail == null)
            {
                return HttpNotFound();
            }
            return View(formDetail);
        }

        // POST: Adm/FormDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FormDetail formDetail = db.FormDetail.Find(id);
            db.FormDetail.Remove(formDetail);
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
