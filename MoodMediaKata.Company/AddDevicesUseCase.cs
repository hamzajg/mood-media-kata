using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public class AddDevicesUseCase(IRepository<Location> locationRepository, IDeviceRepository deviceRepository)
{
    public IEnumerable<Location> Execute(Company company)
    {
        foreach (var location in company.Locations)
        {
            locationRepository.Save(location);
            deviceRepository.Save(location.Device);
        }

        return company.Locations;
    }
}