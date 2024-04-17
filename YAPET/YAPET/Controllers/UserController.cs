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
    public class UserController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: User/Details/5
        [LoginCheck]
        public ActionResult Details()
        {
            int id = ((User)Session["user"]).UserNo;

            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [LoginCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user, HttpPostedFileBase photo)
        {
            if (photo != null)
            {
                user.ImageMimeType = photo.ContentType;
                user.Herder = new byte[photo.ContentLength];
                photo.InputStream.Read(user.Herder, 0, photo.ContentLength);
            }

            ModelState.Remove("Herder");
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Details", new { id = user.UserNo });
        }

        [LoginCheck]
        public ActionResult PwdChange()
        {
            return View();
        }

        [LoginCheck]
        [HttpPost]
        public ActionResult PwdChange(PwdChange pwdchange)
        {
            string pw = GetPwd.getHashPassword(pwdchange.OldPwd);
            string newpw = GetPwd.getHashPassword(pwdchange.NewPwd);

            int id = ((User)Session["user"]).UserNo;
            var user = db.User.Find(id);

            if (user.UserPwd == pw)
            {
                user.UserPwd = newpw;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            else
            {
                ViewBag.PwdErr = "原密碼有誤";
            }
            return View();
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
