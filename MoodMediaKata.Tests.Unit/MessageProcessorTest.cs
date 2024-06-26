using MoodMediaKata.App;
using MoodMediaKata.Company;

namespace MoodMediaKata.Tests.Unit;

[Collection("SequentialTests")]
public class MessageProcessorTest : IDisposable
{
    private readonly IRepository<Company.Company> _companyRepository = new InMemoryRepository<Company.Company>();
    private readonly IRepository<Location> _locationRepository = new InMemoryRepository<Location>();
    private readonly IDeviceRepository _deviceRepository = new InMemoryDeviceRepository();
    private readonly MessageProcessor _sut;

    public MessageProcessorTest()
    {
        _sut = new MessageProcessor(new CreateCompanyUseCase(_companyRepository,
            new AddDevicesUseCase(_locationRepository, _deviceRepository)),
            new DeleteDevicesUseCase(_deviceRepository),
            new QueryCompanyByIdUseCase(_companyRepository));
    }

    [Fact]
    public async void CanProcessNewCompanyMessageType()
    {
        var message = new CreateNewCompanyMessage
        {
            CompanyName = "My Company 1",
            CompanyCode = "COMP-123", Licensing = "Standard",
            Devices = new[]
            {
                new DeviceDto { OrderNo = "1", Type = "Standard" }, new DeviceDto { OrderNo = "2", Type = "Custom" }
            }
        };

        await _sut.Process(message);

        Assert.NotEmpty(_companyRepository.FindAll());
        Assert.NotNull(await _companyRepository.FindOneById(1));
        Assert.Equal(1, (await _companyRepository.FindOneById(1))?.Id);
        Assert.Equal("My Company 1", (await _companyRepository.FindOneById(1))?.Name);
        Assert.Equal("COMP-123", (await _companyRepository.FindOneById(1))?.Code);
        Assert.Equal("Standard", (await _companyRepository.FindOneById(1))?.Licensing);
        Assert.NotEmpty(_locationRepository.FindAll());
        Assert.NotNull(await _locationRepository.FindOneById(1));
    }

    [Fact]
    public async void CanProcessDeleteDevicesMessageType()
    {
        _sut.Process(new CreateNewCompanyMessage
        {
            CompanyName = "My Company 1",
            CompanyCode = "COMP-123", Licensing = "Standard",
            Devices = new[]
            {
                new DeviceDto { SerialNumber = "Serial1", Type = "Standard" },
                new DeviceDto { SerialNumber = "Serial2", Type = "Custom" }
            }
        });

        var message = new DeleteDevicesMessage
        {
            SerialNumbers = new[]
            {
                "Serial1", "Serial2"
            }
        };

        await _sut.Process(message);

        Assert.Empty(_deviceRepository.FindAll());
    }

    public void Dispose()
    {
        IdGenerator.Reset();
    }
}