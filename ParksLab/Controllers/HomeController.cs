using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParksLab.Models;

namespace ParksLab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ParkData _parkData;
        

        public HomeController(ILogger<HomeController> logger, ParkData parkData)
        {
            _logger = logger;
            _parkData = parkData;
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
        public IActionResult Parks(List<string> search)
        {
            List<ParkData> data = _parkData.CallApiWithCaching();
            
            if (search.Count == 0)
            {
                ViewBag.Data = data;
            }
            else
            {
                ViewBag.Data = _parkData.ParksMatchingQuery(data, search);
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
