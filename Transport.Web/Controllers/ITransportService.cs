using System;
using System.Collections.Generic;
using System.Text;
using Transport.Data.Models;
using Transport.Services.DTOs;

namespace Transport.Services
{
    public interface ITransportService
    {
        IEnumerable<BusLine> GetBusLineForStop(string stopName);
        IEnumerable<BusStop> GetStopsForLine(string lineNumber);
        IEnumerable<RouteSearchResult> FindRoutes(string fromStop, string toStop);
        IEnumerable<DepartureInfo> GetNextDepartures(string fromStop, string toStop, TimeSpan time, int count);
    }
}
