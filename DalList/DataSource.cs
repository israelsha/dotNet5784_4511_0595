
namespace Dal;

/// <summary>
/// The database of the data layer
/// </summary>
internal static class DataSource   
{
    internal static class Config        //class for run numbers
    {
        internal const int startTaskId = 100;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        internal const int startDependencyId = 100;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
    }

    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.Dependency> Dependencies { get; } = new();


}
