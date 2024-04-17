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
    public class AdoptsController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: Adopts
        public ActionResult Index(String State, String SpeciesNo, String GenderNo, String SizeNo, String CityNo, int page = 1)
        {
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName");
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender");
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size");
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName");

            var adopt = from m in db.Adopt
                        select m;

            if (State == "on")
            {
                adopt = adopt.Where(x => x.State == true);
            }
            if (!String.IsNullOrEmpty(SpeciesNo))
            {
                adopt = adopt.Where(x => x.SpeciesNo == SpeciesNo);
            }
            if (!String.IsNullOrEmpty(GenderNo))
            {
                adopt = adopt.Where(x => x.GenderNo == GenderNo);
            }
            if (!String.IsNullOrEmpty(SizeNo))
            {
                adopt = adopt.Where(x => x.SizeNo == SizeNo);
            }
            if (!String.IsNullOrEmpty(CityNo))
            {
                adopt = adopt.Where(x => x.CityNo == CityNo);
            }

            int pageSize = 12;
            int currentPage = page < 1 ? 1 : page;

            var pageAdopt = adopt.OrderByDescending(n => n.Date).ToPagedList(currentPage, pageSize);

            return View(pageAdopt);
        }

        [LoginCheck]
        public ActionResult UserAdopt()
        {
            int userNo = ((User)Session["user"]).UserNo;
            List<Adopt> useradopt = db.Adopt.Where(u => u.UserNo == userNo).OrderByDescending(n => n.Date).ToList();

            return View(useradopt);
        }

        [ChildActionOnly]
        public ActionResult _UserAdoptIndex()
        {
            int userNo = ((User)Session["user"]).UserNo;

            var useradopt = db.Adopt.Where(u => u.UserNo == userNo).OrderByDescending(n => n.Date).Take(5).ToList();

            return PartialView(useradopt);
        }

        // GET: Adopts/Details/5
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
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName", adopt.CityNo);
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender", adopt.GenderNo);
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size", adopt.SizeNo);
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName", adopt.SpeciesNo);

            return View(adopt);
        }

        // GET: Adopts/Create
        [LoginCheck]
        public ActionResult Create()
        {
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName");
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender");
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size");
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName");
            return View();
        }

        // POST: Adopts/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [LoginCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string SpeciesNo, Adopt adopt, HttpPostedFileBase photo)
        {
            string ano = "";
            Adopt adoptno = db.Adopt.Where(n => n.SpeciesNo == SpeciesNo).OrderByDescending(f => f.AdoptNo).FirstOrDefault();
            if (adoptno == null)
            {
                ano = SpeciesNo + "000000001";
            }
            else
            {
                string temp = adoptno.AdoptNo.Replace(SpeciesNo, "");
                ano = string.Format(SpeciesNo + "{0:000000000}", Convert.ToInt32(temp) + 1);
            }

            if (photo == null)
            {
                ViewBag.errMsg1 = "請上傳照片";
                return View(adopt);
            }
            adopt.ImageMimeType = photo.ContentType;
            adopt.Photo = new byte[photo.ContentLength];
            photo.InputStream.Read(adopt.Photo, 0, photo.ContentLength);
            adopt.UserNo = ((User)Session["user"]).UserNo;
            adopt.Date = DateTime.Now;
            adopt.State = true;
            adopt.AdoptNo = ano;

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
            return View(adopt);
        }

        // POST: Adopts/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [LoginCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Adopt adopt, HttpPostedFileBase photo)
        {
            int user = ((User)Session["user"]).UserNo;
            if (adopt.UserNo != user)
            {
                return Content("不要試圖修改他人文章");
            }

            if (photo != null)
            {
                adopt.ImageMimeType = photo.ContentType;
                adopt.Photo = new byte[photo.ContentLength];
                photo.InputStream.Read(adopt.Photo, 0, photo.ContentLength);
            }

            ModelState.Remove("Photo");
            if (ModelState.IsValid)
            {
                db.Entry(adopt).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.CityNo = new SelectList(db.City, "CityNo", "CityName", adopt.CityNo);
            ViewBag.GenderNo = new SelectList(db.PetGender, "GenderNo", "Gender", adopt.GenderNo);
            ViewBag.SizeNo = new SelectList(db.PetSize, "SizeNo", "Size", adopt.SizeNo);
            ViewBag.SpeciesNo = new SelectList(db.Species, "SpeciesNo", "SpeciesName", adopt.SpeciesNo);

            return RedirectToAction("Details", new { id = adopt.AdoptNo });
        }

        // GET: Adopts/Delete/5
        [LoginCheck]
        public ActionResult Delete(string id)
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
