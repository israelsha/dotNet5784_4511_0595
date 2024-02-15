using BlApi;
using BO;
namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private Dal.IDal _dal = DalApi.Factory.Get;

    public int Create(BO.Task boTask)
    {
        //check if one of the parameter is invalid
        Tools.checkTaskData(boTask);

        //create new Task
        DO.Task doTask = Tools.boToDo(boTask);
        int idTask = _dal.Task.Create(doTask);

        //create new dependsies
        foreach (var item in boTask.Dependencies)
        {
            DO.Dependency doDependency = new DO.Dependency(0, idTask, item.Id);
            _dal.Dependency.Create(doDependency);
        }

        return idTask;
    }

    string error = "";
    public void Delete(int id)
    {
        try//try to delete 
        {
            if (_dal.Task.Read(id) == null) throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist") ;
            var depended = from doDependency in _dal.Dependency.ReadAll()
                             where doDependency.DependentTask == id
                             select doDependency;

            if (depended != null&&_dal.Task.Read(id).ScheduledDate==null )
            {
                error = "Task";
                _dal.Task.Delete(id);
                //delete all the Dependencies that is conected with this task
                error = "Dependency";
                IEnumerable<int>? dependedId = from doDependency in _dal.Dependency.ReadAll()
                               where doDependency.DependsOnTask == id select doDependency.Id;
                foreach (var item in dependedId) _dal.Dependency.Delete(item);
            }
            else
                throw new cannotDeleteException($"Task with {id} can't be deleted");
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"{error} with ID={id} does Not exist", ex);
        }
    }

    public BO.Task? Read(int id)
    {
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");

        return Tools.doToBo(doTask);
    }

    public IEnumerable<BO.TaskInList> ReadAll(Func<DO.Task, bool>? filter = null)
    {
        return (from DO.Task doTask in _dal.Task.ReadAll(filter)
                select new BO.TaskInList
                {
                    Id = doTask.Id,
                    Description = doTask.Description,
                    Alias = doTask.Alias,
                    Status = Tools.calcStatus(doTask)
                });
    }

    public void Update(BO.Task boTask)
    { 
        Tools.checkTaskData(boTask);
        DO.Task temp = _dal.Task.Read(boTask.Id);
        try
        {
            if(boTask.Dependencies != null)
            {
                //get all the conected dependecy Id
                IEnumerable<int> dependecyId = from doDependency in _dal.Dependency.ReadAll()
                    where doDependency.DependentTask == boTask.Id select doDependency.Id;
                //delete all the conected dependecies
                foreach (var item in dependecyId) _dal.Dependency.Delete(item);
                //add all the new dependecies
                foreach (var item in boTask.Dependencies)
                    _dal.Dependency.Create(new DO.Dependency { DependentTask = boTask.Id, DependsOnTask = item.Id });
            }
            if (boTask.ScheduledDate == null)
                _dal.Task.Update(Tools.boToDo(boTask));
            else _dal.Task.Update(Tools.boToDo(boTask) with { ScheduledDate=temp.ScheduledDate,StartDate=temp.StartDate,
                DeadlineDate=temp.DeadlineDate,CreatedAtDate=temp.CreatedAtDate,CompleteDate=temp.CompleteDate,RequiredEffortTime=temp.RequiredEffortTime });
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={boTask.Id} does Not exist",ex);
        }
    }

    public void UpdateDate(int id, DateTime date)
    {
        DO.Task? task = _dal.Task.Read(id);
        if (task != null)
        {
            IEnumerable<int?>? dependededTask = from doDependency in _dal.Dependency.ReadAll()
                                                where doDependency.DependentTask == id
                                                select doDependency.DependsOnTask;
            bool flag = true;
            foreach (var item in dependededTask) flag = (task.ScheduledDate == null) ? false : flag;
            if (flag == false) throw new errorInDateException("The start date of the previous tasks does not exist");
            foreach (var item in dependededTask) flag = (task.DeadlineDate == null || task.DeadlineDate > date) ? false : flag;
            if (flag == false) throw new errorInDateException("the date is before Deadline date of depended task");
            _dal.Task.Update(task with { ScheduledDate = date });
        }
        else throw new BlDoesNotExistException($"Task with ID ={id} does Not exist");
    }

    //external function  to reset all the ScheduledDate and the deadline of all the task 
    public void resetDate(DateTime startProject)
    {
        //find all the task that is not depend on any other task ie: task.Dependencies = null
        IEnumerable<BO.Task> notDependentTask = from doTask in _dal.Task.ReadAll()
                                                 let boTask = Read(doTask.Id)
                                                 where boTask.Dependencies == null || boTask.Dependencies.Count() == 0
                                                 select boTask;
        reset(startProject,notDependentTask);
    }
    //recursive function, reset all the ScheduledDate and the deadline of all the task
    public void reset(DateTime? prevDate, IEnumerable<BO.Task> ?tasks)
    {
        if (tasks != null)
            foreach (var item in tasks) 
            { 
                //update the task whit the correct ScheduledDate and DeadlineDate
                _dal.Task.Update(Tools.boToDo(item) with
                {
                    ScheduledDate = (item.ScheduledDate == null || item.ScheduledDate < prevDate) ? prevDate : item.ScheduledDate,
                    DeadlineDate = prevDate + item.RequiredEffortTime
                });
                //sending the tasks that is depending on this task
                reset(prevDate + item.RequiredEffortTime, from dep in _dal.Dependency.ReadAll()
                                                          where dep.DependsOnTask != null && dep.DependsOnTask == item.Id
                                                          select Read(dep.DependentTask ?? 0));
            }
    }
}
