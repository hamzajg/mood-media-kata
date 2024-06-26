using System.Collections;
using System.Text.Json.Serialization;
using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public sealed class Company : Entity
{
    private string _licencing;
    public IEnumerable<Location> Locations { get; } = new List<Location>();
    public string Name { get; private set; }
    public string Code { get; private set; }

    public string Licensing
    {
        get => _licencing;
        private set => _licencing = int.TryParse(value, out var intValue) ? ((Licensing)intValue).ToString() : value;
    }

    public Company()
    {
        Id = IdGenerator.NextId(nameof(Company));
    }

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
            var location = new Location($"Location {index++}", "", device, this);
            ((IList)Locations).Add(location);
            device.AddLocation(location);
        }
    }
}

public class Device : Entity
{
    public Device()
    {
    }

    public Device(string serialNumber, DeviceType type, string orderNumber = "")
    {
        SerialNumber = string.IsNullOrEmpty(serialNumber) ? DeviceSerialNumberGenerator.Next(Id) : serialNumber;
        OrderNumber = orderNumber;
        Type = type.ToString();
    }

    public string SerialNumber { get; private set; }
    public string OrderNumber { get; private set; }
    public string Type { get; private set; }

    public override long Id { get; protected set; } = IdGenerator.NextId(nameof(Device));
    public Location Location { get; private set; }

    public void AddLocation(Location location)
    {
        Location = location;
    }
}


public class Location : Entity
{
    public Location()
    {
    }

    public Location(string name, string address, Device device, Company company)
    {
        Name = name;
        Address = address;
        Device = device;
        Company = company;
    }

    public string Name { get; private set; }
    public string Address { get; private set; }
    [JsonIgnore] public Device Device { get; private set; }
    public override long Id { get; protected set; } = IdGenerator.NextId(nameof(Location));
    [JsonIgnore] public Company Company { get; private set; }
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