namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;
 
internal class EngineerImplementation : IEngineer
{
    public IEngineer Engineer => new EngineerImplementation();

    public int Create(Engineer item)
    {
        //for entities with normal id (not auto id)
        if (Read(item.Id) is not null)
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
        DataSource.Engineers.Add(item);
        return item.Id;
    }

    public void Delete(int id)
    {
        Engineer? obj = DataSource.Engineers.Find(Engineer => Engineer.Id == id);
        if (obj != null)
        {
            DataSource.Engineers.Remove(obj);
        }
        else throw new DalDoesNotExistException($"Engineer with ID={id} does Not exist");
    }

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.FirstOrDefault(Engineer => Engineer.Id == id);
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        var obj = from item in DataSource.Engineers
                  where filter(item)
                  select item;
        return obj.FirstOrDefault();
    }

    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null) 
    {
        if (filter != null)
        {
            return from item in DataSource.Engineers
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Engineers
               select item;
    }


    public void Update(Engineer item)
    {
        Engineer? obj = DataSource.Engineers.Find(Engineer => Engineer.Id == item.Id);
        if (obj != null)     // we find it
        {
            DataSource.Engineers.Remove(obj);
            DataSource.Engineers.Add(item);
        }
        else throw new DalDoesNotExistException($"Engineer with ID={item.Id} does Not exist");
    }

}
