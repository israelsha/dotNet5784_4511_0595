namespace Dal;
using DalApi;




public interface IDal
{
    ITask Task { get; }
    IEngineer Engineer { get; }
    IDependency Dependency { get; }
}
