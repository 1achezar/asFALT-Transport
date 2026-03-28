using Transport.Web.Data;
using Transport.Web.Models;
using Transport.Services.DTOs;
using RouteDirection = Transport.Web.Models.RouteDirection;

namespace Transport.Services
{
    public class TransportService : ITransportService
    {
        private readonly TransportDbContext _context;

        public TransportService(TransportDbContext context)
        {
            _context = context;
        }

        // Returns all bus lines that pass through the given stop (no duplicates)
        public IEnumerable<BusLine> GetBusLineForStop(string stopName)
        {
            List<BusLine> result = new List<BusLine>();

            foreach (RouteStop rs in _context.RouteStops.ToList())
            {
                if (rs.BusStop.Name == stopName && !result.Contains(rs.BusLine))
                    result.Add(rs.BusLine);
            }

            return result;
        }

        // Returns the stops for a line in the order the bus visits them
        public IEnumerable<BusStop> GetStopsForLine(string lineNumber)
        {
            List<RouteStop> stops = new List<RouteStop>();

            foreach (RouteStop rs in _context.RouteStops.ToList())
            {
                if (rs.BusLine.Number == lineNumber)
                    stops.Add(rs);
            }

            // Sort by stop order so they come out in the correct sequence
            stops.Sort((a, b) => a.StopOrder.CompareTo(b.StopOrder));

            List<BusStop> result = new List<BusStop>();
            foreach (RouteStop rs in stops)
                result.Add(rs.BusStop);

            return result;
        }

        // Finds all direct routes from one stop to another
        public IEnumerable<RouteSearchResult> FindRoutes(string fromStop, string toStop)
        {
            List<RouteStop> fromEntries = new List<RouteStop>();
            List<RouteStop> toEntries = new List<RouteStop>();

            foreach (RouteStop rs in _context.RouteStops.ToList())
            {
                if (rs.BusStop.Name == fromStop) fromEntries.Add(rs);
                if (rs.BusStop.Name == toStop) toEntries.Add(rs);
            }

            List<RouteSearchResult> result = new List<RouteSearchResult>();

            foreach (RouteStop from in fromEntries)
            {
                foreach (RouteStop to in toEntries)
                {
                    // Same line, same direction, and fromStop comes before toStop
                    if (from.BusLineId == to.BusLineId && from.Direction == to.Direction && from.StopOrder < to.StopOrder)
                    {
                        result.Add(new RouteSearchResult
                        {
                            BusLineNumber = from.BusLine.Number,
                            TravelMinutes = to.MinutesFromStart - from.MinutesFromStart,
                            Direction = from.Direction
                        });
                    }
                }
            }

            return result;
        }

        // Returns how many minutes after the first stop the bus reaches this stop
        private int GetStopOffset(string lineNumber, string stopName, RouteDirection direction)
        {
            foreach (RouteStop rs in _context.RouteStops.ToList())
            {
                if (rs.BusLine.Number == lineNumber && rs.BusStop.Name == stopName && rs.Direction == direction)
                    return rs.MinutesFromStart;
            }

            return 0;
        }

        // Returns all scheduled departure times for a line in a given direction
        private List<TimeSpan> GetSchedulesForLine(string lineNumber, RouteDirection direction)
        {
            List<TimeSpan> result = new List<TimeSpan>();

            foreach (Schedule s in _context.Schedules.ToList())
            {
                if (s.BusLine.Number == lineNumber && s.Direction == direction)
                    result.Add(s.DepartureTime);
            }

            return result;
        }

        // Returns the next N departures from a stop towards a destination, after a given time
        public IEnumerable<DepartureInfo> GetNextDepartures(string fromStop, string toStop, TimeSpan time, int count)
        {
            List<DepartureInfo> result = new List<DepartureInfo>();

            foreach (RouteSearchResult route in FindRoutes(fromStop, toStop))
            {
                // How many minutes after the line's start time the bus reaches fromStop
                int offset = GetStopOffset(route.BusLineNumber, fromStop, route.Direction);

                foreach (TimeSpan departure in GetSchedulesForLine(route.BusLineNumber, route.Direction))
                {
                    // Add the offset to get the actual arrival time at fromStop
                    TimeSpan actualTime = departure + TimeSpan.FromMinutes(offset);

                    if (actualTime >= time)
                    {
                        result.Add(new DepartureInfo
                        {
                            BusLineNumber = route.BusLineNumber,
                            DepartureFromStop = actualTime
                        });
                    }
                }
            }

            result.Sort((a, b) => a.DepartureFromStop.CompareTo(b.DepartureFromStop));

            return result.Take(count);
        }

        public IEnumerable<string> GetAllStops()
        {
            return _context.BusStops.Select(s => s.Name).ToList();
        }
    }
}
