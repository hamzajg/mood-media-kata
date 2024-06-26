using System.Data;
using Microsoft.Extensions.DependencyInjection;
using MoodMediaKata.App;
using Npgsql;

namespace MoodMediaKata;

public static class RepositoryFactory
{
    public static IRepository<T> CreateRepository<T>(string repositoryType, IServiceProvider provider) where T : Entity
    {
        return repositoryType switch
        {
            "InMemory" => new InMemoryRepository<T>(),
            "PostgreSql" => new PostgreSqlRepository<T>(provider.GetRequiredService<IDbConnection>()),
            "Sql" => new SqlRepository<T>(provider.GetRequiredService<KataDbContext>()),
            "MongoDb" => new MongoDbRepository<T>(),
            _ => throw new ArgumentException("Invalid repository type", nameof(repositoryType))
        };
    }
}