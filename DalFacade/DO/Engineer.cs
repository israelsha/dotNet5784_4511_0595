
using System.Xml;

namespace DO;

public record Engineer
{
    int Id;
    string Email;
    double Cost;  //daily cost of the engineer, including salary, workplace, tools
    string Name;
    DO.EngineerExperience Level;

    public Engineer() { Id = 0; Email = ""; Cost = 0.0; Name = ""; Level = EngineerExperience.Beginner; }

    public Engineer(int id, string email, double cost, string name)
    {
        Id = id;
        Email = email;
        Cost = cost;
        Name = name;
    }
}