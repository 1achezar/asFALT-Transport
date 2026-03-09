using System;
using System.Collections.Generic;
using System.Text;
using Transport.Data.Models;

namespace Transport.Services.DTOs
{
    public class RouteSearchResult
    {
        public string BusLineNumber { get; set; }
        public int TravelMinutes { get; set; }
    }
}
