using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YAPET.Models;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;
using Newtonsoft.Json;

namespace YAPET.Controllers
{
    public class VMBlogController : Controller
    {
        Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        [ChildActionOnly]
        public ActionResult _NewPost()
        {
            var post = db.Post.Where(p => p.State == "A").OrderByDescending(n => n.Date).Take(3).ToList();
            List<int> pno = post.Select(no => no.PostNo).ToList();
            var photo = db.Photo.Where(ph => pno.Contains(ph.PostNo)).ToList();

            VMBlog newpost = new VMBlog()
            {
                posts = post,
                photos = photo
            };

            return PartialView(newpost);
        }

        //黑單部份已修改好
        public ActionResult Index(string searchTitle, string ptno = "", int page = 1)
        {
            int pageSize = 12;
            int currentPage = page < 1 ? 1 : page;
            int temp = pageSize * (currentPage - 1);

            var user = (User)Session["user"];
            int userNo = 0;

            List<int> listBlack = new List<int>();
            List<int> listBlacked = new List<int>();
            List<int> listFollow = new List<int>();
            if (user != null)
            {
                userNo = user.UserNo;
                listBlack = db.Black.Where(n => n.UserNo == user.UserNo && n.State == true).Select(n => n.BlackedNo).ToList();
                listBlacked = db.Black.Where(n => n.BlackedNo == user.UserNo && n.State == true).Select(n => n.UserNo).ToList();
                listFollow = db.Follow.Where(n => n.UserNo == user.UserNo && n.State == true).Select(n => n.FollowedNo).ToList();
            }
            List<Post> post = new List<Post>();

            post = (from posts in db.Post
                    where (string.IsNullOrEmpty(ptno) ? true : posts.PostTypeNo == ptno) &&
                    (string.IsNullOrEmpty(searchTitle) ? true : posts.Title.Contains(searchTitle)) &&
                    ((listBlack.Count() <= 0) ? true : !listBlack.Contains(posts.UserNo)) &&
                    ((listBlacked.Count() <= 0) ? true : !listBlacked.Contains(posts.UserNo)) &&
                    (posts.State == "A" || posts.State == "C") &&
                    (posts.State != "C" || listFollow.Contains(posts.UserNo) || posts.UserNo == userNo)
                    orderby posts.Date descending
                    select posts).ToList();

            List<int> pno = post.Select(no => no.PostNo).ToList();
            var photo = db.Photo.Where(ph => pno.Contains(ph.PostNo)).ToList();

            VMBlog vmpt = new VMBlog()
            {
                posttypes = db.PostType.ToList(),
                posts = post.Skip(temp).Take(pageSize).ToList(),
                photos = photo,
                PageCount = (post.Count() / pageSize) + 1
            };

            return View(vmpt);
        }

        [ChildActionOnly]
        public ActionResult _UserPostIndex()
        {
            int userNo = ((User)Session["user"]).UserNo;

            var post = db.Post.Where(p => p.UserNo == userNo && p.State == "A" || p.State == "B" || p.State == "C").OrderByDescending(n => n.Date).Take(5).ToList();
            List<int> pno = post.Select(no => no.PostNo).ToList();
            var photo = db.Photo.Where(ph => pno.Contains(ph.PostNo)).ToList();

            VMBlog userpostIndex = new VMBlog()
            {
                posts = post,
                photos = photo
            };

            return PartialView(userpostIndex);
        }

        //黑單部份已修改好
        public ActionResult UserBlog(int? id)
        {
            ViewBag.NO = id;
            ViewBag.Name = db.User.Where(c => c.UserNo == id).FirstOrDefault().Name;
            ViewBag.AboutMe = db.User.Where(n => n.UserNo == id).FirstOrDefault().AboutMe;
            ViewBag.Follow = db.Follow.Count(c => c.UserNo == id && c.State == true);
            ViewBag.Followed = db.Follow.Count(c => c.FollowedNo == id && c.State == true);

            var user = (User)Session["user"];
            int userNo = 0;
            List<int> listBlacked = new List<int>();
            List<int> listBlack = new List<int>();
            List<int> listFollow = new List<int>();
            if (user != null)
            {
                userNo = user.UserNo;
                listBlack = db.Black.Where(n => n.UserNo == user.UserNo && n.State == true).Select(n => n.BlackedNo).ToList();
                listBlacked = db.Black.Where(n => n.BlackedNo == user.UserNo && n.State == true).Select(n => n.UserNo).ToList();
                listFollow = db.Follow.Where(n => n.UserNo == user.UserNo && n.State == true).Select(n => n.FollowedNo).ToList();
            }
            List<Post> post = new List<Post>();

            post = (from posts in db.Post
                    where (posts.UserNo == id) &&
                    ((listBlack.Count() <= 0) ? true : !listBlack.Contains(posts.UserNo)) &&
                    ((listBlacked.Count() <= 0) ? true : !listBlacked.Contains(posts.UserNo)) &&
                    (posts.State == "A" || posts.State == "B" || posts.State == "C") &&
                    (posts.State != "B" || posts.UserNo == userNo) &&
                    (posts.State != "C" || listFollow.Contains(posts.UserNo) || posts.UserNo == userNo)
                    orderby posts.Date descending
                    select posts).ToList();

            List<int> pno = post.Select(no => no.PostNo).ToList();
            var photo = db.Photo.Where(ph => pno.Contains(ph.PostNo)).ToList();

            VMBlog userblog = new VMBlog()
            {
                posts = post,
                photos = photo,
                follows = db.Follow.ToList(),
                blacks = db.Black.ToList()
            };

            return View(userblog);
        }

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
                likes = db.Like.Where(c => c.PostNo == id && c.State == true).ToList(),
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

            return View(vmptm);
        }

        [LoginCheck]
        public ActionResult Create()
        {
            ViewBag.PostTypeNo = new SelectList(db.PostType, "PostTypeNo", "PostType1");
            ViewBag.State = new SelectList(db.PostState, "StateNo", "StateName");
            return View();
        }

        [LoginCheck]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VMPost vmp, HttpPostedFileBase[] photo)
        {
            Post posts = new Post();
            posts.UserNo = ((User)Session["user"]).UserNo;
            posts.Title = vmp.Title;
            posts.Content = vmp.Content;
            posts.Date = DateTime.Now;
            posts.State = vmp.State;
            posts.PostTypeNo = vmp.PostTypeNo;

            List<Photo> photos = new List<Photo>();

            for (int i = 0; i < photo.Length; i++)
            {
                HttpPostedFileBase f = (HttpPostedFileBase)photo[i];
                if (f != null && f.ContentLength > 0)
                {
                    vmp.ImageMimeType = f.ContentType;
                    vmp.Photo1 = new byte[f.ContentLength];
                    f.InputStream.Read(vmp.Photo1, 0, f.ContentLength);
                    photos.Add(new Photo()
                    {
                        State = true,
                        PostNo = posts.PostNo,
                        ImageMimeType = vmp.ImageMimeType,
                        Photo1 = vmp.Photo1
                    });
                }
            }

            ModelState.Remove("Photo1");
            if (ModelState.IsValid)
            {
                if (photo.Count() >= 1)
                {
                    db.Photo.AddRange(photos);
                }
                db.Post.Add(posts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PostTypeNo = new SelectList(db.PostType, "PostTypeNo", "PostType1", vmp.PostTypeNo);
            ViewBag.State = new SelectList(db.PostState, "StateNo", "StateName", vmp.State);
            return RedirectToAction("Details", new { id = vmp.PostNo });
        }

        //////////////////////////////修改還在寫//////////////////////////////
        public ActionResult Edit(int id)
        {
            int userNo = ((User)Session["user"]).UserNo;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            if (post.UserNo != userNo)
            {
                return RedirectToAction("Index");
            }


            ViewBag.PostTypeNo = new SelectList(db.PostType, "PostTypeNo", "PostType1", post.PostTypeNo);
            ViewBag.StateNo = new SelectList(db.PostState, "StateNo", "StateName", post.State);
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post, int id)
        {

            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }
            ViewBag.PostTypeNo = new SelectList(db.PostType, "PostTypeNo", "PostType1", post.PostTypeNo);
            ViewBag.StateNo = new SelectList(db.PostState, "StateNo", "StateName", post.State);
            return View(post);
        }

        [LoginCheck]
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

        [LoginCheck]
        [HttpPost]
        public string ReportPost(int id, string reason)
        {
            Report report = new Report();
            report.PostNo = id;
            report.UserNo = ((User)Session["user"]).UserNo;
            report.Reason = reason;
            report.Time = DateTime.Now;
            report.State = false;

            if (ModelState.IsValid)
            {
                db.Report.Add(report);
                db.SaveChanges();
                return "檢舉已送出";
            }
            return "檢舉送出失敗，請再試一次";
        }

        //按讚
        public string Like(int postid)
        {
            bool State = true;
            User u = ((User)Session["user"]);
            var postLike = db.Like.Where(n => n.PostNo == postid && n.UserNo == u.UserNo).FirstOrDefault();
            if (postLike == null)
            {
                postLike = new Like();
                postLike.PostNo = postid;
                postLike.State = true;
                postLike.UserNo = u.UserNo;
                postLike.Time = DateTime.Now;
                db.Like.Add(postLike);
            }
            else
            {
                State = !postLike.State;
                postLike.State = !postLike.State;
            }

            var messAges = new
            {
                PostNo = postid,
                State = State
            };
            db.SaveChanges();

            return JsonConvert.SerializeObject(messAges);
        }

        //追蹤
        public string Followers(int foid)
        {
            bool State = true;
            User u = ((User)Session["user"]);
            var followLike = db.Follow.Where(n => n.FollowedNo == foid && n.UserNo == u.UserNo).FirstOrDefault();
            if (followLike == null)
            {
                followLike = new Follow();
                followLike.FollowedNo = foid;
                followLike.State = true;
                followLike.UserNo = u.UserNo;
                db.Follow.Add(followLike);
            }
            else
            {
                State = !followLike.State;
                followLike.State = !followLike.State;
            }

            var mes = new
            {
                FollowedNo = foid,
                State = State
            };
            db.SaveChanges();

            return JsonConvert.SerializeObject(mes);
        }

        [ChildActionOnly]
        public ActionResult _Following(int? id)
        {
            var following = db.Follow.Where(c => c.UserNo == id && c.State == true).ToList();

            return PartialView(following);
        }
        [ChildActionOnly]
        public ActionResult _Follower(int? id)
        {
            var followers = db.Follow.Where(c => c.FollowedNo == id && c.State == true).ToList();

            return PartialView(followers);
        }

        //黑單
        public string Black(int blid)
        {
            bool State = true;
            User u = ((User)Session["user"]);
            var blacked = db.Black.Where(n => n.BlackedNo == blid && n.UserNo == u.UserNo).FirstOrDefault();
            if (blacked == null)
            {
                blacked = new Black();
                blacked.BlackedNo = blid;
                blacked.State = true;
                blacked.UserNo = u.UserNo;
                db.Black.Add(blacked);
            }
            else
            {
                State = !blacked.State;
                blacked.State = !blacked.State;
            }

            var mes = new
            {
                BlackedNo = blid,
                State = State
            };
            db.SaveChanges();

            return JsonConvert.SerializeObject(mes);
        }

        public ActionResult BlackDelete(int id)
        {
            Black black = db.Black.Find(id);
            black.State = false;

            db.Entry(black).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", "User");
        }

        [ChildActionOnly]
        public ActionResult _Black()
        {
            int id = ((User)Session["user"]).UserNo;
            var blacks = db.Black.Where(c => c.UserNo == id && c.State == true).ToList();

            return PartialView(blacks);
        }

        //留言部分
        [HttpPost]
        public ActionResult CreateMessage(int id, string content)
        {
            Message message = new Message();
            message.PostNo = id;
            message.UserNo = ((User)Session["user"]).UserNo;
            message.Content = content;
            message.Date = DateTime.Now;
            message.State = true;

            if (ModelState.IsValid)
            {
                db.Message.Add(message);
                db.SaveChanges();
            }
            ViewBag.No = message.MessageNo;
            var user = db.User.Where(n => n.UserNo == message.UserNo).FirstOrDefault();
            return Json(new
            {
                No = message.MessageNo,
                Content = message.Content,
                Date = message.Date.ToString("yyyy/MM/dd HH:mm:ss"), //2022/2/22 上午 12:00:00
                uId = user.UserId,
                uNickname = user.Nickname,
                uNo = user.UserNo
            });
        }
        public ActionResult DeleteMessage(int id)
        {
            Message message = db.Message.Find(id);
            message.State = false;
            db.Entry(message).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = message.PostNo });
        }

        [LoginCheck]
        [HttpPost]
        public string ReportMessage(int id, string reason)
        {
            if (Session["user"] == null)
            {
                return "請先登入";
            }

            MsgReport msgreport = new MsgReport();
            msgreport.MessageNo = id;
            msgreport.UserNo = ((User)Session["user"]).UserNo;
            msgreport.Reason = reason;
            msgreport.Time = DateTime.Now;
            msgreport.State = false;

            if (ModelState.IsValid)
            {
                db.MsgReport.Add(msgreport);
                db.SaveChanges();
                return "檢舉已送出";
            }
            return "檢舉送出失敗，請再試一次";
        }

    }


}