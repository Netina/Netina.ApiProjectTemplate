namespace Repos.BaseRepositories.Contracts;

internal interface IRepository<T> where T : class, IApiEntity
{
    DbSet<T> Entities { get; }
    IQueryable<T> Table { get; }
    IQueryable<T> TableNoTracking { get; }
}