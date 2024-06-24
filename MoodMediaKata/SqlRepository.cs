using MoodMediaKata.App;

namespace MoodMediaKata;

public class SqlRepository<T> : IRepository<T> where T : Entity
{
    public T Save(T entity)
    {
        throw new NotImplementedException();
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