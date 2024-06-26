using System.Data;
using Microsoft.Extensions.DependencyInjection;
using MoodMediaKata.App;
using MoodMediaKata.Company;

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
    
    public static IDeviceRepository CreateRepository<T, TE>(string repositoryType, IServiceProvider provider) where TE : Entity where T : IRepository<TE>
    {
        return repositoryType switch
        {
            "InMemory" => new InMemoryDeviceRepository(),
            "PostgreSql" => new PostgreSqlDeviceRepository(provider.GetRequiredService<IDbConnection>()),
            "Sql" => new SqlDeviceRepository(provider.GetRequiredService<KataDbContext>()),
            "MongoDb" => new MongoDeviceDbRepository(),
            _ => throw new ArgumentException("Invalid repository type", nameof(repositoryType))
        };
    }
}

public class MongoDeviceDbRepository : MongoDbRepository<Device>, IDeviceRepository
{
    public void DeleteDeviceBySerialNumber(string serialNumber)
    {
        throw new NotImplementedException();
    }
}

public class SqlDeviceRepository(KataDbContext context) : SqlRepository<Device>(context), IDeviceRepository
{

    public void DeleteDeviceBySerialNumber(string serialNumber)
    {
        throw new NotImplementedException();
    }
}

public class PostgreSqlDeviceRepository(IDbConnection dbConnection) : PostgreSqlRepository<Device>(dbConnection), IDeviceRepository
{
    public void DeleteDeviceBySerialNumber(string serialNumber)
    {
        throw new NotImplementedException();
    }
}