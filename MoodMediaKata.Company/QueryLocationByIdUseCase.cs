using MoodMediaKata.App;

namespace MoodMediaKata.Company;

public class QueryLocationByIdUseCase(IRepository<Location> locationRepository)
{
    public async Task<Location?> Execute(long id) => await locationRepository.FindOneById(id);
}