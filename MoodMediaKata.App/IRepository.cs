namespace MoodMediaKata.App;

public interface IRepository<T> where T : Entity 
{
    Task<T> Save(T entity);
    IEnumerable<T> FindAll();
    T? FindOneById(long id);
}