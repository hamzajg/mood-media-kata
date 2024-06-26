namespace MoodMediaKata.Company;

public class DeleteDevicesUseCase(IDeviceRepository deviceRepository)
{
    public async Task Execute(IEnumerable<string> devicesSerialNumbers)
    {
        foreach (var serialNumber in devicesSerialNumbers)
        {
            await deviceRepository.DeleteDeviceBySerialNumber(serialNumber);
        }
    }
}