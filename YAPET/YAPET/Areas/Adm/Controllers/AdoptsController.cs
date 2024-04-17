using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YAPET.Controllers;
using YAPET.Models;

namespace YAPET.Areas.Adm.Controllers
{
    [AdmLoginCheckController]
    public class AdoptsController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: Adm/Adopts
        public ActionResult Index(int page = 1)
        {
            var adopt = db.Adopt.Include(a => a.City).Include(a => a.PetGender).Include(a => a.PetSize).Include(a => a.Species).Include(a => a.User);
            if (HttpContext.Session["adm"] != null)
            {
                return View("Index", "_Layout-Adm", adopt.ToList());
                // return View("Index");
            }
            else
            {
                return View("Index", "_Layout", adopt.ToList());
            }
        }

        // GET: Adm/Adopts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adopt adopt = db.Adopt.Find(id);
            if (adopt == null)
            {
                return HttpNotFound();
            }
            return View(adopt);
        }

        //照片
        [LogReporter(IsLog = false)]
        public FileContentResult GetImage(string id)
        {
            var photo = db.Adopt.Find(id);
            if (photo == null)
                return null;
            return File(photo.Photo, photo.ImageMimeType);
        }

        // GET: Adm/Adopts/Create
        public ActionResult Create()
        {
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName");
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender");
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size");
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName");
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId");
            return View();
        }

        // POST: Adm/Adopts/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Adopt adopt, HttpPostedFileBase photo)
        {
            if (photo == null)
            {
                ViewBag.errmsg = "請上傳照片";
                return View(adopt);
            }
            if (db.Introduction.Find(adopt.AdoptNo) != null)
            {
                ViewBag.errmsg2 = "編號重複";
                return View(adopt);
            }

          

            //獨立處理欄位，不要驗證
            ModelState.Remove("Photo");
            if (ModelState.IsValid)
            {
                db.Adopt.Add(adopt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName", adopt.CityNo);
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender", adopt.GenderNo);
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size", adopt.SizeNo);
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName", adopt.SpeciesNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", adopt.UserNo);
            return View(adopt);
        }

        // GET: Adm/Adopts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adopt adopt = db.Adopt.Find(id);
            if (adopt == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName", adopt.CityNo);
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender", adopt.GenderNo);
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size", adopt.SizeNo);
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName", adopt.SpeciesNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", adopt.UserNo);
            return View(adopt);
        }

        // POST: Adm/Adopts/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AdoptNo,Photo,Name,SpeciesNo,GenderNo,SizeNo,Color,Age,State,LigationState,ContactPerson,Contact,Details,ChipNumber,UserNo,CityNo,Date")] Adopt adopt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adopt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName", adopt.CityNo);
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender", adopt.GenderNo);
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size", adopt.SizeNo);
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName", adopt.SpeciesNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", adopt.UserNo);
            return View(adopt);
        }

        // GET: Adm/Adopts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adopt adopt = db.Adopt.Find(id);
            if (adopt == null)
            {
                return HttpNotFound();
            }
            return View(adopt);
        }

        // POST: Adm/Adopts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Adopt adopt = db.Adopt.Find(id);
            db.Adopt.Remove(adopt);
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
