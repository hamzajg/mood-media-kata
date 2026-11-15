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
    QueryCompanyByIdResult,
    AddDevices,
    QueryLocationById,
    QueryLocationByIdResult
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
    /** Dedicated result queue — keeps responses off the request queue. */
    public const string Queue = "Q.MoodMediaKata.Results";

    public override MessageType MessageType { get; set; } = MessageType.QueryCompanyByIdResult;
    /** Correlates the result to the QueryCompanyByIdMessage.Id that produced it. */
    public Guid RequestId { get; set; }
    /** Null when the company does not exist. */
    public CompanyDto? Company { get; set; }
}

public class CompanyDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public string Licensing { get; set; }
}

public class AddDevicesMessage : Message
{
    public override MessageType MessageType { get; set; } = MessageType.AddDevices;
    public long CompanyId { get; set; }
    public DeviceDto[] Devices { get; set; }
}

public class QueryLocationByIdMessage : Message
{
    public override MessageType MessageType { get; set; } = MessageType.QueryLocationById;
    public long LocationId { get; set; }
}

public class QueryLocationByIdResultMessage : Message
{
    /** Dedicated result queue — keeps responses off the request queue. */
    public const string Queue = "Q.MoodMediaKata.LocationResults";

    public override MessageType MessageType { get; set; } = MessageType.QueryLocationByIdResult;
    /** Correlates the result to the QueryLocationByIdMessage.Id that produced it. */
    public Guid RequestId { get; set; }
    /** Null when the location does not exist. */
    public LocationDto? Location { get; set; }
}

public class LocationDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public long CompanyId { get; set; }
    public string DeviceSerialNumber { get; set; }
    public string DeviceType { get; set; }
}
