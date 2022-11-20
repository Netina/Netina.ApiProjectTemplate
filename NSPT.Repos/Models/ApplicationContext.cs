namespace Repos.Models;

public class ApplicationContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly Assembly _projectAssembly;

    public ApplicationContext(DbContextOptions<ApplicationContext> options, ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
        _projectAssembly = options.GetExtension<DbContextOptionCustomExtensions>().ProjectAssembly;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entitiesAssembly = _projectAssembly;
        modelBuilder.RegisterAllEntities<ApiEntity>(entitiesAssembly);
        modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
        modelBuilder.AddRestrictDeleteBehaviorConvention();
        modelBuilder.AddSequentialGuidForIdConvention();
        modelBuilder.AddPluralizingTableNameConvention();
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<IApiEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserName;
                    entry.Entity.CreatedAt = DateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedBy = _currentUserService.UserName;
                    entry.Entity.ModifiedAt = DateTime.Now;
                    break;
            }

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        foreach (var entry in ChangeTracker.Entries<IApiEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserName;
                    entry.Entity.CreatedAt = DateTime.Now;
                    break;

                case EntityState.Modified:
                    entry.Entity.ModifiedBy = _currentUserService.UserName;
                    entry.Entity.ModifiedAt = DateTime.Now;
                    break;
            }

        return base.SaveChangesAsync(cancellationToken);
    }
}
