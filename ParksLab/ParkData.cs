using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ParksLab.Controllers;

namespace ParksLab
{
    public class ParkData
    {
        private IMemoryCache _cache;

        public string ParkID { get; set; }
        public string Parkname { get; set; }
        public string SanctuaryName { get; set; }
        public string Borough { get; set; }
        public string Acres { get; set; }
        public string Directions { get; set; }
        public string Description { get; set; }
        public string HabitatType { get; set; }

        public ParkData(IMemoryCache cache)
        {
            _cache = cache;
        }

        public List<ParkData> CallApiWithCaching()
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

        public List<ParkData> ParksMatchingQuery(List<ParkData> data, List<string> queries)
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

    }
}
