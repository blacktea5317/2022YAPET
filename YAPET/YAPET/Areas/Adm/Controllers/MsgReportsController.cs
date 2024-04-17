using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YAPET.Models;

namespace YAPET.Areas.Adm.Controllers
{
    [AdmLoginCheckController]
    public class MsgReportsController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // GET: Adm/MsgReports
        public ActionResult Index()
        {
            var msgReport = db.MsgReport.Include(m => m.Message ).Include(m => m.User).ToList(); 
            return View(msgReport.ToList());
        }

        // GET: Adm/MsgReports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MsgReport msgReport = db.MsgReport.Find(id);
            if (msgReport == null)
            {
                return HttpNotFound();
            }
            return View(msgReport);
        }


    
        // GET: Adm/MsgReports/Create
        public ActionResult Create()
        {
            ViewBag.MessageNo = new SelectList(db.Message, "MessageNo", "Content");
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId");
            return View();
        }

        // POST: Adm/MsgReports/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MsgReportNo,MessageNo,UserNo,Time,Reason,State")] MsgReport msgReport)
        {
            if (ModelState.IsValid)
            {
                db.MsgReport.Add(msgReport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MessageNo = new SelectList(db.Message, "MessageNo", "Content", msgReport.MessageNo);
            ViewBag.UserNo = new SelectList(db.User, "UserNo", "UserId", msgReport.UserNo);
            return View(msgReport);
        }


        // POST: Adm/MsgReports/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int userNo,int days,string reason,int msgid,int id)
        {
            User user = db.User.Where(n => n.UserNo == userNo).FirstOrDefault();
            if (user != null)
            {
                YAPET.Models.Adm adm = (YAPET.Models.Adm)Session["adm"];

                //建立編號
                Models.GetNo getnoService = new Models.GetNo();

                DateTime date = DateTime.Now;
                Freeze freeze = new Freeze();
                freeze.FreezeNo = getnoService.GetNO("Freeze"); ;
                freeze.UserNo = user.UserNo;
                freeze.StartDays = date;
                freeze.EndDays = date.AddDays(days);
                freeze.Reason = reason;
                freeze.AdmNo = adm.AdmNo;
                freeze.State = true;

                //Message message = new Message();
                //message = db.Message.Where(m => m.MessageNo== msgid).FirstOrDefault();
                //message.State = false;

                //MsgReport msgReport = new MsgReport();
                //msgReport = db.MsgReport.Find(id);
                //msgReport.State = true;

                db.Freeze.Add(freeze); 
                user.State = true;
                db.SaveChanges();
            }
           
            return RedirectToAction("Index");
          
        }


        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MsgReport msgReport = db.MsgReport.Find(id);

            if (msgReport == null)
            {
                return HttpNotFound();
            }
            return View(msgReport);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //還沒好
        public ActionResult DeleteConfirmed(int id)
        {
            MsgReport msgreport = db.MsgReport.Find(id);
            msgreport.State = true;


            var a = msgreport.MessageNo;

            var messages = db.Message.Where(m => m.MessageNo == a).ToList();
            messages.ForEach(ms => ms.State = false);


            db.Entry(msgreport).State = EntityState.Modified;
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
