using BlApi;
using BO;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private Dal.IDal _dal = DalApi.Factory.Get;

    public void CheckDate(int id, DateTime date)
    {
        throw new NotImplementedException();
    }

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
            DO.Dependency doDependency = new DO.Dependency(0, boTask.Id, item.Id);
            _dal.Dependency.Create(doDependency);
        }

        return idTask;
    }

    string error = "";
    public void Delete(int id)
    {
        try//try to delete 
        {
            var depended = from doDependency in _dal.Dependency.ReadAll()
                             where doDependency.DependentTask == id
                             select doDependency;

            if (depended != null)
            {
                error = "Task";
                _dal.Task.Delete(id);
                //delete all the Dependencies that is conected with this task
                error = "Dependency";
                IEnumerable<int>? dependedId = from doDependency in _dal.Dependency.ReadAll()
                               where doDependency.DependsOnTask == id
                               select doDependency.Id;
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

    public IEnumerable<BO.TaskInList> ReadAll()
    {
        return (from DO.Task doTask in _dal.Task.ReadAll()
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
        try
        {
            _dal.Task.Update(Tools.boToDo(boTask));
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Task with ID={boTask.Id} does Not exist",ex);
        }
    }
}
