using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YAPET.Models;

namespace YAPET.Controllers
{
    public class GetImageController : Controller
    {
        private Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        [LogReporter(IsLog = false)]
        public FileContentResult GetImage(string id, string tableName)
        {
            byte[] photo = null;
            string ImageMimeType = "";
            switch (tableName)
            {
                case "User":
                    var User = db.User.Find(Convert.ToInt32(id));
                    photo = User.Herder;
                    ImageMimeType = User.ImageMimeType;
                    break;
                case "Adopt":
                    var Adopt = db.Adopt.Find(id);
                    photo = Adopt.Photo;
                    ImageMimeType = Adopt.ImageMimeType;
                    break;
                case "Lost":
                    var Lost = db.Lost.Find(id);
                    photo = Lost.Photo;
                    ImageMimeType = Lost.ImageMimeType;
                    break;
                case "Photo":
                    var Photo = db.Photo.Find(Convert.ToInt32(id));
                    photo = Photo.Photo1;
                    ImageMimeType = Photo.ImageMimeType;
                    break;
                case "Activity":
                    var Activity = db.Activity.Find(id);
                    photo = Activity.Photo;
                    ImageMimeType = Activity.ImageMimeType;
                    break;
                case "Introduction":
                    var Introduction = db.Introduction.Find(id);
                    photo = Introduction.Photo;
                    ImageMimeType = Introduction.ImageMimeType;
                    break;
                case "Knowledge":
                    var knowledge = db.Knowledge.Find(id);
                    photo = knowledge.Photo;
                    ImageMimeType = knowledge.ImageMimeType;
                    break;
            }

            return File(photo, ImageMimeType);

        }
    }
}