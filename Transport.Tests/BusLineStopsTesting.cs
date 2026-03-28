using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Transport.Services;
using Transport.Web.Data;
using Transport.Web.Models;

namespace Transport.Tests
{
    public class BusLineStopsTesting
    {
        // Test: creates BusLine
        [Test]
        public void Should_Create_BusLine()
        {
            var options = new DbContextOptionsBuilder<TransportDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TransportDbContext(options);

            var busLine = new BusLine { Id = 1, Number = "10" };

            context.BusLines.Add(busLine);
            context.SaveChanges();

            Assert.That(context.BusLines.Count(), Is.EqualTo(1));
            Assert.That(context.BusLines.First().Number, Is.EqualTo("10"));
        }

        // Test: creates BusStops
        [Test]
        public void Should_Create_BusStops()
        {
            var options = new DbContextOptionsBuilder<TransportDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TransportDbContext(options);

            var stop1 = new BusStop { Id = 1, Name = "StopA" };
            var stop2 = new BusStop { Id = 2, Name = "StopB" };

            context.BusStops.AddRange(stop1, stop2);
            context.SaveChanges();

            Assert.That(context.BusStops.Count(), Is.EqualTo(2));
            Assert.That(context.BusStops.Any(s => s.Name == "StopA"), Is.True);
            Assert.That(context.BusStops.Any(s => s.Name == "StopB"), Is.True);
        }
    }
}
