using MoodMediaKata.App;
using MoodMediaKata.Company;

namespace MoodMediaKata.Tests.Unit;

[Collection("SequentialTests")]
public class QueryLocationByIdUseCaseTest : IDisposable
{
    private readonly IRepository<Location> _locationRepository = new InMemoryRepository<Location>();
    private readonly IDeviceRepository _deviceRepository = new InMemoryDeviceRepository();
    private readonly CreateCompanyUseCase _createCompanyUseCase;
    private readonly QueryLocationByIdUseCase _sut;

    public QueryLocationByIdUseCaseTest()
    {
        _createCompanyUseCase = new CreateCompanyUseCase(new InMemoryRepository<Company.Company>(),
            new AddDevicesUseCase(_locationRepository, _deviceRepository));
        _sut = new QueryLocationByIdUseCase(_locationRepository);
    }

    [Fact]
    public async void CanQueryCreatedLocationById()
    {
        await _createCompanyUseCase.Execute("My Company 1", "COMP-123", Licensing.Standard,
            new[] { new Device("1", DeviceType.Standard), new Device("2", DeviceType.Custom) });

        var result = await _sut.Execute(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Location 1", result.Name);
    }

    public void Dispose()
    {
        IdGenerator.Reset();
    }
}