using System.Collections;
using MoodMediaKata.App;

namespace MoodMediaKata.Company;

/// <summary>
/// Appends devices to an existing company. Each device gets its own
/// Location named "Location {index}", numbered after the company's
/// current locations (Location N+1, N+2, ...).
/// </summary>
public class AddDevicesToCompanyUseCase(IRepository<Company> companyRepository,
    IRepository<Location> locationRepository, IDeviceRepository deviceRepository)
{
    public async Task<IEnumerable<Location>> Execute(long companyId, IEnumerable<DeviceDto> devices)
    {
        var company = await companyRepository.FindOneById(companyId)
                      ?? throw new KeyNotFoundException($"Company {companyId} was not found.");

        var added = new List<Location>();
        var index = company.Locations.Count() + 1;
        foreach (var dto in devices)
        {
            var device = new Device(dto.SerialNumber,
                (DeviceType)Enum.Parse(typeof(DeviceType), dto.Type), dto.OrderNo);
            var location = new Location($"Location {index++}", "", device, company);
            device.AddLocation(location);
            ((IList)company.Locations).Add(location);
            await locationRepository.Save(location);
            await deviceRepository.Save(device);
            added.Add(location);
        }

        return added;
    }
}
