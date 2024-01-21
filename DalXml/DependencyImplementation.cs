
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

internal class DependencyImplementation :IDependency
{
    readonly string s_dependencies_xml = "dependencies";

    public int Create(Dependency item)
    {
        XElement Dependcies=XMLTools.LoadListFromXMLElement(s_dependencies_xml);   

    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Dependency? Read(int id)
    {
        XElement? dependencyElem = XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().FirstOrDefault(st => (int?)st.Element("Id") == id);
        //XElenent
        return dependencyElem is null ? null : XMLTools.ToEnumNullable<Dependency>(dependencyElem, s_dependencies_xml);

    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().Select(s => getDependency(s)).FirstOrDefault(filter);
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
