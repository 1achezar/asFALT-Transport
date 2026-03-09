using System;
using System.Collections.Generic;
using System.Text;

namespace Transport.Data.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public TimeSpan DepartureTime { get; set; }

        public int BusLineId { get; set; }
        public BusLine BusLine { get; set; }
    }
}
