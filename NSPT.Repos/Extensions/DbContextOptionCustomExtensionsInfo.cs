namespace Repos.Extensions;

public class DbContextOptionCustomExtensionsInfo : DbContextOptionsExtensionInfo
{
    public DbContextOptionCustomExtensionsInfo(IDbContextOptionsExtension extension) : base(extension)
    {
    }

    public override bool IsDatabaseProvider { get; }
    public override string LogFragment { get; }

    public override int GetServiceProviderHashCode()
    {
        return Extension.GetHashCode();
    }

    public override void PopulateDebugInfo(IDictionary<string, string> debugInfo)
    {
    }

    public override bool ShouldUseSameServiceProvider(DbContextOptionsExtensionInfo other)
    {
        return true;
    }
}

public class DbContextOptionCustomExtensions : IDbContextOptionsExtension
{
    public DbContextOptionCustomExtensions()
    {
        Info = new DbContextOptionCustomExtensionsInfo(this);
    }

    public Assembly ProjectAssembly { get; set; }

    public void ApplyServices(IServiceCollection services)
    {
    }

    public void Validate(IDbContextOptions options)
    {
    }

    public DbContextOptionsExtensionInfo Info { get; }
}

public static class ApplicationContextExtensions
{
    public static DbContextOptionsBuilder UseProjectAssembly(this DbContextOptionsBuilder contextOptions,
        Assembly projectAssembly)
    {
        var extension = new DbContextOptionCustomExtensions
        {
            ProjectAssembly = projectAssembly
        };
        ((IDbContextOptionsBuilderInfrastructure)contextOptions).AddOrUpdateExtension(extension);
        return contextOptions;
    }
}
