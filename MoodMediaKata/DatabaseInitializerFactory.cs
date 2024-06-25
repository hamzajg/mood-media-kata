using Microsoft.Extensions.DependencyInjection;

namespace MoodMediaKata;

public static class DatabaseInitializerFactory
{
    public static IDatabaseInitializer InitializeDatabase(string databaseOrmType, IServiceCollection services)
    {
        return databaseOrmType switch
        {
            "dapper" => new DatabaseDapperInitializer(services),
            "ef" => new DatabaseEfInitializer(services),
            _ => throw new ArgumentException("Invalid repository type", nameof(databaseOrmType))
        };
    }
}


public interface IDatabaseInitializer
{
    void InitializeDatabase();
}