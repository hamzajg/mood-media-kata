using MoodMediaKata.App;
using MoodMediaKata.Company;

namespace MoodMediaKata.Tests.Unit;

[Collection("SequentialTests")]
public class CompanyTest : IDisposable
{
    [Fact]
    public void CanCreateNewCompanyWithUniqueId()
    {
        var company1 = new Company.Company("My Company 1", "COMP-123", Licensing.Standard, Array.Empty<Device>());
        var company2 =  new Company.Company("My Company 2", "COMP-456", Licensing.Standard, Array.Empty<Device>());
        
        Assert.NotEqual(company1.Id, company2.Id);
        Assert.Equal(1, company1.Id);
        Assert.Equal(2, company2.Id);
    }
    
    [Fact]
    public void CanCreateNewCompanyWithLocationsFromDevices()
    {
        var company = new Company.Company("My Company 1", "COMP-123", Licensing.Standard, 
                new []{new Device("1", DeviceType.Standard), new Device("2", DeviceType.Custom)});
        
        Assert.Equal(2, company.Locations.Count());
        Assert.Equal(1, company.Locations.ElementAt(0).Device.Id);
        Assert.Equal("1", company.Locations.ElementAt(0).Device.SerialNumber);
        Assert.Equal("Standard", company.Locations.ElementAt(0).Device.Type);
        Assert.Equal(2, company.Locations.ElementAt(1).Device.Id);
        Assert.Equal("2", company.Locations.ElementAt(1).Device.SerialNumber);
        Assert.Equal("Custom", company.Locations.ElementAt(1).Device.Type);
    }

    public void Dispose()
    {
        IdGenerator.Reset();
    }
}