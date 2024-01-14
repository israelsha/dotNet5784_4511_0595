namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        //for entities with auto id
        int id = DataSource.Config.NextTaskId;
        Task copy = item with { Id = id };
        DataSource.Tasks.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        Task? obj = DataSource.Tasks.Find(Task => Task.Id == id);
        if (obj != null)
        {
            DataSource.Tasks.Remove(obj);
        }
        else throw new Exception($"Task with ID={id} does Not exist");

    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(Task => Task.Id == id);
    }

    public List<Task> ReadAll()
    {
        List<Task> list = new List<Task>();
        list.AddRange(DataSource.Tasks);
        return new List<Task>(DataSource.Tasks);
    }

    public void Update(Task item)
    {
        Task? obj = DataSource.Tasks.Find(Task => Task.Id == item.Id);
        if (obj != null)  // we find it
        {
            DataSource.Tasks.Remove(obj);
            DataSource.Tasks.Add(item);
        }
        else throw new Exception($"Task with ID={item.Id} does Not exist");

    }

    public ITask Task => new TaskImplementation();
}