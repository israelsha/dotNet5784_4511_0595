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
        if (boEngineer.Id <= 0) error = "Id";
        else if (boEngineer.Name == "") error = "Name";
        else if (boEngineer.Cost <= 0) error = "Cost";
        error= BO.Tools.IsValidEmail(boEngineer.Email); 
        if(error != "") //there is invalid data
            throw new BO.BlInvalidDataException($"Invalid {error}");

                             
        try
        {
            DO.Engineer doEngineer = Tools.boToDo(boEngineer);
            int idEngineer = _dal.Engineer.Create(doEngineer);
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
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");

        DO.Task task = BO.Tools.findTask(id);

        return Tools.doToBo(doEngineer);
    }

    public IEnumerable<BO.Engineer> ReadAll()
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                select Tools.doToBo(doEngineer));
    }

    public void Update(BO.Engineer boEngineer)
    {
        //check if one of the parameter is invalid
        string error = "";
        if (boEngineer.Id <= 0) error = "Id";
        else if (boEngineer.Name == "") error = "Name";
        else if (boEngineer.Cost <= 0) error = "Cost";
        error = BO.Tools.IsValidEmail(boEngineer.Email);
        //there is invalid data
        if (error != "")
            throw new BO.BlInvalidDataException($"Invalid {error}");

        //If the engineer level update is lower than the current engineer level then the level will stay the same and not be updated
        if ((int)boEngineer.Level < (int)_dal.Engineer.Read(boEngineer.Id).Level) 
            boEngineer.Level = (BO.EngineerExperience)_dal.Engineer.Read(boEngineer.Id).Level;


        int errorId = 0;    
        try//updating
        {
            DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Email, boEngineer.Cost, boEngineer.Name, (DO.EngineerExperience)boEngineer.Level);
            //tryng to update the engineer
            errorId = doEngineer.Id;
            error = "Engineer";
           _dal.Engineer.Update(doEngineer);

            //trying to update the task because the EngineerId might changed 
            errorId = boEngineer.Task.Id;
            error = "Task";
            //Initializes the EngineerId in the previous task
            DO.Task prevTask = Tools.findTask(boEngineer.Id) with { EngineerId = null };
            _dal.Task.Update(prevTask);
            //Enters the engineer's ID in the appropriate task
            DO.Task updetedTask = _dal.Task.Read(boEngineer.Task.Id) with { EngineerId = boEngineer.Id };
            _dal.Task.Update(updetedTask);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"{error} with ID={errorId} does Not exist",ex) ;
        }
    }


}
