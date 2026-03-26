using System;
using System.Collections.Generic;
using System.Text;
using Transport.Web.Models;
using RouteDirection = Transport.Web.Models.RouteDirection;

namespace Transport.Services.DTOs
{
    public class RouteSearchResult
    {
        public string BusLineNumber { get; set; }
        public int TravelMinutes { get; set; }
        public RouteDirection Direction { get; set; }
    }
}
