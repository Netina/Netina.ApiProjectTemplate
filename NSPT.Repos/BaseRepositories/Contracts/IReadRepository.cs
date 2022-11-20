namespace Repos.BaseRepositories.Contracts;

public interface IReadRepository<T> where T : class, IApiEntity
{
    DbSet<T> Entities { get; }
    IQueryable<T> Table { get; }
    IQueryable<T> TableNoTracking { get; }
    T GetById(params object[] ids);
    ValueTask<T> GetByIdAsync(CancellationToken cancellationToken, params object[] ids);

    void LoadCollection<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> collectionProperty)
        where TProperty : class;

    Task LoadCollectionAsync<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> collectionProperty,
        CancellationToken cancellationToken) where TProperty : class;

    void LoadReference<TProperty>(T entity, Expression<Func<T, TProperty>> referenceProperty) where TProperty : class;

    Task LoadReferenceAsync<TProperty>(T entity, Expression<Func<T, TProperty>> referenceProperty,
        CancellationToken cancellationToken) where TProperty : class;
}