using System;
using System.Collections.Generic;
using System.Text;

namespace Transport.Data.Models
{
    public class RouteStop
    {
        public int Id { get; set; }

        public int BusLineId { get; set; }
        public BusLine BusLine { get; set; }

        public int BusStopId { get; set; }
        public BusStop BusStop { get; set; }

        public int StopOrder { get; set; } 
        public int MinutesFromStart { get; set; }
        public RouteDirection Direction { get; set; }
    }
}
