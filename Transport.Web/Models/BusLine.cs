using System;
using System.Collections.Generic;
using System.Text;

namespace Transport.Web.Models
{
    public class BusLine
    {
        public int Id { get; set; }
        public string Number { get; set; }

        public ICollection<RouteStop> RouteStops { get; set; } = new List<RouteStop>();
        public ICollection<Schedule> Departures { get; set; } = new List<Schedule>();
    }
}
