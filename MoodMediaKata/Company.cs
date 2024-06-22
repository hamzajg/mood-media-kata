using System.Collections;

namespace MoodMediaKata;

public class Company
{
    public IEnumerable<Location> Locations { get; private set; } = new List<Location>();
    public long Id { get; private set; }
    public string Name { get; private set; }
    public string Code { get; private set; }
    public string Licensing { get; private set; }

    public Company(string name, string code, Licensing licensing, IEnumerable<Device> devices)
    {
        Id = IdGenerator.NextId(nameof(Company));
        Name = name;
        Code = code;
        Licensing = licensing.ToString();
        SetupLocationsAndDevicesFrom(devices);
    }

    private void SetupLocationsAndDevicesFrom(IEnumerable<Device> devices)
    {
        var index = 1;
        foreach (var device in devices)
        {
            ((IList) Locations).Add(new Location($"Location {index++}", "", device));
        }
    }
}