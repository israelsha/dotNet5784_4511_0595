
namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Data.Common;

internal class TaskImplementation :ITask
{
    readonly string s_tasks_xml = "tasks";

    public int Create(Task item)        //for entities with auto id
    {
        List<Task> arrTask = XMLTools.LoadListFromXMLSerializer<Task>(s_tasks_xml); //get list of task
        int id = Config.NextTaskId;
        Task copy = item with { Id = id };
        arrTask.Add(copy);
        XMLTools.SaveListToXMLSerializer<Task>(arrTask, s_tasks_xml);
        return item.Id;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Task? Read(Func<Task, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Task item)
    {
        throw new NotImplementedException();
    }
}
