using MoodMediaKata.App;

namespace MoodMediaKata.Repository;

public class MongoDbRepository<T> : IRepository<T> where T : Entity
{
    public Task<T> Save(T entity)
    {
        throw new NotImplementedException();
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