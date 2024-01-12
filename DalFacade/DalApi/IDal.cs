namespace Dal;
using DalApi;
sealed public class DalList : IDal
{
    public ITask Task => throw new NotImplementedException();

    public IEngineer Engineer => throw new NotImplementedException();

    public IDependency dependency => throw new NotImplementedException();
}

public interface IDal
{
    ITask Task { get; } 
    IEngineer Engineer { get; }
    IDependency dependency { get; }
}
