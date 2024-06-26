using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace MoodMediaKata;

public static class DatabaseInitializerFactory
{
    public static IDatabaseInitializer InitializeDatabase(string databaseOrmType, IServiceCollection services)
    {
        switch (databaseOrmType)
        {
            case "dapper":
            {
                var dbConnection =
                    new NpgsqlConnection("Host=localhost;Username=postgres;Password=V3ryS3cr3t;Database=kata_db;");
                services.AddSingleton<IDbConnection>(_ => dbConnection);
                return new DatabaseDapperInitializer(dbConnection);
            }
            case "ef":
                var optionsBuilder = new DbContextOptionsBuilder<KataDbContext>();
                optionsBuilder.UseSqlServer("Server=localhost;Database=kata_db;User Id=sa;Password=V3ryS3cr3t;");

                var dbContext = new KataDbContext(optionsBuilder.Options);
                services.AddDbContext<KataDbContext>();
                dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(20));
                return new DatabaseEfInitializer(dbContext);
            default:
                throw new ArgumentException("Invalid repository type", nameof(databaseOrmType));
        }
    }
}


public interface IDatabaseInitializer
{
    void InitializeDatabase();
}