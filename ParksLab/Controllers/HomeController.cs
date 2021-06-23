using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParksLab.Models;

namespace ParksLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("parkdata")]
        public IActionResult Parks()
        {

            var json = new WebClient().DownloadString("https://seriouslyfundata.azurewebsites.net/api/parks");
            List<ParkData> data = JsonConvert.DeserializeObject<List<ParkData>>(json);
            List<ParkData> results = new List<ParkData>();
            string search = null;

            if (Request.Query.Count > 0)
            {
                foreach (var query in Request.Query)
                {
                    if (query.Key.ToLower() == "search")
                    {
                        search = query.Value;
                        break;
                    }
                }
            }

            if (search == null)
            {
                ViewBag.Data = data;
            } else
            {
                foreach (var park in data)
                {
                    if (park.Parkname.ToLower().Contains(search.ToLower()) || park.Description.ToLower().Contains(search.ToLower()))
                    {
                        results.Add(park);
                    }
                }

                ViewBag.Data = results;
            }

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
