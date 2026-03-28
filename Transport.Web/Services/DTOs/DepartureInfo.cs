using System;
using System.Collections.Generic;
using System.Text;

namespace Transport.Services.DTOs
{
    // Holds basic departure data for a bus line
    public class DepartureInfo
    {
        public string BusLineNumber { get; set; }
        public TimeSpan DepartureFromStop { get; set; }
    }
}
