namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
using System.Linq;

internal class TaskImplementation : ITask
{
    public ITask Task => new TaskImplementation();

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
        else throw new DalDoesNotExistException($"Task with ID={id} does Not exist");

    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(Task => Task.Id == id);
    }

    public Task? Read(Func<Task, bool> filter)
    {
        var obj = from item in DataSource.Tasks
                  where filter(item)
                  select item;
        return obj.FirstOrDefault();
    }

    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null) 
    {
        if (filter != null)
        {
            return from item in DataSource.Tasks
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Tasks
               select item;
    }

    public void Update(Task item)
    {
        Task? obj = DataSource.Tasks.Find(Task => Task.Id == item.Id);
        if (obj!=null)  // we find it
        {
            DataSource.Tasks.Remove(obj);
            DataSource.Tasks.Add(item);
        }
        else throw new DalDoesNotExistException($"Task with ID={item.Id} does Not exist");

    }

}
