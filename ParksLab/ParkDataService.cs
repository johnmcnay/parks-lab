using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using ParksLab.Controllers;

namespace ParksLab
{
    public class ParkDataService
    {
        private IMemoryCache _cache;

        public ParkDataService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public List<ParkData> CallApiWithCaching()
        {
            string json;
            List<ParkData> data;

            //from code sample in https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-5.0
            //Look for cache key.
            if (!_cache.TryGetValue(CacheKeys.Entry, out data))
            {
                // Key not in cache, so get data.
                json = new WebClient().DownloadString("https://seriouslyfundata.azurewebsites.net/api/parks");

                data = JsonConvert.DeserializeObject<List<ParkData>>(json);

                // Save data in cache and set the relative expiration time
                _cache.Set(CacheKeys.Entry, data, TimeSpan.FromMinutes(5));
            }

            return data;
        }

        internal List<ParkData> GetFilteredData(List<string> search)
        {
            List<ParkData> data = CallApiWithCaching();

            if (search.Count == 0)
            {
                return data;
            }
            else
            {
                return ParksMatchingQuery(data, search);
            }
        }

        public List<ParkData> ParksMatchingQuery(List<ParkData> data, List<string> queries)
        {
  
            if (queries[0] == null)
            {
                return new List<ParkData>();
            }

            string query = queries[0].Trim().ToLower();

            var results = from p in data
                    where p.Parkname.ToLower().Contains(query) || p.Description.ToLower().Contains(query)
                    select p;

            return results.ToList();
        }
    }
}