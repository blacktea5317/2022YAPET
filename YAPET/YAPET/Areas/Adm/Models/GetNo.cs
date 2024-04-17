using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YAPET.Models;

namespace YAPET.Areas.Adm.Models
{
    public class GetNo
    {
        Hw_MyPetsEntities db = new Hw_MyPetsEntities();

        private string GetActivityNo()
        {
            var activity = db.Activity.OrderByDescending(n => n.ActivityNo).FirstOrDefault();
            if (activity != null)
                return activity.ActivityNo;
            else
                return "";
        }

        private string GetIntroductionNo()
        {
            var a = db.Introduction.OrderByDescending(n => n.IntroductionNo).FirstOrDefault();
            if (a != null)
                return a.IntroductionNo;
            else
                return "";
        }

        private string GetKnowledgeNo()
        {
            var k = db.Knowledge.OrderByDescending(n => n.KnowledgeNo).FirstOrDefault();
            if (k != null)
                return k.KnowledgeNo;
            else
                return "";
        }

        private string GetFreezeNo()
        {
            var f = db.Freeze.OrderByDescending(n => n.FreezeNo).FirstOrDefault();
            if (f != null)
                return f.FreezeNo;
            else
                return "";
        }

        public string GetNO(string dbname)
        {
            string no = "";
            string temp = "";
            switch (dbname)
            {
                case "Activity":
                    no = GetActivityNo();
                    temp = "ACTI";
                    break;
                case "Introduction":
                    no = GetIntroductionNo();
                    temp = "INFO";
                    break;
                case "Knowledge":
                    no = GetKnowledgeNo();
                    temp = "KNOW";
                    break;
                case "Freeze":
                    no = GetFreezeNo();
                    temp = "FREZ";
                    break;
            }

            if (string.IsNullOrEmpty(no))
            {
                no = temp + "000001";
            }
            else
            {
                no = no.Replace(temp, "");
                no = string.Format(temp + "{0:000000}", Convert.ToInt32(no) + 1);
            }
            return no;
        }
     
    }
}