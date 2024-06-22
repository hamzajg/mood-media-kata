using System.Collections;

namespace MoodMediaKata;

public interface ILocationRepository
{
    IEnumerable<Location> FindAll();
    Location? FindOneById(long i);
    Location Save(Location location);
}