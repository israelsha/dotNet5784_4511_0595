
using System.Xml;

namespace DO;

/// <param name="Id"></param>
/// <param name="Email"></param>
/// <param name="Cost"></param>
/// <param name="Name"></param>
/// <param name="Level"></param>
public record Engineer
(
    int Id,
    string Email,
    double Cost,     //daily cost of the engineer, including salary, workplace, tools
    string Name,
    DO.EngineerExperience Level
)
{
    public Engineer() : this(0, "", 0.0, "", EngineerExperience.Beginner) { }
}