using MoodMediaKata.App;

namespace MoodMediaKata.Infra;

public class RepositoryFactory
{
    public static IRepository<Entity> CreateRepository(string repositoryType)
    {
        switch (repositoryType)
        {
            case "InMemory":
                return new InMemoryRepository<Entity>();
            case "Sql":
                return new SqlRepository<Entity>();
            case "MongoDb":
                return new MongoDbRepository<Entity>();
            default:
                throw new ArgumentException("Invalid repository type", nameof(repositoryType));
        }
    }
}