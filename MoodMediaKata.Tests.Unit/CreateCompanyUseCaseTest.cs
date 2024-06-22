namespace MoodMediaKata.Tests.Unit;

[Collection("SequentialTests")]
public class CreateCompanyUseCaseTest : IDisposable
{
    private readonly ILocationRepository _locationRepository = new InMemoryLocationRepository();
    private readonly IDeviceRepository _deviceRepository = new InMemoryDeviceRepository();
    private readonly CreateCompanyUseCase _sut;

    public CreateCompanyUseCaseTest()
    {
        _sut = new CreateCompanyUseCase(new InMemoryCompanyRepository(), new AddDevicesUseCase(_locationRepository, _deviceRepository));
    }

    [Fact]
    public void CanCreateNewCompanyWithLocationsAndDevices()
    {
        var result = _sut.Execute("My Company 1", "COMP-123", Licensing.Standard,
            new[] { new Device("1", DeviceType.Standard), new Device("2", DeviceType.Custom) });

        Assert.Equal(1, result.Id);
        Assert.Equal("My Company 1", result.Name);
        Assert.Equal("COMP-123", result.Code);
        Assert.Equal("Standard", result.Licensing);
        Assert.NotEmpty(result.Locations);
        Assert.Equal(1, result.Locations.ElementAt(0).Id);
        Assert.Equal("Location 1", result.Locations.ElementAt(0).Name);
        Assert.Equal(2, result.Locations.ElementAt(1).Id);
        Assert.Equal("Location 2", result.Locations.ElementAt(1).Name);
    }

    [Fact]
    public void CanCreateNewDevices()
    {
        _sut.Execute("My Company 1", "COMP-123", Licensing.Standard,
            new[] { new Device("1", DeviceType.Standard), new Device("2", DeviceType.Custom) });

        var result = _deviceRepository.FindAll();

        Assert.NotEmpty(result);
        Assert.Equal(1, result.ElementAt(0).Id);
        Assert.Equal("1", result.ElementAt(0).SerialNumber);
        Assert.Equal("Standard", result.ElementAt(0).Type);
        Assert.Equal(2, result.ElementAt(1).Id);
        Assert.Equal("2", result.ElementAt(1).SerialNumber);
        Assert.Equal("Custom", result.ElementAt(1).Type);
    }

    [Fact]
    public void CanCreateNewLocationsFromDevices()
    {
        _sut.Execute("My Company 1", "COMP-123", Licensing.Standard,
            new[] { new Device("1", DeviceType.Standard), new Device("2", DeviceType.Custom) });

        var result = _locationRepository.FindAll();

        Assert.NotEmpty(result);
        Assert.Equal(1, result.ElementAt(0).Id);
        Assert.Equal("Location 1", result.ElementAt(0).Name);
        Assert.Equal(2, result.ElementAt(1).Id);
        Assert.Equal("Location 2", result.ElementAt(1).Name);
    }

    public void Dispose()
    {
        IdGenerator.Reset();
    }
}