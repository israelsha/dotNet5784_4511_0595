namespace BlApi;
/// <summary>
/// functhion for task
/// </summary>
public interface ITask
{
    //add new task
    public int Create(BO.Task item);

    //get Id and return the task with this Id
    public BO.Task? Read(int id, Func<DO.Task, bool>? filter = null);

    //return list of all the tasks
    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task, bool>? filter = null);

    //updating the task
    public void Update(BO.Task item);

    //delete task
    public void Delete(int id);

    //check if the all date is correct
    public void UpdateDate(int id,DateTime date);

    public void resetDate(DateTime startProject);

}
