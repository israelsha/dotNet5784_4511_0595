using DalApi;

namespace Dal;

sealed internal class DalList : IDal
{
   // public static IDal Instance { get; } = new DalList();

    private static Lazy<IDal> lazyInstance = new Lazy<IDal>(() => new DalList(), LazyThreadSafetyMode.ExecutionAndPublication); // Singleton with lazy initialization.                                                          
    public static IDal Instance => lazyInstance.Value; // call to create the instance. only now the instance is created, and not when the program is compiled.
    private DalList() { } // private CTOR for Singleton
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public IDependency Dependency => new DependencyImplementation();

    public IDates Dates => new DatesImplementation();
}