
using System.Reflection.Emit;
using System.Xml.Linq;

namespace DO;

/// <param name="Id"></param>
/// <param name="DependentTask"></param>
/// <param name="DependsOnTask"></param>
public record Dependency
    (
    int Id,
    int? DependentTask = null,
    int? DependsOnTask = null
    )
{
    public Dependency() : this(0) { }
}