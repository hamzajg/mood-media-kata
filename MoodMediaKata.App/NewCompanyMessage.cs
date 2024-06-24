namespace MoodMediaKata.App;

public class NewCompanyMessage : Message
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