namespace Dal;
using DalApi;
using DO;
using System.Collections.Generic;



internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        //for entities with normal id (not auto id)
        if (Read(item.Id) is not null)
            throw new Exception($"Engineer with ID={item.Id} already exists");
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
        else throw new Exception($"Engineer with ID={id} does Not exist");
    }

    public Engineer? Read(int id)
    {
        return DataSource.Engineers.Find(Engineer => Engineer.Id == id);
    }

    public List<Engineer> ReadAll()
    {
        List<Engineer> list = new List<Engineer>();
        list.AddRange(DataSource.Engineers);
        return new List<Engineer>(DataSource.Engineers);
    }

    public void Update(Engineer item)
    {
        Engineer? obj = DataSource.Engineers.Find(Engineer => Engineer.Id == item.Id);
        if (obj != null)     // we find it
        {
            DataSource.Engineers.Remove(obj);
            DataSource.Engineers.Add(item);
        }
        else throw new Exception($"Engineer with ID={item.Id} does Not exist");
    }
    public IEngineer Engineer => new EngineerImplementation();

}