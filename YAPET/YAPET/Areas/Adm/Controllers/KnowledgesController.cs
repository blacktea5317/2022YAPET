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
    public class KnowledgesController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: Adm/Knowledges
        public ActionResult Index(int page = 1)
        {
            var Knowledge = db.Knowledge.Include(a => a.Adm);
            if (HttpContext.Session["adm"] != null)
            {
                return View("Index", "_Layout-Adm", Knowledge.ToList());
                // return View("Index");
            }
            else
            {
                return View("Index", "_Layout", Knowledge.ToList());
            }
        }

        // GET: Adm/Knowledges/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledge.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            return View(knowledge);
        }

        //照片
        [LogReporter(IsLog = false)]
        public FileContentResult GetImage(string id)
        {
            var photo = db.Knowledge.Find(id);
            if (photo == null)
                return null;
            return File(photo.Photo, photo.ImageMimeType);
        }

        // GET: Adm/Knowledges/Create
        public ActionResult Create()
        {
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId");
            return View();
        }

        // POST: Adm/Knowledges/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Knowledge knowledge, HttpPostedFileBase photo)
        {
                knowledge.AdmNo = ((YAPET.Models.Adm)Session["adm"]).AdmNo;
            if (photo == null)
            {
                ViewBag.errmsg = "請上傳照片";
                return View(knowledge);
            }
            if (db.Introduction.Find(knowledge.KnowledgeNo) != null)
            {
                ViewBag.errmsg2 = "編號重複";   
                return View(knowledge);
            }
            //建立編號
            Models.GetNo getnoService = new Models.GetNo();
            knowledge.KnowledgeNo = getnoService.GetNO("Introduction");

            knowledge.ImageMimeType = photo.ContentType;
            knowledge.Photo = new byte[photo.ContentLength];
            photo.InputStream.Read(knowledge.Photo, 0, photo.ContentLength);
            knowledge.Date = DateTime.Today;
            //products.Discontinued = true 下架

            //獨立處理欄位，不要驗證
            ModelState.Remove("Photo");
            if (ModelState.IsValid)
            {
                db.Knowledge.Add(knowledge);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", knowledge.AdmNo);
            return View(knowledge);
        }

        // GET: Adm/Knowledges/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledge.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", knowledge.AdmNo);
            return View(knowledge);
        }

        // POST: Adm/Knowledges/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Knowledge knowledge, HttpPostedFileBase photo)
        {
            if (photo != null)
            {
                knowledge.AdmNo = ((YAPET.Models.Adm)Session["adm"]).AdmNo;
                knowledge.ImageMimeType = photo.ContentType;
                knowledge.Photo = new byte[photo.ContentLength];
                photo.InputStream.Read(knowledge.Photo, 0, photo.ContentLength);
            }

            ModelState.Remove("Photo");
            if (ModelState.IsValid)
            {
                db.Entry(knowledge).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdmNo = new SelectList(db.Adm, "AdmNo", "AdmId", knowledge.AdmNo);
            return View(knowledge);
        }

        // GET: Adm/Knowledges/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledge.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            return View(knowledge);
        }

        // POST: Adm/Knowledges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Knowledge knowledge = db.Knowledge.Find(id);
            db.Knowledge.Remove(knowledge);
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
