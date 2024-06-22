using System.Collections;

namespace MoodMediaKata;

public interface IDeviceRepository
{
    Device Save(Device device);
    void DeleteDeviceBySerialNumber(string serialNumber);
    Device? FindOneById(long id);
    IEnumerable<Device> FindAll();
}