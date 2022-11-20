namespace Repos.BaseRepositories;

public class RepositoryWrapper : IRepositoryWrapper, IScopedDependency
{
    private readonly ApplicationContext _context;
    private readonly ICurrentUserService _currentUserService;

    public RepositoryWrapper(
        ApplicationContext context,
        ICurrentUserService currentUserService
    )
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public IBaseRepository<T> SetRepository<T>() where T : ApiEntity
    {
        return new BaseRepository<T>(_context, _currentUserService);
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}