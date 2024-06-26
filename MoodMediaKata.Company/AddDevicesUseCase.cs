using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public class AddDevicesUseCase(IRepository<Location> locationRepository, IDeviceRepository deviceRepository)
{
    public async Task<IEnumerable<Location>> Execute(Company company)
    {
        foreach (var location in company.Locations)
        {
            await locationRepository.Save(location);
            await deviceRepository.Save(location.Device);
        }

        return company.Locations;
    }
}