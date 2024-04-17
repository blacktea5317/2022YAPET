using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using YAPET.Models;
using PagedList;
using PagedList.Mvc;

namespace YAPET.Controllers
{
    public class LPetAPIController : Controller
    {
        // GET: user/LPetAPI
        public async Task<ActionResult> Index(int page = 1)
        {
            string url = "https://data.coa.gov.tw/Service/OpenData/TransService.aspx?UnitId=IFJomqVzyB0i";
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = Int32.MaxValue;
            var resp = await client.GetStringAsync(url);

            var collection = JsonConvert.DeserializeObject<IEnumerable<LPetAPI>>(resp);


            int pageSize = 9;
            int currentPage = page < 1 ? 1 : page;

            var pagecollection = collection.ToPagedList(currentPage, pageSize);

            return View(pagecollection);

        }
    }
}