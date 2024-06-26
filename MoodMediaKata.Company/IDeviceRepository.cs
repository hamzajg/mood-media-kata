using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public interface IDeviceRepository : IRepository<Device>
{
    Task DeleteDeviceBySerialNumber(string serialNumber);
}