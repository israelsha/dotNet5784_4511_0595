namespace DalApi;
using DO;

public interface ICrud<T> where T : class
{
    public int Create(T item);
    public void Delete(int id);
    public T? Read(int id);
    public List<T> ReadAll();
    public void Update(T item);
}


