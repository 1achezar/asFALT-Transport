namespace Transport.Web.Models
{
    // Represents a bus stop and its associated routes
    public class BusStop
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<RouteStop> RouteStops { get; set; } = new List<RouteStop>();
    }
}
