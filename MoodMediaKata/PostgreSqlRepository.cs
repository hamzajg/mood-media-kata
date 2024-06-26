using System.Data;
using Dapper;
using MoodMediaKata.App;
using MoodMediaKata.Company;

namespace MoodMediaKata;

public class PostgreSqlRepository<T>(IDbConnection dbConnection) : IRepository<T>
    where T : Entity
{
    public async Task<T> Save(T entity)
    {
        switch (entity)
        {
            case Company.Company company:
            {
                var sql =
                    "INSERT INTO kata_schema.company (id, name, code, licensing) VALUES (@Id, @Name, @Code, @Licensing)";
                await dbConnection.ExecuteAsync(sql, new
                {
                    company.Id, company.Name, company.Code,
                    Licensing = (int)Enum.Parse<Licensing>(company.Licensing)
                });
                return entity;
            }
            case Location location:
            {
                var sql =
                    "INSERT INTO kata_schema.location (id, name, address, parentId) VALUES (@Id, @Name, @Address, @ParentId)";
                await dbConnection.ExecuteAsync(sql, new
                {
                    location.Id, location.Name, location.Address, ParentId = location.Company.Id
                });

                return entity;
            }
            case Device device:
            {
                var sql =
                    "INSERT INTO kata_schema.device (id, serialnumber, type, locationId) VALUES (@Id, @SerialNumber, @Type, @LocationId)";
                await dbConnection.ExecuteAsync(sql, new
                {
                    device.Id, device.SerialNumber, Type = (int)Enum.Parse<DeviceType>(device.Type), LocationId = device.Location.Id
                });
                return entity;
            }
            default:
                throw new ArgumentException($"Unsupported Entity Type {entity}");
        }
    }

    public IEnumerable<T> FindAll()
    {
        throw new NotImplementedException();
    }

    public async Task<T?> FindOneById(long id)
    {
        var sql = $"SELECT * FROM kata_schema.{typeof(T).Name} WHERE id = {id}";
        var result = await dbConnection.QuerySingleOrDefaultAsync<T>(sql);
        return result;
    }
}