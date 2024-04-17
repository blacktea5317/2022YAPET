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

namespace YAPET.Controllers
{
    public class LostsController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: Losts
        public ActionResult Index(String SpeciesNo, String GenderNo, String SizeNo, String CityNo, int page = 1)
        {
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName");
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender");
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size");
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName");

            var lost = from m in db.Lost
                        select m;

            if (!String.IsNullOrEmpty(SpeciesNo))
            {
                lost = lost.Where(x => x.SpeciesNo == SpeciesNo);
            }
            if (!String.IsNullOrEmpty(GenderNo))
            {
                lost = lost.Where(x => x.GenderNo == GenderNo);
            }
            if (!String.IsNullOrEmpty(SizeNo))
            {
                lost = lost.Where(x => x.SizeNo == SizeNo);
            }
            if (!String.IsNullOrEmpty(CityNo))
            {
                lost = lost.Where(x => x.CityNo == CityNo);
            }

            int pageSize = 12;
            int currentPage = page < 1 ? 1 : page;

            var pageLost = lost.OrderByDescending(n => n.Date).ToPagedList(currentPage, pageSize);

            return View(pageLost);
        }

        [LoginCheck]
        public ActionResult UserLost()
        {
            int userNo = ((User)Session["user"]).UserNo;
            List<Lost> userlost = db.Lost.Where(u => u.UserNo == userNo).OrderByDescending(n => n.Date).ToList();

            return View(userlost);
        }

        [ChildActionOnly]
        public ActionResult _UserLostIndex()
        {
            int userNo = ((User)Session["user"]).UserNo;

            var userlost = db.Lost.Where(u => u.UserNo == userNo).OrderByDescending(n => n.Date).Take(5).ToList();

            return PartialView(userlost);
        }

        // GET: Losts/Details/5
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
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName", lost.CityNo);
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender", lost.GenderNo);
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size", lost.SizeNo);
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName", lost.SpeciesNo);

            return View(lost);
        }

        [LoginCheck]
        public ActionResult Create()
        {
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName");
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender");
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size");
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName");
            return View();
        }

        [LoginCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string SpeciesNo, Lost lost, HttpPostedFileBase photo)
        {
            string lno = "";
            Lost lostno = db.Lost.Where(n => n.SpeciesNo == SpeciesNo).OrderByDescending(f => f.LostNo).FirstOrDefault();
            if (lostno == null)
            {
                lno = SpeciesNo + "000000001";
            }
            else
            {
                string temp = lostno.LostNo.Replace(SpeciesNo, "");
                lno = string.Format(SpeciesNo + "{0:000000000}", Convert.ToInt32(temp) + 1);
            }

            if (photo == null)
            {
                ViewBag.errMsg1 = "請上傳照片";
                return View(lost);
            }
            lost.ImageMimeType = photo.ContentType;
            lost.Photo = new byte[photo.ContentLength];
            photo.InputStream.Read(lost.Photo, 0, photo.ContentLength);
            lost.UserNo = ((User)Session["user"]).UserNo;
            lost.Date = DateTime.Now;
            lost.LostNo = lno;

            ModelState.Remove("Photo");
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
            return View(lost);
        }

        [LoginCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Lost lost, HttpPostedFileBase photo)
        {
            int user = ((User)Session["user"]).UserNo;
            if (lost.UserNo != user)
            {
                return Content("不要試圖修改他人文章");
            }

            if (photo != null)
            {
                lost.ImageMimeType = photo.ContentType;
                lost.Photo = new byte[photo.ContentLength];
                photo.InputStream.Read(lost.Photo, 0, photo.ContentLength);
            }

            ModelState.Remove("Photo");
            if (ModelState.IsValid)
            {
                db.Entry(lost).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName", lost.CityNo);
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender", lost.GenderNo);
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size", lost.SizeNo);
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName", lost.SpeciesNo);

            return RedirectToAction("Details", new { id = lost.LostNo });
        }

        [LoginCheck]
        public ActionResult Delete(string id)
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
