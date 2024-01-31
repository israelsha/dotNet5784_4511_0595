using BlApi;
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
        { 
            throw new BO.BlInvalidDataException($"Invalid {error}");
        }

        DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Email, boEngineer.Cost, boEngineer.Name, (DO.EngineerExperience)boEngineer.Level);                        
        try
        {
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

        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Cost = doEngineer.Cost,
            Level = (BO.EngineerExperience)doEngineer.Level,
            Task = new BO.TaskInEngineer() { Id = task.Id, Alias = task.Alias }
        };

    }

    public IEnumerable<BO.Engineer> ReadAll()
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                select new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Name = doEngineer.Name,
                    Task = new BO.TaskInEngineer() { Id = BO.Tools.findTask(doEngineer.Id).Id, Alias = BO.Tools.findTask(doEngineer.Id).Alias }
                });
    }

    public void Update(BO.Engineer boEngineer)
    {
        //check if one of the parameter is invalid
        string error = "";
        if (boEngineer.Id <= 0) error = "Id";
        else if (boEngineer.Name == "") error = "Name";
        else if (boEngineer.Cost <= 0) error = "Cost";
        error = BO.Tools.IsValidEmail(boEngineer.Email);
        if (error != "")//there is invalid data
        {
            throw new BO.BlInvalidDataException($"Invalid {error}");
        }

        //If the engineer level update is lower than the current engineer level then the level will stay the same and not be updated
        if ((int)boEngineer.Level < (int)_dal.Engineer.Read(boEngineer.Id).Level) 
            boEngineer.Level = (BO.EngineerExperience)_dal.Engineer.Read(boEngineer.Id).Level;

        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////
        DO.Task task = _dal.Task.Read(boEngineer.Task.Id);

        DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Email, boEngineer.Cost, boEngineer.Name, (DO.EngineerExperience)boEngineer.Level);
        try//try to update
        {
           _dal.Engineer.Update(doEngineer);
            
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"Engineer with ID={doEngineer.Id} does Not exist",ex) ;
        }
    }


}
