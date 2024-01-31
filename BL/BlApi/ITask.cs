namespace BlApi;
/// <summary>
/// functhion for task
/// </summary>
public interface ITask
{
    //add new task
    public int Create(BO.Task item);

    //get Id and return the task with this Id
    public BO.Task? Read(int id);

    //return list of all the tasks
    public IEnumerable<BO.TaskInList> ReadAll();

    //updating the task
    public void Update(BO.Task item);

    //delete task
    public void Delete(int id);

    //check if the all date is correct
    public void CheckDate(int id,DateTime date);

    //public BO.MilestoneInTask GetDetailedMilestoneForTask(int MilestoneId, int TaskId);//return which Milestone are in this task
}
