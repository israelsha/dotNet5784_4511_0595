
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Xml.Serialization;

internal class EngineerImplementation : IEngineer
{
    readonly string s_engineers_xml = "engineers";

    public int Create(Engineer item)        //for entities with normal id (not auto id)
    {
        List<Engineer> arrEngneers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);//get list of engneers
        if (Read(item.Id) is not null)
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
       arrEngneers.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(arrEngneers, s_engineers_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        List<Engineer> arrEngneers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);//get list of engineers
        Engineer? obj = arrEngneers.Find(Engineer => Engineer.Id == id);
        if(obj != null)  //we find it
        {
            arrEngneers.Remove(obj);
            XMLTools.SaveListToXMLSerializer<Engineer>(arrEngneers, s_engineers_xml);
        }
        else throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");
    }

    public Engineer? Read(int id)
    {
        //get list of engineers from XML and then return the engineer with the same ID
        return XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml).FirstOrDefault(Engineer => Engineer.Id == id);
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        //get list of enigneers from XML and then return the engineer with the same filter
        return XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml).FirstOrDefault(Engineer => Engineer.Equals == filter);
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        //get list engineers from XML and then return the all list
        if (filter != null)
        {
            return from item in XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml)
                   where filter(item)
                   select item;
        }
        return from item in XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml)
               select item;
    }

    public void Update(Engineer item)
    {
        List<Engineer> arrEngneers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);//get list of engineers
        Engineer? obj = arrEngneers.Find(Engineer => Engineer.Id == item.Id);
        if (obj != null)     // we find it
        {
            arrEngneers.Remove(obj);
            arrEngneers.Add(item);
            XMLTools.SaveListToXMLSerializer(arrEngneers, s_engineers_xml);
        }
        else throw new DalDoesNotExistException($"Engineer with ID={item.Id} does Not exist");
    }
}

