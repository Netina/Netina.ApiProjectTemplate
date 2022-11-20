namespace Repos.BaseRepositories.Contracts;

public interface IRepositoryWrapper : IDisposable
{
    IBaseRepository<T> SetRepository<T>() where T : ApiEntity;
}