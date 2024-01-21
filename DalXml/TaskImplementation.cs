namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;

internal class TaskImplementation :ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(Task item)        //for entities with auto id
    {
        List<Task> lsTask = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); //get list of tasks
        int id = Config.NextTaskId;
        Task copy = item with { Id = id };
        lsTask.Add(copy);
        XMLTools.SaveListToXMLSerializer<Task>(lsTask, s_tasks_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        List<Task> lsTasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        Task? obj = lsTasks.Find(Task => Task.Id == id);
        if (obj != null)
        {
            lsTasks.Remove(obj);
            XMLTools.SaveListToXMLSerializer<Task>(lsTasks, s_tasks_xml);
        }
        else throw new DalDoesNotExistException($"Task with ID={id} does Not exist");
    }

    public Task? Read(int id)
    {
        return XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml).FirstOrDefault(Task => Task.Id == id);
    }

    public Task? Read(Func<Task, bool> filter)
    {
        var obj = from item in XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml)
                  where filter(item)
                  select item;
        return obj.FirstOrDefault();
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        if (filter != null)
        {
            return from item in XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml)
                   where filter(item)
                   select item;
        }
        return from item in XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml)
               select item;
    }

    public void Update(Task item)
    {
        List<Task> lsTasks = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml);
        Task? obj = lsTasks.Find(Task => Task.Id == item.Id);
        if (obj != null)  // we find it
        {
            lsTasks.Remove(obj);
            lsTasks.Add(item);
            XMLTools.SaveListToXMLSerializer<Task>(lsTasks, s_tasks_xml);
        }
        else throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist");

    }
}
