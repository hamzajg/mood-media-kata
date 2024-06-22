namespace MoodMediaKata;

public abstract class Message
{
    public Guid Id = Guid.NewGuid();
    public abstract MessageType MessageType { get; set; }
}

public class NewCompanyMessage : Message
{
    public override MessageType MessageType { get; set; } = MessageType.NewCompany;
    public string CompanyName { get; set; }
    public string CompanyCode { get; set; }
    public string Licensing { get; set; }
    public DeviceDto[] Devices { get; set; }
}

public class DeleteDevicesMessage : Message
{
    public override MessageType MessageType { get; set; } = MessageType.DeleteDevices;
    public string[] SerialNumbers { get; set; }
}