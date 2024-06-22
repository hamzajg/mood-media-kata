namespace MoodMediaKata;

public class Device(string serialNumber, DeviceType type)
{
    public string SerialNumber { get; private set; } = serialNumber;
    public string Type { get; private set; } = type.ToString();
    public long Id { get; private set; } = IdGenerator.NextId(nameof(Device));
}