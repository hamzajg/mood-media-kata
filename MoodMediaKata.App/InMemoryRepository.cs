namespace MoodMediaKata.App;

using System.Collections.Concurrent;

public class InMemoryRepository<T> : IRepository<T> where T : Entity
{
    protected readonly IDictionary<long, T> Store = new ConcurrentDictionary<long, T>();

    public Task<T> Save(T entity)
    {
        Store.Add(entity.Id, entity);
        return Task.FromResult(entity);
    }

    public IEnumerable<T> FindAll() => Store.Values;

    public T? FindOneById(long id) => Store.ToDictionary().GetValueOrDefault(id);
}