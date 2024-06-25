using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MoodMediaKata.App;
using Npgsql;

namespace MoodMediaKata;

public class PostgreSqlRepository<T>(IDbConnection dbConnection) : IRepository<T>
    where T : Entity
{        
    public async Task<T> Save(T entity)
    {
        var sql = "INSERT INTO kata_schema.company (id, name, code, licensing) VALUES (@Id, @Name, @Code, @Licensing)";
        var company = entity as Company.Company;
        await using var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=V3ryS3cr3t;Database=kata_db;");
        Console.WriteLine(dbConnection.State);
        Console.WriteLine(connection);
        await connection.OpenAsync();
        var result = await connection.ExecuteAsync(sql, new {Id = company.Id, Name= company.Name, Code = company.Code, Licensing = 0});
        Console.WriteLine(result);
        return entity;
    }

    public IEnumerable<T> FindAll()
    {
        throw new NotImplementedException();
    }

    public T? FindOneById(long id)
    {
        throw new NotImplementedException();
    }
}