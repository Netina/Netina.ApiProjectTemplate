namespace Repos.BaseRepositories;

public class WriteRepository<T> : Repository<T>, IDisposable, IWriteRepository<T> where T : class, IApiEntity
{
    public WriteRepository(
        ApplicationContext dbContext,
        ICurrentUserService currentUserService) : base(dbContext, currentUserService)
    {
    }

    public void Dispose()
    {
        DbContext?.Dispose();
    }

    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken, bool saveNow = true)
    {
        AssertExtensions.NotNull(entity, nameof(entity));
        entity.CreatedBy = _currentUserService.UserName;
        entity.CreatedAt = DateTime.Now;
        await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken,
        bool saveNow = true)
    {
        AssertExtensions.NotNull(entities, nameof(entities));
        await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken cancellationToken, bool saveNow = true)
    {
        AssertExtensions.NotNull(entity, nameof(entity));
        var entry = await TableNoTracking.FirstOrDefaultAsync(t => t.Equals(entity));
        if (entry != null)
        {
            entity.CreatedBy = entry.CreatedBy;
            entity.CreatedAt = entry.CreatedAt;
        }

        entry.ModifiedAt = DateTime.Now;
        entry.ModifiedBy = _currentUserService.UserName;
        DbContext.Update(entity);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken,
        bool saveNow = true)
    {
        AssertExtensions.NotNull(entities, nameof(entities));
        Entities.UpdateRange(entities);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteAsync(T entity, CancellationToken cancellationToken, bool saveNow = true)
    {
        AssertExtensions.NotNull(entity, nameof(entity));
        entity.IsRemoved = true;
        entity.RemovedAt = DateTime.Now;
        entity.RemovedBy = _currentUserService.UserName;
        Entities.Update(entity);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task HardDeleteAsync(T entity, CancellationToken cancellationToken, bool saveNow = true)
    {
        AssertExtensions.NotNull(entity, nameof(entity));
        Entities.Remove(entity);
        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken,
        bool saveNow = true)
    {
        AssertExtensions.NotNull(entities, nameof(entities));
        foreach (var apiEntity in entities)
        {
            apiEntity.IsRemoved = true;
            apiEntity.RemovedAt = DateTime.Now;
            apiEntity.RemovedBy = _currentUserService.UserName;
            Entities.Update(apiEntity);
        }

        if (saveNow)
            await DbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual void Add(T entity, bool saveNow = true)
    {
        AssertExtensions.NotNull(entity, nameof(entity));
        Entities.Add(entity);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void AddRange(IEnumerable<T> entities, bool saveNow = true)
    {
        AssertExtensions.NotNull(entities, nameof(entities));
        Entities.AddRange(entities);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void Update(T entity, bool saveNow = true)
    {
        AssertExtensions.NotNull(entity, nameof(entity));
        Detach(entity);
        Entities.Update(entity);
        DbContext.SaveChanges();
    }

    public virtual void UpdateRange(IEnumerable<T> entities, bool saveNow = true)
    {
        AssertExtensions.NotNull(entities, nameof(entities));
        Entities.UpdateRange(entities);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void Delete(T entity, bool saveNow = true)
    {
        AssertExtensions.NotNull(entity, nameof(entity));
        Entities.Remove(entity);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void DeleteRange(IEnumerable<T> entities, bool saveNow = true)
    {
        AssertExtensions.NotNull(entities, nameof(entities));
        Entities.RemoveRange(entities);
        if (saveNow)
            DbContext.SaveChanges();
    }

    public virtual void Detach(T entity)
    {
        AssertExtensions.NotNull(entity, nameof(entity));
        var entry = DbContext.Entry(entity);
        if (entry != null)
            entry.State = EntityState.Detached;
    }

    public virtual void Attach(T entity)
    {
        AssertExtensions.NotNull(entity, nameof(entity));
        if (DbContext.Entry(entity).State == EntityState.Detached)
            Entities.Attach(entity);
    }
}