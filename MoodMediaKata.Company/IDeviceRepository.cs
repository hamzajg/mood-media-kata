using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public interface IDeviceRepository : IRepository<Device>
{
    void DeleteDeviceBySerialNumber(string serialNumber);
}