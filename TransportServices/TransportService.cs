using System;
using System.Collections.Generic;
using System.Text;
using Transport.Data;
using Transport.Data.Models;
using Transport.Services.DTOs;

namespace Transport.Services
{
    public class TransportService: ITransportService
    {
        private readonly TransportDbContext _context;

        public TransportService(TransportDbContext context)
        {
            _context = context;

        }

        public IEnumerable<BusLine> GetBusLineForStop(string stopName)
        {
            return _context.RouteStops
                .Where(rs => rs.BusStop.Name == stopName)
                .Select(rs => rs.BusLine)
                .Distinct()
                .ToList();
        }

        public IEnumerable<BusStop> GetStopsForLine(string lineNumber)
        {
            return _context.RouteStops
                .Where(rs => rs.BusLine.Number == lineNumber)
                .OrderBy(rs => rs.StopOrder)
                .Select(rs => rs.BusStop)
                .ToList();
        }

        public IEnumerable<RouteSearchResult> FindRoutes(string fromStop, string toStop)
        {
            var fromEntries = _context.RouteStops.Where(rs => rs.BusStop.Name == fromStop);
            var toEntries = _context.RouteStops.Where(rs => rs.BusStop.Name == toStop);

            return fromEntries
                .Join(toEntries, from => from.BusLineId, to => to.BusLineId, (from, to) => new { from, to })
                .Where(pair => pair.from.StopOrder < pair.to.StopOrder)
                .Select(pair => new RouteSearchResult
                {
                    BusLineNumber = pair.from.BusLine.Number,
                    TravelMinutes = pair.to.MinutesFromStart - pair.from.MinutesFromStart

                })
                .ToList();
        }

        public IEnumerable<DepartureInfo> GetNextDepartures(string fromStop, string toStop, TimeSpan time, int count)
        {
            var routes = FindRoutes(fromStop, toStop);
            var results = new List<DepartureInfo>();

            foreach (var route in routes)
            {
                var offset = _context.RouteStops
                    .Where(rs => rs.BusLine.Number == route.BusLineNumber && rs.BusStop.Name == fromStop)
                    .Select(rs => rs.MinutesFromStart)
                    .First();

                var schedules = _context.Schedules
                    .Where(s => s.BusLine.Number == route.BusLineNumber)
                    .Select(s => s.DepartureTime)
                    .ToList();

                foreach (var departure in schedules)
                {
                    var actualTime = departure + TimeSpan.FromMinutes(offset);
                    if (actualTime >= time)
                    {
                        results.Add(new DepartureInfo
                        {
                            BusLineNumber = route.BusLineNumber,
                            DepartureFromStop = actualTime
                        });
                    }
                }
            }

            return results.OrderBy(d => d.DepartureFromStop).Take(count);
        }
    }
}
