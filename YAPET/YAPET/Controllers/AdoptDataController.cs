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
    public class AdoptDataController : Controller
    {
        // GET: AdoptData
        public async Task<ActionResult> Index(string search, int page = 1)
        {
            string url = "https://data.coa.gov.tw/Service/OpenData/TransService.aspx?UnitId=QcbUEzN6E6DL";
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = Int32.MaxValue;
            var resp = await client.GetStringAsync(url);

            var collention = JsonConvert.DeserializeObject<IEnumerable<AdoptData>>(resp).Where(s => s.animal_status == "OPEN");

            if (!String.IsNullOrEmpty(search))
            {
                collention = collention.Where(s => s.animal_kind.Contains(search) || s.animal_colour.Contains(search) || s.animal_colour.Contains(search) || s.animal_place.Contains(search) || s.shelter_address.Contains(search));
            }

            int pageSize = 12;
            int currentPage = page < 1 ? 1 : page;

            var pageAdoptData = collention.ToPagedList(currentPage, pageSize);

            return View(pageAdoptData);
        }
    }
}