using System.Collections.Concurrent;

namespace MoodMediaKata;

public class InMemoryLocationRepository : ILocationRepository
{
    private readonly IDictionary<long, Location> _locations = new ConcurrentDictionary<long, Location>();

    public IEnumerable<Location> FindAll() => _locations.Values;

    public Location? FindOneById(long id) => _locations.ToDictionary().GetValueOrDefault(id);
    public Location Save(Location location)
    {
        _locations.Add(location.Id, location);
        return location;
    }
}