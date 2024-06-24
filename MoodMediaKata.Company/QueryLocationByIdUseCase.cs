using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public class QueryLocationByIdUseCase(IRepository<Location> locationRepository)
{
    public Location? Execute(long id) => locationRepository.FindOneById(id);
}