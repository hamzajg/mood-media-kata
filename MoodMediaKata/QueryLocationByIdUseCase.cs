namespace MoodMediaKata;

public class QueryLocationByIdUseCase(ILocationRepository locationRepository)
{
    public Location? Execute(long id) => locationRepository.FindOneById(id);
}