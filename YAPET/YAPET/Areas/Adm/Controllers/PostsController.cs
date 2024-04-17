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
    public class PostsController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: Adm/Posts
        [LogReporter(IsLog = false)]
        public FileContentResult GetImage(int id)
        {
            var photo = db.Photo.Find(id);
            if (photo == null)
                return null;

            return File(photo.Photo1, photo.ImageMimeType);
        }
        public ActionResult IndexAll(string searchTitle)
        {
           
            var post = db.Post.Where(p => p.State == "A"|| p.State == "B" || p.State == "C").OrderByDescending(n => n.Date).ToList();
            List<int> pno = post.Select(no => no.PostNo).ToList();
            var photo = db.Photo.Where(ph => pno.Contains(ph.PostNo)).ToList();

            if (!String.IsNullOrEmpty(searchTitle))
            {
                post = post.Where(s => s.Title.Contains(searchTitle) && s.State == "A" || s.State == "B" || s.State == "C").OrderByDescending(n => n.Date).ToList();
            }

            VMBlog blog = new VMBlog()
            {
                posttypes = db.PostType.ToList(),
                posts = post,
                photos = photo
            };

            return View(blog);
        }

        public ActionResult Index(string searchTitle,  string ptno = "A")
        {
            var post = db.Post.Where(p => p.PostTypeNo == ptno && p.State == "A" || p.State == "B" || p.State == "C").OrderByDescending(n => n.Date).ToList();
            List<int> pno = post.Select(no => no.PostNo).ToList();
            var photo = db.Photo.Where(ph => pno.Contains(ph.PostNo)).ToList();
            

            if (!String.IsNullOrEmpty(searchTitle))
            {
                post = post.Where(s => s.Title.Contains(searchTitle) && s.PostTypeNo == ptno && s.State == "A" || s.State == "B" || s.State == "C").OrderByDescending(n => n.Date).ToList();
            }

            VMBlog vmpt = new VMBlog()
            {
                posttypes = db.PostType.ToList(),
                posts = post,
                photos = photo
            };

            return View(vmpt);
        }

        // GET: Adm/Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.id = id;
            VMBlog vmptm = new VMBlog()
            {
                posts = db.Post.Where(c => c.PostNo == id).ToList(),
                photos = db.Photo.Where(ph => ph.PostNo == id).ToList(),
                messages = db.Message.Where(m => m.PostNo == id && m.State == true).Select(n =>
                  new VMMessage()
                  {
                      MessageNo = n.MessageNo,
                      UserNo = n.UserNo,
                      Date = n.Date,
                      Content = n.Content,
                      uId = db.User.Where(u => u.UserNo == n.UserNo).FirstOrDefault().UserId,
                      uNickname = db.User.Where(u => u.UserNo == n.UserNo).FirstOrDefault().Nickname

                  }).ToList()
            };

            //ViewBag.State = new SelectList(db.PostState, "StateNo", "StateName");

            return View(vmptm);
        }


        // GET: Adm/Posts/Delete/5
        public ActionResult Delete(int id)
        {
            Post post = db.Post.Find(id);
            post.State = "D";

            var photos = db.Photo.Where(p => p.PostNo == id).ToList();
            photos.ForEach(ps => ps.State = false);
            var messages = db.Message.Where(m => m.PostNo == id).ToList();
            messages.ForEach(ms => ms.State = false);

            db.Entry(post).State = EntityState.Modified;
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