using System;
using System.Collections.Generic;
using System.Data;
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
                .Join(toEntries, from => new { from.BusLineId , from.Direction}, to => new { to.BusLineId, to.Direction }, (from, to) => new { from, to })
                .Where(pair => pair.from.StopOrder < pair.to.StopOrder)
                .Select(pair => new RouteSearchResult
                {
                    BusLineNumber = pair.from.BusLine.Number,
                    TravelMinutes = pair.to.MinutesFromStart - pair.from.MinutesFromStart,
                    Direction = pair.from.Direction
                })
                .ToList();
        }

        private int GetStopOffset(string lineNumber  , string stopName , RouteDirection direction)
        {
            return _context.RouteStops
                .Where(rs => rs.BusLine.Number == lineNumber && rs.BusStop.Name == stopName && rs.Direction == direction)
                .Select(rs => rs.MinutesFromStart)
                .First();
        }
        
        private IEnumerable<TimeSpan> GetSchedulesForLine(string lineNumber , RouteDirection direction)
        {
            return _context.Schedules
                    .Where(s => s.BusLine.Number == lineNumber && s.Direction == direction)
                    .Select(s => s.DepartureTime)
                    .ToList();
        }

        private List<DepartureInfo>  FilterDepartures(IEnumerable<TimeSpan> schedules , int offset , string lineNumber , TimeSpan time)
        {
            var results = new List<DepartureInfo>();
            foreach (var departure in schedules)
            {
                var actualTime = departure + TimeSpan.FromMinutes(offset);
                if (actualTime >= time)
                {
                    results.Add(new DepartureInfo
                    {
                        BusLineNumber = lineNumber,
                        DepartureFromStop = actualTime
                    });
                }
            }

            return results;
        }
        public IEnumerable<DepartureInfo> GetNextDepartures(string fromStop, string toStop, TimeSpan time, int count)
        {
            var routes = FindRoutes(fromStop, toStop);
            var results = new List<DepartureInfo>();

            foreach (var route in routes)
            {
                var offset = GetStopOffset(route.BusLineNumber, fromStop , route.Direction);
                var schedules = GetSchedulesForLine(route.BusLineNumber, route.Direction);
                results.AddRange(FilterDepartures(schedules, offset, route.BusLineNumber, time));
            }

            return results.OrderBy(d => d.DepartureFromStop).Take(count);
        }
    }
}
