using BlApi;
namespace BlImplementation;


internal class EngineerImplementation : IEngineer
{
    private Dal.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Engineer boEngineer)
    {
        string error = "";
        if (boEngineer.Id <= 0) error = "Id";
        else if (boEngineer.Name == "") error = "Name";
        else if (boEngineer.Cost <= 0) error = "Cost";
       error= BO.Tools.IsValidEmail(boEngineer.Email);

        if(error != "") 
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
        throw new NotImplementedException();
    }

    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);
        if (doEngineer == null)
            throw new BO.BlDoesNotExistException($"Engineer with ID={id} does Not exist");

        return new BO.Engineer()
        {
            Id = id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Cost = doEngineer.Cost,
            Level = (BO.EngineerExperience)doEngineer.Level
            // Task = new BO.TaskInEngineer(doEngineer);
        };

    }

    public IEnumerable<BO.Engineer> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Engineer item)
    {
        throw new NotImplementedException();
    }
}
