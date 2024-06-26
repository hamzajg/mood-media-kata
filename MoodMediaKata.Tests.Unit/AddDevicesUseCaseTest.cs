using MoodMediaKata.App;
using MoodMediaKata.Company;

namespace MoodMediaKata.Tests.Unit;

[Collection("SequentialTests")]
public class AddDevicesUseCaseTest : IDisposable
{
    private readonly AddDevicesUseCase _sut = new (new InMemoryRepository<Location>(), new InMemoryDeviceRepository());
    
    [Fact]
    public async void CanAddDevicesToCompany()
    {
        var result = await _sut.Execute(new Company.Company("My Company 1", "COMP-123", Licensing.Standard,
            new[] { new Device("1", DeviceType.Standard), new Device("2", DeviceType.Custom) }));
        
        Assert.NotEmpty(result);
        Assert.Equal(1, result.ElementAt(0).Id);
        Assert.Equal("Location 1", result.ElementAt(0).Name);
        Assert.Equal(1, result.ElementAt(0).Device.Id);
        Assert.Equal("1", result.ElementAt(0).Device.SerialNumber);
        Assert.Equal("Standard", result.ElementAt(0).Device.Type);
        Assert.Equal(2, result.ElementAt(1).Id);
        Assert.Equal("Location 2", result.ElementAt(1).Name);
        Assert.Equal(2, result.ElementAt(1).Device.Id);
        Assert.Equal("2", result.ElementAt(1).Device.SerialNumber);
        Assert.Equal("Custom", result.ElementAt(1).Device.Type);
    }
    
    public void Dispose()
    {
        IdGenerator.Reset();
    }
}