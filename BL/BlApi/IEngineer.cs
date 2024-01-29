namespace BlApi;

/// <summary>
/// functhion for Engineer
/// </summary>
public interface IEngineer
{
    public int Create(BO.Engineer item);
    public BO.Engineer? Read(int id);
    public IEnumerable<BO.Engineer> ReadAll();
    public void Update(BO.Engineer item);
    public void Delete(int id);
    public BO.TaskInEngineer GetDetailedTaskForEngineer(int EngineerId, int TaskId);//return which Task the engineer is doing
}
