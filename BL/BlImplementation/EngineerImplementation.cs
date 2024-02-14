using BlApi;
using BO;
namespace BlImplementation;

internal class EngineerImplementation : IEngineer
{
    private Dal.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Engineer boEngineer)
    {
        //check if one of the parameter is invalid
        string error = "";
        error = BO.Tools.IsValidEmail(boEngineer.Email);
        if (boEngineer.Id <= 0) error = "Id";
        else if (boEngineer.Name == "") error = "Name";
        else if (boEngineer.Cost <= 0) error = "Cost";
        else if ((int)boEngineer.Level >= 5) error = "Level";
        if(error != "") //there is invalid data
            throw new BO.BlInvalidDataException($"Invalid {error}");

        if (boEngineer.Task != null && _dal.Task.Read(boEngineer.Task.Id) == null) 
            throw new BO.BlAlreadyExistsException($"Task with ID={boEngineer.Task.Id} doesn't exist");
        try
        {
            DO.Engineer doEngineer = Tools.boToDo(boEngineer);
            int idEngineer = _dal.Engineer.Create(doEngineer);

            //updating in the appropriate the taskId 
            if (boEngineer.Task == null) return idEngineer;
            //insert the engineer's ID in the appropriate task
            DO.Task updetedTask = _dal.Task.Read(boEngineer.Task.Id) with { EngineerId = boEngineer.Id };
            _dal.Task.Update(updetedTask);
            return idEngineer;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exists", ex);
        }

    }

    public void Delete(int id)
    {
        //get the ingineer and if the ingineer doesn't exsist it will throw en error
        BO.Engineer boEngineer = Read(id);
        DO.Task task = _dal.Task.Read(boEngineer.Task.Id);

        //check if the engineer is allredy started the task
        if(task.StartDate<DateTime.Now)
            throw new BO.cannotDeleteException("The engineer is already in the middle or finished the task and therefore cannot be deleted ");
   
         _dal.Engineer.Delete(id);
    }

    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID: {id} does Not exist");
        return Tools.doToBo(doEngineer);
    }

    public IEnumerable<BO.Engineer> ReadAll(Func<DO.Engineer, bool>? filter = null)
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll(filter)
                select Tools.doToBo(doEngineer));
    }

    public void Update(BO.Engineer boEngineer)
    {
        if (_dal.Engineer.Read(boEngineer.Id) is null)//engineer does'nt exist
            throw new BO.BlDoesNotExistException($"Engineer with ID: {boEngineer.Id} does Not exist");
        if (boEngineer.Task != null && _dal.Task.Read(boEngineer.Task.Id) is null) //task does'nt exist
            throw new BO.BlDoesNotExistException($"Task with ID: {boEngineer.Task.Id} does Not exist");

        //check if one of the parameter is invalid
        string error = "";
        error = BO.Tools.IsValidEmail(boEngineer.Email);
        if (boEngineer.Id <= 0) error = "Id";
        else if (boEngineer.Name == "") error = "Name";
        else if (boEngineer.Cost <= 0) error = "Cost";
        else if ((int)boEngineer.Level >= 5) error = "Level";
        //there is invalid data
        if (error != "")
            throw new BO.BlInvalidDataException($"Invalid {error}");

        //If the engineer level update is lower than the current engineer level then the level will stay the same and not be updated
        if ((int)boEngineer.Level < (int)_dal.Engineer.Read(boEngineer.Id).Level)
            boEngineer.Level = (BO.EngineerExperience)_dal.Engineer.Read(boEngineer.Id).Level;

        //updating
        DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Email, boEngineer.Cost, boEngineer.Name, (DO.EngineerExperience)boEngineer.Level);
        //tryng to update the engineer
        _dal.Engineer.Update(doEngineer);

        //trying to update the task because the EngineerId might changed 
        //Initializes the EngineerId in the previous task
        if (boEngineer.Task == null) return;
        DO.Task prevTask = Tools.findTask(boEngineer.Id) with { EngineerId = null };
        _dal.Task.Update(prevTask);
        //insert the engineer's ID in the appropriate task
        DO.Task updetedTask = _dal.Task.Read(boEngineer.Task.Id) with { EngineerId = boEngineer.Id };
        _dal.Task.Update(updetedTask);
    }
}
