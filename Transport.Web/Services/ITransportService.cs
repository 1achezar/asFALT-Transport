using System;
using System.Collections.Generic;
using System.Text;
using Transport.Web.Models;
using Transport.Services.DTOs;

namespace Transport.Services
{
    public interface ITransportService
    {
        // Returns all bus lines that stop at the given stop name
        IEnumerable<BusLine> GetBusLineForStop(string stopName);

        // Returns all stops for a given line number, in order
        IEnumerable<BusStop> GetStopsForLine(string lineNumber);

        // Finds all bus lines that go directly from one stop to another
        IEnumerable<RouteSearchResult> FindRoutes(string fromStop, string toStop);

        // Returns the next departures from a stop towards a destination, starting from a given time
        IEnumerable<DepartureInfo> GetNextDepartures(string fromStop, string toStop, TimeSpan time, int count);
        
        // Returns all stops
        IEnumerable<string> GetAllStops();
    }
}
