namespace Repos.BaseRepositories.Contracts;

public interface IBaseRepository<T> : IDisposable, IReadRepository<T>, IWriteRepository<T> where T : class, IApiEntity
{
}