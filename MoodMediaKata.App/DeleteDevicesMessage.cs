namespace MoodMediaKata.App;

public class DeleteDevicesMessage : Message
{
    public override MessageType MessageType { get; set; } = MessageType.DeleteDevices;
    public string[] SerialNumbers { get; set; }
}