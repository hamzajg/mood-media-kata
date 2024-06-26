using MoodMediaKata.App;

namespace MoodMediaKata;

public class SqlRepository<T>(KataDbContext context) : IRepository<T>
    where T : Entity
{
    public async Task<T> Save(T entity)
    {
        context.Companies.Add(entity as Company.Company);
        await context.SaveChangesAsync();
        return entity;
    }

    public IEnumerable<T> FindAll()
    {
        throw new NotImplementedException();
    }

    public Task<T?> FindOneById(long id)
    {
        throw new NotImplementedException();
    }
}