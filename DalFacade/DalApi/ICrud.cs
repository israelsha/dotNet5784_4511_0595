namespace DalApi;
using DO;


public interface ICrud<T> where T : class
{
    public int Create(T item);

    public void Delete(int id);

    public T? Read(int id);
    T? Read(Func<T, bool> filter);
    IEnumerable<T?> ReadAll(Func<T, bool>? filter = null); 
    public void Update(T item);

}


