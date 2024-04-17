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
    
    public class ActivitiesController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: Adm/Activities
        [AdmLoginCheckController]
        public ActionResult Index(int page = 1)
        {
            var activity = db.Activity.Include(a => a.Adm);
            if (HttpContext.Session["adm"] != null)
            {
                return View("Index", "_Layout-Adm", activity.ToList());
                // return View("Index");
            }
            else
            {
                return View("Index", "_Layout", activity.ToList());
            }

        }
        [AdmLoginCheckController]
        // GET: Adm/Activities/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activity.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }
        //照片
        [LogReporter(IsLog = false)]
        public FileContentResult GetImage(string id)
        {
            var photo = db.Activity.Find(id);
            if (photo == null)
                return null;
            return File(photo.Photo, photo.ImageMimeType);
        }
        [AdmLoginCheckController]
        // GET: Adm/Activities/Create
        public ActionResult Create()
        {
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId");
            return View();
        }

        // POST: Adm/Activities/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdmLoginCheckController]
        public ActionResult Create(Activity activity, HttpPostedFileBase photo)
        {

            activity.AdmNo = ((YAPET.Models.Adm)Session["adm"]).AdmNo;
            if (photo == null)
            {
                ViewBag.errmsg = "請上傳照片";
                return View(activity);
            }

            //建立編號
            Models.GetNo getnoService = new Models.GetNo();
            activity.ActivityNo = getnoService.GetNO("Activity");

            
            activity.ImageMimeType = photo.ContentType;
            activity.Photo = new byte[photo.ContentLength];
            photo.InputStream.Read(activity.Photo, 0, photo.ContentLength);
            activity.Date = DateTime.Today;
            //products.Discontinued = true 下架

            //獨立處理欄位，不要驗證
            ModelState.Remove("Photo");
            if (ModelState.IsValid)
            {
                db.Activity.Add(activity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", activity.AdmNo);
            return View(activity);
        }

        // GET: Adm/Activities/Edit/5
        [AdmLoginCheckController]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activity.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", activity.AdmNo);
            return View(activity);
        }

        // POST: Adm/Activities/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdmLoginCheckController]
        public ActionResult Edit(Activity activity, HttpPostedFileBase photo)
        {
                activity.AdmNo = ((YAPET.Models.Adm)Session["adm"]).AdmNo;
            if (photo != null)
            {
                
                activity.ImageMimeType = photo.ContentType;
                activity.Photo = new byte[photo.ContentLength];
                photo.InputStream.Read(activity.Photo, 0, photo.ContentLength);
            }
            ModelState.Remove("Photo");
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", activity.AdmNo);
            return View(activity);
        }

        // GET: Adm/Activities/Delete/5
        [AdmLoginCheckController]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activity.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Adm/Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AdmLoginCheckController]
        public ActionResult DeleteConfirmed(string id)
        {
            Activity activity = db.Activity.Find(id);
            db.Activity.Remove(activity);
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