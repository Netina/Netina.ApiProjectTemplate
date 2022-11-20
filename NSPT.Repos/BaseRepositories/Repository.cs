namespace Repos.BaseRepositories;

public class Repository<T> : IRepository<T> where T : class, IApiEntity
{
    protected readonly ICurrentUserService _currentUserService;
    protected readonly ApplicationContext DbContext;

    public Repository(ApplicationContext dbContext, ICurrentUserService currentUserService)
    {
        DbContext = dbContext;
        _currentUserService = currentUserService;
        Entities = DbContext.Set<T>();
        DbContext.ChangeTracker.Clear();
    }

    public DbSet<T> Entities { get; }

    public virtual IQueryable<T> Table => Entities.Where(e=>!e.IsRemoved);

    public virtual IQueryable<T> TableNoTracking => Table.AsNoTracking();
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        DbContext?.Dispose();
    }
}