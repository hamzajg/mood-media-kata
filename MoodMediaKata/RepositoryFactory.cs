using MoodMediaKata.App;

namespace MoodMediaKata;

public static class RepositoryFactory
{
    public static IRepository<T> CreateRepository<T>(string repositoryType) where T : Entity
    {
        return repositoryType switch
        {
            "InMemory" => new InMemoryRepository<T>(),
            "Sql" => new SqlRepository<T>(),
            "MongoDb" => new MongoDbRepository<T>(),
            _ => throw new ArgumentException("Invalid repository type", nameof(repositoryType))
        };
    }
}