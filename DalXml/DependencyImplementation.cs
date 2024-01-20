
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Linq;

internal class DependencyImplementation :IDependency
{
    readonly string s_dependencies_xml = "dependencies";

    public int Create(Dependency item)
    {
        //for entities with auto id
       XElement Dependencies  = XMLTools.LoadListFromXMLElement(s_dependencies_xml); //get list of dependencies
        int id = Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        Dependencies.Add(copy);
        XMLTools.SaveListToXMLElement(Dependencies, s_dependencies_xml);
        return id;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Dependency item)
    {
        throw new NotImplementedException();
    }
}
