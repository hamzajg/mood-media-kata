namespace MoodMediaKata.App;

public abstract class Message
{
    public Guid Id = Guid.NewGuid();
    public abstract MessageType MessageType { get; set; }
}
public enum MessageType
{
    NewCompany,
    DeleteDevices
}