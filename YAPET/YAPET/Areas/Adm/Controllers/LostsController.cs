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
    public class LostsController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: Adm/Losts
        public ActionResult Index(int page = 1)
        {
            var lost = db.Lost.Include(a => a.City).Include(a => a.PetGender).Include(a => a.PetSize).Include(a => a.Species).Include(a => a.User);
            if (HttpContext.Session["adm"] != null)
            {
                return View("Index", "_Layout-Adm", lost.ToList());
                // return View("Index");
            }
            else
            {
                return View("Index", "_Layout", lost.ToList());
            }

        }

        // GET: Adm/Losts/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lost lost = db.Lost.Find(id);
            if (lost == null)
            {
                return HttpNotFound();
            }
            return View(lost);
        }

        //照片
        [LogReporter(IsLog = false)]
        public FileContentResult GetImage(string id)
        {
            var photo = db.Lost.Find(id);
            if (photo == null)
                return null;
            return File(photo.Photo, photo.ImageMimeType);
        }

        // GET: Adm/Losts/Create
        public ActionResult Create()
        {
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName");
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender");
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size");
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName");
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId");
            return View();
        }

        // POST: Adm/Losts/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LostNo,Photo,Name,SpeciesNo,GenderNo,SizeNo,Color,Age,Time,Place,ContactPerson,Contact,Details,ChipNumber,UserNo,CityNo,Date")] Lost lost)
        {
            if (ModelState.IsValid)
            {
                db.Lost.Add(lost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName", lost.CityNo);
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender", lost.GenderNo);
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size", lost.SizeNo);
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName", lost.SpeciesNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", lost.UserNo);
            return View(lost);
        }

        // GET: Adm/Losts/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lost lost = db.Lost.Find(id);
            if (lost == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName", lost.CityNo);
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender", lost.GenderNo);
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size", lost.SizeNo);
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName", lost.SpeciesNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", lost.UserNo);
            return View(lost);
        }

        // POST: Adm/Losts/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LostNo,Photo,Name,SpeciesNo,GenderNo,SizeNo,Color,Age,Time,Place,ContactPerson,Contact,Details,ChipNumber,UserNo,CityNo,Date")] Lost lost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName", lost.CityNo);
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender", lost.GenderNo);
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size", lost.SizeNo);
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName", lost.SpeciesNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", lost.UserNo);
            return View(lost);
        }

        // GET: Adm/Losts/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lost lost = db.Lost.Find(id);
            if (lost == null)
            {
                return HttpNotFound();
            }
            return View(lost);
        }

        // POST: Adm/Losts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Lost lost = db.Lost.Find(id);
            db.Lost.Remove(lost);
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
