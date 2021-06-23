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
        private IMemoryCache _cache;

        public HomeController(ILogger<HomeController> logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public List<ParkData> CallApi()
        {
            string json;
            
            //from code sample in https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-5.0
            //Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.Entry, out json))
            {
                // Key not in cache, so get data.
                json = new WebClient().DownloadString("https://seriouslyfundata.azurewebsites.net/api/parks");

                // Save data in cache and set the relative expiration time
                _cache.Set(CacheKeys.Entry, json, TimeSpan.FromMinutes(5));
            }

            return JsonConvert.DeserializeObject<List<ParkData>>(json);
        }

        [Route("parkdata")]
        public IActionResult Parks(List<string> search)
        {
            List<ParkData> data = CallApi();
            
            if (search.Count == 0)
            {
                ViewBag.Data = data;
            }
            else
            {
                ViewBag.Data = ParksMatchingQuery(data, search);
            }

            return View();
        }

        private List<ParkData> ParksMatchingQuery(List<ParkData> data, List<string> queries)
        {
            List<ParkData> results = new List<ParkData>();

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
            return results;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
