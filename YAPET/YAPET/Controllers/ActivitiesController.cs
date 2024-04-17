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
    public class ActivitiesController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        // Activity
        public ActionResult ActivityIndex()
        {
            var activity = db.Activity.OrderBy(n => n.Deadline).ToList();

            return View(activity);
        }

        public ActionResult ActivityDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = db.Activity.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }
        public ActionResult findall()
        {
            return Json(db.Activity.AsEnumerable().Select(e => new
            {

                title = e.Title,
                Date = e.Date.ToString("yyyy-MM-dd"),
                Content = e.Content,
                Deadline = e.Deadline.ToString("yyyy-MM-dd")

            }).ToList(), JsonRequestBehavior.AllowGet);

        }

        // Introduction
        public ActionResult IntroductionIndex(int page = 1)
        {
            var introduction = db.Introduction.OrderByDescending(n => n.Date).ToList();

            return View(introduction);
        }

        public ActionResult IntroductionDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Introduction introduction = db.Introduction.Find(id);
            if (introduction == null)
            {
                return HttpNotFound();
            }
            return View(introduction);
        }

        // Knowledge
        public ActionResult KnowledgeIndex(int page = 1)
        {
            var knowledge = db.Knowledge.OrderByDescending(n => n.Date).ToList();

            return View(knowledge);
        }

        public ActionResult KnowledgeDetails(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Knowledge knowledge = db.Knowledge.Find(id);
            if (knowledge == null)
            {
                return HttpNotFound();
            }
            return View(knowledge);
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
