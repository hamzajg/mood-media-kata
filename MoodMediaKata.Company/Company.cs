using System.Collections;
using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public sealed class Company : Entity
{
    public IEnumerable<Location> Locations { get; } = new List<Location>();
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

public class Device(string serialNumber, DeviceType type) : Entity
{ 
    public string SerialNumber { get; private set; } = serialNumber;
    public string Type { get; private set; } = type.ToString();

    public override long Id { get; protected set; } = IdGenerator.NextId(nameof(Device));
}

public class Location(string name, string address, Device device) : Entity
{
    public string Name { get; private set; } = name;
    public string Address { get; private set; } = address;
    public Device Device { get; private set; } = device;
    public override long Id { get; protected set; } = IdGenerator.NextId(nameof(Location));

}

public enum DeviceType
{
    Standard,
    Custom
}
public enum Licensing
{
    Standard
}