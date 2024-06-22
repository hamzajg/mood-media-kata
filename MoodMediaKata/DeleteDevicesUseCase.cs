namespace MoodMediaKata;

public class DeleteDevicesUseCase(IDeviceRepository deviceRepository)
{
    public void Execute(IEnumerable<string> devicesSerialNumbers)
    {
        foreach (var serialNumber in devicesSerialNumbers)
        {
            deviceRepository.DeleteDeviceBySerialNumber(serialNumber);
        }
    }
}