namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
    public IDependency Dependency => new DependencyImplementation();

    public int Create(Dependency item)
    {
        //for entities with auto id
        int id = DataSource.Config.NextDependencyId;
        Dependency copy = item with { Id = id };
        DataSource.Dependencies.Add(copy);
        return id;
    }

    public void Delete(int id)
    {
        Dependency? obj = DataSource.Dependencies.Find(Dependency => Dependency.Id == id);
        if (obj != null)
        {
            DataSource.Dependencies.Remove(obj);
        }
        else throw new DalDoesNotExistException($"Dependency with ID={id} does Not exist");
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(Dependency => Dependency.Id == id);
    }

    public Dependency? Read(Func<Dependency, bool> filter)
    {
        var obj = from item in DataSource.Dependencies
                  where filter(item)
                  select item;
        return obj.FirstOrDefault();
    }

    public IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null) 
    {
        if (filter != null)
        {
            return from item in DataSource.Dependencies
                   where filter(item)
                   select item;
        }
        return from item in DataSource.Dependencies
               select item;
    }


    public void Update(Dependency item)
    {
        Dependency? obj = DataSource.Dependencies.Find(Dependency => Dependency.Id == item.Id);
        if (obj != null)  // we find it
        {
            DataSource.Dependencies.Remove(obj);
            DataSource.Dependencies.Add(item);
        }
        else throw new DalDoesNotExistException($"Dependency with ID={item.Id} does Not exist");
    }

}
