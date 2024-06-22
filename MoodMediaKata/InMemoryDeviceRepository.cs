using System.Collections.Concurrent;

namespace MoodMediaKata;

public class InMemoryDeviceRepository : IDeviceRepository
{
    private readonly IDictionary<long, Device> _devices = new ConcurrentDictionary<long, Device>();
    
    public Device Save(Device device)
    {
        _devices.Add(device.Id, device);
        return device;
    }
    
    public void DeleteDeviceBySerialNumber(string serialNumber)
    {
        _devices.Remove(_devices.SingleOrDefault(item => item.Value.SerialNumber == serialNumber).Key);
    }

    public Device? FindOneById(long id)
    {
        return _devices.ToDictionary().GetValueOrDefault(id);
    }

    public IEnumerable<Device> FindAll() => _devices.Values;
}