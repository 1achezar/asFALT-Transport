using System;
using System.Collections.Generic;
using System.Text;

namespace Transport.Web.Models
{
    // Defines a timetable entry for a bus line
    // Specifies when a bus departs and in which direction
    public class Schedule
    {
        public int Id { get; set; }
        public TimeSpan DepartureTime { get; set; }

        public int BusLineId { get; set; }
        public BusLine BusLine { get; set; }
        public RouteDirection Direction { get; set; }
    }
}
