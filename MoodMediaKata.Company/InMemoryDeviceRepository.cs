using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public class InMemoryDeviceRepository : InMemoryRepository<Device>, IDeviceRepository
{
    public Task DeleteDeviceBySerialNumber(string serialNumber)
    {
        Store.Remove(Store.SingleOrDefault(item => item.Value.SerialNumber == serialNumber).Key);
        return Task.CompletedTask;
    }
}