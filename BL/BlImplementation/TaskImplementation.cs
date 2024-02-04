using BlApi;
using BO;
using System.Security.Cryptography;
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
        string error = "";
        if (boTask.Id <= 0) error = "Id";
        else if (boTask.Alias == "") error = "Alias";
        if (error != "") //there is invalid data
            throw new BO.BlInvalidDataException($"Invalid {error}");

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

    public void Delete(int id)
    {
        throw new NotImplementedException();
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

    public void Update(BO.Task item)
    {
        throw new NotImplementedException();
    }
}
