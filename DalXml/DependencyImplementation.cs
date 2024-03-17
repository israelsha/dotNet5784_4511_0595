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
        //for entities with auto id
        XElement dependencyElem = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        int id = Config.NextDependencyId;
        XElement x_id = new XElement("ID", id);
        XElement x_dependentTask = new XElement("DependentTask",item.DependentTask);
        XElement x_dependsOnTask = new XElement("DependsOnTask",item.DependsOnTask);
        XElement x_dependency = new XElement("Dependency", x_id, x_dependentTask, x_dependsOnTask);
        dependencyElem.Add(x_dependency);
        XMLTools.SaveListToXMLElement(dependencyElem, s_dependencies_xml);

        return id;
    }

    public void Delete(int id)
    {
        XElement dependencyElem = XMLTools.LoadListFromXMLElement(s_dependencies_xml);

        XElement? toRemove = dependencyElem.Elements().FirstOrDefault(st => (int?)st.Element("ID") == id);
        if(toRemove is not null)
        {
            toRemove.Remove();  
            XMLTools.SaveListToXMLElement(dependencyElem, s_dependencies_xml);
        }
        else throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist");

    }

    public Dependency? Read(int id)
    {
        XElement rootDependency = XMLTools.LoadListFromXMLElement(s_dependencies_xml);// this is root
        return (from depend in rootDependency.Elements()
                where (int?)depend.Element("ID") == id
                select new Dependency()
                {
                    Id = (int)(depend.Element("ID")),
                    DependentTask = (int?)depend.Element("DependentTask"),
                    DependsOnTask = (int?)depend.Element("DependsOnTask")
                }).FirstOrDefault() ?? throw new DalDoesNotExistException($"ID: {id}, not exist");
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        return XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().Select(s => XMLTools.getDependency(s)).FirstOrDefault(filter);
    }

    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null)
    {
        if(filter is null)
            return XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().Select(s => XMLTools.getDependency(s));
        else
            return XMLTools.LoadListFromXMLElement(s_dependencies_xml).Elements().Select(s=>XMLTools.getDependency(s)).Where(filter);
    }

    public void Update(Dependency item)
    {
        XElement? dependencyElem = XMLTools.LoadListFromXMLElement(s_dependencies_xml);
        XElement? toRemove = dependencyElem.Elements().FirstOrDefault(st => (int?)st.Element("Id") == item.Id);
        if (toRemove is not null)
        {
            toRemove.Remove();
            XElement x_id = new XElement("ID", item.Id);
            XElement x_dependentTask = new XElement("DependentTask", item.DependentTask);
            XElement x_dependsOnTask = new XElement("DependsOnTask", item.DependsOnTask);
            XElement x_dependency = new XElement("Dependency", x_id, x_dependentTask, x_dependsOnTask);
            dependencyElem.Add(x_dependency);
            XMLTools.SaveListToXMLElement(dependencyElem, s_dependencies_xml);
        }
        else throw new DalDoesNotExistException($"Dependency with ID={item.Id} does Not exist");
    }
}
