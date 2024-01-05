

using System.Reflection.Emit;
using System.Xml.Linq;

namespace DO;

public record Dependency
{
   int Id;
   int? DependentTask = null;
    int? DependsOnTask = null;
    public Dependency() { Id = 0;}

    public Dependency(int id, int? dependentTask, int? dependsOnTask)
    {
        Id = id;
        DependentTask = dependentTask;
        DependsOnTask = dependsOnTask;
    }
}



