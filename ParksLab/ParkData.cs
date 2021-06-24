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
      
        public string ParkID { get; set; }
        public string Parkname { get; set; }
        public string SanctuaryName { get; set; }
        public string Borough { get; set; }
        public string Acres { get; set; }
        public string Directions { get; set; }
        public string Description { get; set; }
        public string HabitatType { get; set; }
    }
}
