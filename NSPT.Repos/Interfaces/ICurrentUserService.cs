namespace Repos.Interfaces;
public interface ICurrentUserService : IScopedDependency
{
    string UserId { get; }
    string UserName { get; }
    int MedicalCenterId { get; }
}
