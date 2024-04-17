using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YAPET.Models;

namespace YAPET.Controllers
{
    public class PhotosController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        [LogReporter(IsLog = false)]
        public ActionResult Index()
        {
            var photo = db.Photo.Include(p => p.Post);
            return View(photo.ToList());
        }

        public FileContentResult GetImage(int id)
        {
            var photo = db.Photo.Find(id);
            if (photo == null)
                return null;

            return File(photo.Photo1, photo.ImageMimeType);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Photo photos, HttpPostedFileBase photo)
        {
            photos.ImageMimeType = photo.ContentType;
            photos.Photo1 = new byte[photo.ContentLength];
            photo.InputStream.Read(photos.Photo1, 0, photo.ContentLength);
            photos.State = true;
            ModelState.Remove("Photo1");
            if (ModelState.IsValid)
            {
                db.Photo.Add(photos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(photos);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photo.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }

            return View(photo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Photo photos, HttpPostedFileBase photo)
        {
            if (photo != null)
            {
                photos.ImageMimeType = photo.ContentType;
                photos.Photo1 = new byte[photo.ContentLength];
                photo.InputStream.Read(photos.Photo1, 0, photo.ContentLength);
            }
            ModelState.Remove("Photo1");
            if (ModelState.IsValid)
            {
                db.Entry(photos).State = EntityState.Modified;
                db.SaveChanges();
            }
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
