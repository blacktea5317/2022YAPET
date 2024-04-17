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
using YAPET.Controllers;

namespace YAPET.Areas.Adm.Controllers
{
    [AdmLoginCheckController]
    public class IntroductionsController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: Adm/Introductions
        public ActionResult Index(int page = 1)
        {

            var introductions = db.Introduction.Include(a => a.Adm);
            if (HttpContext.Session["adm"] != null)
            {
                return View("Index", "_Layout-Adm", introductions.ToList());
                // return View("Index");
            }
            else
            {
                return View("Index", "_Layout", introductions.ToList());
            }

        }

        // GET: Adm/Introductions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Introduction introduction = db.Introduction.Find(id);
            if (introduction == null)
            {
                return HttpNotFound();
            }
            return View(introduction);
        }

        //照片
        [LogReporter(IsLog = false)]
        public FileContentResult GetImage(string id)
        {
            var photo = db.Introduction.Find(id);
            if (photo == null)
                return null;
            return File(photo.Photo, photo.ImageMimeType);
        }

        // GET: Adm/Introductions/Create
        public ActionResult Create()
        {
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId");
            return View();
        }

        // POST: Adm/Introductions/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Introduction introduction, HttpPostedFileBase photo)
        {
            introduction.AdmNo = ((YAPET.Models.Adm)Session["adm"]).AdmNo;

            if (photo == null)
            {
                ViewBag.errmsg = "請上傳照片";
                return View(introduction);
            }
            if (db.Introduction.Find(introduction.IntroductionNo) != null)
            {
                ViewBag.errmsg2 = "編號重複";
                return View(introduction);
            }
            //建立編號
            Models.GetNo getnoService = new Models.GetNo();
            introduction.IntroductionNo = getnoService.GetNO("Introduction");

           
            introduction.ImageMimeType = photo.ContentType;
            introduction.Photo = new byte[photo.ContentLength];
            photo.InputStream.Read(introduction.Photo, 0, photo.ContentLength);
            introduction.Date = DateTime.Today;
            //products.Discontinued = true 下架

            //獨立處理欄位，不要驗證
            ModelState.Remove("Photo");
            if (ModelState.IsValid)
            {
                db.Introduction.Add(introduction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", introduction.AdmNo);
            return View(introduction);
        }

        // GET: Adm/Introductions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Introduction introduction = db.Introduction.Find(id);
            if (introduction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", introduction.AdmNo);
            return View(introduction);
        }

        // POST: Adm/Introductions/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Introduction introduction, HttpPostedFileBase photo)
        {
            introduction.AdmNo = ((YAPET.Models.Adm)Session["adm"]).AdmNo;

            if (photo != null)
            {
                introduction.ImageMimeType = photo.ContentType;
                introduction.Photo = new byte[photo.ContentLength];
                photo.InputStream.Read(introduction.Photo, 0, photo.ContentLength);
            }

            ModelState.Remove("Photo");
            if (ModelState.IsValid)
            {
                db.Entry(introduction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", introduction.AdmNo);
            return View(introduction);
        }

        // GET: Adm/Introductions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Introduction introduction = db.Introduction.Find(id);
            if (introduction == null)
            {
                return HttpNotFound();
            }
            return View(introduction);
        }

        // POST: Adm/Introductions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Introduction introduction = db.Introduction.Find(id);
            db.Introduction.Remove(introduction);
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
