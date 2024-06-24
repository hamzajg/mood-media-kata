using MoodMediaKata.App;
using MoodMediaKata.Company;
using MoodMediaKata.Infra;

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
            new DeleteDevicesUseCase(_deviceRepository));
    }

    [Fact]
    public void CanProcessNewCompanyMessageType()
    {
        var message = new NewCompanyMessage
        {
            CompanyName = "My Company 1",
            CompanyCode = "COMP-123", Licensing = "Standard",
            Devices = new[]
            {
                new DeviceDto { OrderNo = "1", Type = "Standard" }, new DeviceDto { OrderNo = "2", Type = "Custom" }
            }
        };

        _sut.Process(message);

        Assert.NotEmpty(_companyRepository.FindAll());
        Assert.NotNull(_companyRepository.FindOneById(1));
        Assert.Equal(1, _companyRepository.FindOneById(1)?.Id);
        Assert.Equal("My Company 1", _companyRepository.FindOneById(1)?.Name);
        Assert.Equal("COMP-123", _companyRepository.FindOneById(1)?.Code);
        Assert.Equal("Standard", _companyRepository.FindOneById(1)?.Licensing);
        Assert.NotEmpty(_locationRepository.FindAll());
        Assert.NotNull(_locationRepository.FindOneById(1));
    }

    [Fact]
    public void CanProcessDeleteDevicesMessageType()
    {
        _sut.Process(new NewCompanyMessage
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

        _sut.Process(message);

        Assert.Empty(_deviceRepository.FindAll());
    }

    public void Dispose()
    {
        IdGenerator.Reset();
    }
}