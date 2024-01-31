using BlApi;

namespace BlImplementation;

internal class TaskImplementation : ITask
{
    private Dal.IDal _dal = DalApi.Factory.Get;

    public void CheckDate(int id, DateTime date)
    {
        throw new NotImplementedException();
    }

    public int Create(BO.Task item)
    {
        string error = "";
        if (boTask.Id <= 0) error = "Id";
        else if (boTask.Name == "") error = "Name";
        else if (boTask.Cost <= 0) error = "Cost";
        error = BO.Tools.IsValidEmail(boTask.Email);

        if (error != "")
        {
            throw new BO.BlInvalidDataException($"Invalid {error}");
        }
        DO.Engineer doEngineer = new DO.Engineer(boTask.Id, boTask.Email, boTask.Cost, boTask.Name, (DO.TaskExperience)boTask.Level);
        try
        {
            int idTask = _dal.Task.Create(doEngineer);
            return idTask;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Task with ID={boTask.Id} already exists", ex);
        }
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public BO.Task? Read(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BO.TaskInList> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task item)
    {
        throw new NotImplementedException();
    }
}
