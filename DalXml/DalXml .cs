namespace Dal;
using DalApi;
using System.Diagnostics;

sealed internal class DalXml : IDal
{
    Lazy<DalXml> _dalxml = new Lazy<DalXml>();
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    public ITask Task => new TaskImplementation();

    public IEngineer Engineer => new EngineerImplementation();

    public IDependency Dependency => new DependencyImplementation();

}
