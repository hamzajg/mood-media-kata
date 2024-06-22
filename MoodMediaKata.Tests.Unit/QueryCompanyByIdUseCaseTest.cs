namespace MoodMediaKata.Tests.Unit;

[Collection("SequentialTests")]
public class QueryCompanyByIdUseCaseTest : IDisposable
{
    private readonly ICompanyRepository _companyRepository = new InMemoryCompanyRepository();
    private readonly ILocationRepository _locationRepository = new InMemoryLocationRepository();
    private readonly IDeviceRepository _deviceRepository = new InMemoryDeviceRepository();
    private readonly CreateCompanyUseCase _createCompanyUseCase;
    private readonly QueryCompanyByIdUseCase _sut;

    public QueryCompanyByIdUseCaseTest()
    {
        _createCompanyUseCase = new CreateCompanyUseCase(_companyRepository, new AddDevicesUseCase(_locationRepository, _deviceRepository));
        _sut = new QueryCompanyByIdUseCase(_companyRepository);
    }

    [Fact]
    public void CanQueryCreatedCompanyById()
    {
        _createCompanyUseCase.Execute("My Company 1", "COMP-123", Licensing.Standard,
            new[] { new Device("1", DeviceType.Standard), new Device("2", DeviceType.Custom) });

        var result = _sut.Execute(1);
        
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("My Company 1", result.Name);
    }

    public void Dispose()
    {
        IdGenerator.Reset();
    }
}