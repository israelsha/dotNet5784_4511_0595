

namespace DO;

public record Dependency
{
    int Id;                     // increment
    int? DependentTask = null;  // ref: > DOT.Id
    int? DependsOnTask = null;  // ref: > DOT.Id

    public Dependency() { Id = 0; }
    public Dependency(int id, int dependentTask, int dependsOnTask)
    {
        Id = id;
        DependentTask = dependentTask;
        DependsOnTask = dependsOnTask;
    }
}
