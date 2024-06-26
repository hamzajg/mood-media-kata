namespace MoodMediaKata.App;

public abstract class Message
{
    public Guid Id = Guid.NewGuid();
    public abstract MessageType MessageType { get; set; }
}
public enum MessageType
{
    NewCompany,
    DeleteDevices,
    QueryCompanyById,
    QueryCompanyByIdResult
}

public class DeleteDevicesMessage : Message
{
    public override MessageType MessageType { get; set; } = MessageType.DeleteDevices;
    public string[] SerialNumbers { get; set; }
}

public class CreateNewCompanyMessage : Message
{
    public override MessageType MessageType { get; set; } = MessageType.NewCompany;
    public string CompanyName { get; set; }
    public string CompanyCode { get; set; }
    public string Licensing { get; set; }
    public DeviceDto[] Devices { get; set; }
}
public record DeviceDto
{
    public string OrderNo { get; set; }
    public string SerialNumber { get; set; }
    public string Type { get; set; }
}

public class QueryCompanyByIdMessage : Message
{
    public override MessageType MessageType { get; set; } = MessageType.QueryCompanyById;
    public long CompanyId { get; set; }
}
public class QueryCompanyByIdResultMessage : Message
{
    public override MessageType MessageType { get; set; } = MessageType.QueryCompanyByIdResult;
    public CompanyDto Company { get; set; }
}

public class CompanyDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
}
