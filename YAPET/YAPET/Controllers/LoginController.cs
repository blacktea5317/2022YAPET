using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YAPET.Models;
using sendGmail;


namespace YAPET.Controllers
{
    public class LoginController : Controller
    {
        Hw_MyPetsEntities db = new Hw_MyPetsEntities();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            string pw = GetPwd.getHashPassword(login.Password);
            var user = db.User.Where(u => u.UserId == login.Account && u.UserPwd == pw).FirstOrDefault();

            if (user == null)
            {
                ViewBag.ErrMsg = "帳號或密碼有誤";
                return View();
            }
            if (user.State == true)
            {
                var freeze = db.Freeze.Where(n => n.UserNo == user.UserNo).OrderByDescending(n => n.StartDays).FirstOrDefault();
                if (freeze != null)
                {
                    if (DateTime.Now < freeze.EndDays)
                    {
                        ViewBag.ErrMsg = "帳號已被封鎖至" + freeze.EndDays;
                        return View();
                    }
                    else
                    {
                        user.State = false;
                        db.SaveChanges();
                    }
                }
            }
            Session["user"] = user;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Login");
        }

        public ActionResult PwdForget()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PwdForget(PwdForget mail)
        {
            var email = db.User.Where(e => e.EMail == mail.EMail).FirstOrDefault();

            if (email == null)
            {
                ViewBag.MailErr = "無此信箱";
                return View();
            }
            if (email != null)
            {

                string guid = Guid.NewGuid().ToString();
                SendEmail e = new SendEmail();
                e.sendGmail("YAPET", mail.EMail, "YAPET會員變更密碼", "<h1>更改密碼</h1><hr><a href='http://localhost:52608/Login/PwdReset?no=" + guid + "'>點我重新設定密碼</a><p>請在時效內點擊連結更改，超過時效請重新申請。</p>");

                db.ResetUserPwd.Add(new ResetUserPwd()
                {
                    Guid = guid,
                    State = false,
                    UserNo = email.UserNo,
                    Time = DateTime.Now.AddHours(3)
                });
                db.SaveChanges();

                ViewBag.MailErr = "已傳送至信箱";
                return View();
            }

            return View();
        }

        public ActionResult PwdReset(string no)
        {
            var pwdreset = db.ResetUserPwd.Where(rup => rup.Guid == no && rup.Time >= DateTime.Now && rup.State == false).FirstOrDefault();
            if (pwdreset == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public string PwdReset(string no, string NewPwd)
        {
            var pwdreset = db.ResetUserPwd.Where(rup => rup.Guid == no && rup.Time >= DateTime.Now && rup.State == false).FirstOrDefault();
            if (pwdreset == null)
            {
                return "密碼更改無效，請重新設定。";
            }

            string pw = GetPwd.getHashPassword(NewPwd);

            var userreset = db.User.Find(pwdreset.UserNo);
            userreset.UserPwd = pw;
            pwdreset.State = true;

            db.SaveChanges();
            return "密碼更改成功！";
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register register)
        {
            var userId = db.User.Where(u => u.UserId == register.UserAccount).FirstOrDefault();
            var userMail = db.User.Where(u => u.EMail == register.UserEmail).FirstOrDefault();
            if (userId != null)
            {
                ViewBag.ErrMsg = "帳號已使用";
                return View();
            }
            else if (userMail != null)
            {
                ViewBag.ErrMsg = "信箱已使用";
                return View();
            }

            string pw = GetPwd.getHashPassword(register.UserPwd);

            User use = new User();
            use.UserId = register.UserAccount;
            use.UserPwd = pw;
            use.Name = register.UserName;
            use.EMail = register.UserEmail;
            use.State = false;

            if (ModelState.IsValid)
            {
                db.User.Add(use);
                db.SaveChanges();

                return RedirectToAction("RegisterSuccess");
            }

            return View();
        }

        public ActionResult RegisterSuccess()
        {
            return View();
        }
    }
}