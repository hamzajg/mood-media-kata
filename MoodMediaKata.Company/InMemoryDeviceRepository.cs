using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public class InMemoryDeviceRepository : InMemoryRepository<Device>, IDeviceRepository
{
    public void DeleteDeviceBySerialNumber(string serialNumber)
    {
        Store.Remove(Store.SingleOrDefault(item => item.Value.SerialNumber == serialNumber).Key);
    }
}