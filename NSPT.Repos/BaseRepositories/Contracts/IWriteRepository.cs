namespace Repos.BaseRepositories.Contracts;

public interface IWriteRepository<T> where T : class, IApiEntity
{
    void Add(T entity, bool saveNow = true);
    Task AddAsync(T entity, CancellationToken cancellationToken, bool saveNow = true);
    void AddRange(IEnumerable<T> entities, bool saveNow = true);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken, bool saveNow = true);
    void Delete(T entity, bool saveNow = true);
    Task DeleteAsync(T entity, CancellationToken cancellationToken, bool saveNow = true);
    Task HardDeleteAsync(T entity, CancellationToken cancellationToken, bool saveNow = true);
    void DeleteRange(IEnumerable<T> entities, bool saveNow = true);
    Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken, bool saveNow = true);
    void Update(T entity, bool saveNow = true);
    Task UpdateAsync(T entity, CancellationToken cancellationToken, bool saveNow = true);
    void UpdateRange(IEnumerable<T> entities, bool saveNow = true);
    Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken, bool saveNow = true);
    void Detach(T entity);
    void Attach(T entity);
}