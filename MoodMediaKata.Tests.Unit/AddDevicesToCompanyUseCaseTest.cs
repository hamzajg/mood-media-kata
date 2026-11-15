using MoodMediaKata.App;
using MoodMediaKata.Company;

namespace MoodMediaKata.Tests.Unit;

[Collection("SequentialTests")]
public class AddDevicesToCompanyUseCaseTest : IDisposable
{
    private readonly InMemoryRepository<Company.Company> _companyRepository = new();
    private readonly InMemoryRepository<Location> _locationRepository = new();
    private readonly InMemoryDeviceRepository _deviceRepository = new();
    private readonly AddDevicesToCompanyUseCase _sut;

    public AddDevicesToCompanyUseCaseTest()
    {
        _sut = new AddDevicesToCompanyUseCase(_companyRepository, _locationRepository, _deviceRepository);
    }

    // covers scenario: Add devices to a company
    [Fact]
    public async void AddsDevicesWithLocationsNumberedAfterExistingOnes()
    {
        var company = await _companyRepository.Save(new Company.Company("Acme", "ACM-1", Licensing.Standard,
            new[] { new Device("S1", DeviceType.Standard), new Device("S2", DeviceType.Custom) }));
        await new AddDevicesUseCase(_locationRepository, _deviceRepository).Execute(company);

        var added = await _sut.Execute(company.Id, new[]
        {
            new DeviceDto { OrderNo = "3", Type = "Standard" },
            new DeviceDto { OrderNo = "4", Type = "Custom", SerialNumber = "S4" }
        });

        var locations = added.ToList();
        Assert.Equal(2, locations.Count);
        Assert.Equal("Location 3", locations[0].Name);
        Assert.Equal("Location 4", locations[1].Name);
        Assert.False(string.IsNullOrEmpty(locations[0].Device.SerialNumber));
        Assert.Equal("S4", locations[1].Device.SerialNumber);
        Assert.Equal("Custom", locations[1].Device.Type);
        Assert.Equal(4, company.Locations.Count());
    }

    // covers scenario: Add devices to unknown company fails
    [Fact]
    public async void ThrowsWhenCompanyDoesNotExist()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.Execute(999, new[] { new DeviceDto { Type = "Standard" } }));
        Assert.Empty(_deviceRepository.FindAll());
    }

    public void Dispose()
    {
        IdGenerator.Reset();
    }
}
