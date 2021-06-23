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

            var queries = Request.Query["search"];
          
            if (queries.Count == 0)
            {
                ViewBag.Data = data;
            }
            else
            {
                
                foreach (ParkData park in data)
                {
                    bool hasSearchTerms = true;

                    foreach (string query in queries)
                    {
                        if (!park.Parkname.ToLower().Contains(query.ToLower()) && !park.Description.ToLower().Contains(query.ToLower()))
                        {
                            hasSearchTerms = false;
                            break;
                        }
                    }
                    if (hasSearchTerms)
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
