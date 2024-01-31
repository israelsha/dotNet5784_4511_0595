namespace BlApi;

/// <summary>
/// functhion for Engineer
/// </summary>
public interface IEngineer
{
    //add new engineer
    public int Create(BO.Engineer item);

    //get Id and return the engineer with this Id
    public BO.Engineer? Read(int id);

    //return list of all the engineers
    public IEnumerable<BO.Engineer> ReadAll();

    //updating the ingineer
    public void Update(BO.Engineer item);

    //delete engineer
    public void Delete(int id);

}
