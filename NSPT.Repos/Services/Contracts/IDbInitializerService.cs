
namespace Repos.Services.Contracts;

public interface IDbInitializerService : IScopedDependency
{
    void Initialize();
    Task SeedDate(bool force = false);
    Task SeedRoles();
}
