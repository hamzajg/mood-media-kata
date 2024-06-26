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
        if (entity is Company.Company)
        {
            var sql =
                "INSERT INTO kata_schema.company (id, name, code, licensing) VALUES (@Id, @Name, @Code, @Licensing)";
            var company = entity as Company.Company;
            await dbConnection.ExecuteAsync(sql, new
            {
                company.Id, company.Name, company.Code,
                Licensing = (int)Enum.Parse<Licensing>(company.Licensing)
            });
            return entity;
        } else if (entity is Location)
        {
            var sql =
                "INSERT INTO kata_schema.location (id, name, address, parentId) VALUES (@Id, @Name, @Address, @ParentId)";
            var location = entity as Location;
            await dbConnection.ExecuteAsync(sql, new
            {
                location.Id, location.Name, location.Address, ParentId = location.Company.Id
            });

            return entity;
        } else if (entity is Device)
        {
            var sql =
                "INSERT INTO kata_schema.device (id, serialnumber, type, locationId) VALUES (@Id, @SerialNumber, @Type, LocationId)";
            var device = entity as Device;
            await dbConnection.ExecuteAsync(sql, new
            {
                device.Id, device.SerialNumber, Type = (int)Enum.Parse<DeviceType>(device.Type), LocationId = device.Location.Id
            });
            return entity;
        }

        throw new ArgumentException($"Unsupported Entity Type {entity}");
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