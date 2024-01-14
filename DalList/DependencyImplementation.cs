namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;

internal class DependencyImplementation : IDependency
{
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
        else throw new Exception($"Dependency with ID={id} does Not exist");
    }

    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.Find(Dependency => Dependency.Id == id);
    }

    public List<Dependency> ReadAll()
    {

        return new List<Dependency>(DataSource.Dependencies);
    }

    public void Update(Dependency item)
    {
        Dependency? obj = DataSource.Dependencies.Find(Dependency => Dependency.Id == item.Id);
        if (obj != null)  // we find it
        {
            DataSource.Dependencies.Remove(obj);
            DataSource.Dependencies.Add(item);
        }
        else throw new Exception($"Dependency with ID={item.Id} does Not exist");
    }
    public IDependency Dependency => new DependencyImplementation();

}