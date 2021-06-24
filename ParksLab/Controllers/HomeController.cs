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
        private ParkDataService _parkDataService;
        
        public HomeController(ILogger<HomeController> logger, ParkDataService parkDataService)
        {
            _logger = logger;
            _parkDataService = parkDataService;
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

            ViewBag.Data = _parkDataService.GetFilteredData(search);

            return View();
        }

        [Route("javascriptparks")]
        public IActionResult JavascriptParks()
        {

            return View();
        }

        [Route("park")]
        public JsonResult Park(List<string> filter) 
        {
            List<ParkData> data = _parkDataService.GetFilteredData(filter);

            return Json(data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
