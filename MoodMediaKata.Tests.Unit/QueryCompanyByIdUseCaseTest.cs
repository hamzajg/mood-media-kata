using MoodMediaKata.App;
using MoodMediaKata.Company;

namespace MoodMediaKata.Tests.Unit;

[Collection("SequentialTests")]
public class QueryCompanyByIdUseCaseTest : IDisposable
{
    private readonly IRepository<Company.Company> _companyRepository = new InMemoryRepository<Company.Company>();
    private readonly IRepository<Location> _locationRepository = new InMemoryRepository<Location>();
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