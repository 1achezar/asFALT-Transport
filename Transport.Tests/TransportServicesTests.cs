using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using Transport.Services;
using Transport.Web.Data;
using Transport.Web.Models;
using RouteDirection = Transport.Web.Models.RouteDirection;

namespace Transport.Tests
{
    public class TransportServiceTests
    {
        // Creates an in-memory test database with sample data 
        // This method simulates a real transport system environment so unit tests can run
        // without using the actual database. Each call creates a fresh database instance
        // to ensure tests are isolated and do not affect each other.
        private TransportDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<TransportDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TransportDbContext(options);

            var busLine = new BusLine { Id = 1, Number = "10" };
            var stop1 = new BusStop { Id = 1, Name = "StopA" };
            var stop2 = new BusStop { Id = 2, Name = "StopB" };

            context.BusLines.Add(busLine);
            context.BusStops.AddRange(stop1, stop2);

            context.RouteStops.AddRange(
                new RouteStop
                {
                    BusLine = busLine,
                    BusStop = stop1,
                    StopOrder = 1,
                    MinutesFromStart = 0,
                    Direction = RouteDirection.FromSarafovo
                },
                new RouteStop
                {
                    BusLine = busLine,
                    BusStop = stop2,
                    StopOrder = 2,
                    MinutesFromStart = 10,
                    Direction = RouteDirection.FromSarafovo
                }
            );

            context.Schedules.Add(new Schedule
            {
                BusLine = busLine,
                Direction = RouteDirection.FromSarafovo,
                DepartureTime = new TimeSpan(8, 0, 0)
            });

            context.SaveChanges();
            return context;
        }

        // Returns time
        [Test]
        public void GetNextDepartures_ShouldReturnTime()
        {
            var context = GetDbContext();
            var service = new TransportService(context);

            var result = service.GetNextDepartures("StopA", "StopB", new TimeSpan(7, 50, 0), 5);

            Assert.That(result, Is.Not.Empty);
            Assert.That(result.First().DepartureFromStop, Is.EqualTo(new TimeSpan(8, 0, 0)));
        }

        // Iterates through list
        [Test]
        public void GetNextDepartures_ShouldIterateSchedules()
        {
            var context = GetDbContext();

            context.Schedules.Add(new Schedule
            {
                BusLineId = 1,
                Direction = RouteDirection.FromSarafovo,
                DepartureTime = new TimeSpan(9, 0, 0)
            });

            context.SaveChanges();

            var service = new TransportService(context);

            var result = service.GetNextDepartures("StopA", "StopB", new TimeSpan(7, 0, 0), 10);

            Assert.That(result.Count() >= 2);
        }

        // Start and end stop selection
        [Test]
        public void FindRoutes_ShouldMatchStops()
        {
            var context = GetDbContext();
            var service = new TransportService(context);

            var routes = service.FindRoutes("StopA", "StopB");

            Assert.That(routes, Is.Not.Empty);
            Assert.That(routes.First().BusLineNumber, Is.EqualTo("10"));
        }
    }
}