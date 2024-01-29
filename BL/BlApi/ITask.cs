namespace BlApi;
/// <summary>
/// functhion for task
/// </summary>
public interface ITask
{
    public int Create(BO.Task item);
    public BO.Task? Read(int id);
    public IEnumerable<BO.TaskInList> ReadAll();
    public void Update(BO.Task item);
    public void Delete(int id);
    public BO.EngineerInTask GetDetailedEngineerForTask(int EngineerId, int TaskId);//return which Engineer doing this task
    public BO.MilestoneInTask GetDetailedMilestoneForTask(int MilestoneId, int TaskId);//return which Milestone are in this task
}
