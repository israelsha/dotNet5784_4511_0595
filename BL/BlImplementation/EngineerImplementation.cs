using BlApi;
using BO;
using System.Runtime.CompilerServices;
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
        error = Tools.isValidMail(boEngineer.Email);
        if (error != "") { throw new BO.invalidDataException($"Invalid {error}"); }

        DO.Engineer doEngineer = new DO.Engineer(boEngineer.Id, boEngineer.Email, boEngineer.Cost, boEngineer.Name, (DO.EngineerExperience)boEngineer.Level);
        try
        {
            int idEng = _dal.Engineer.Create(doEngineer);
            return idEng;
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
        

    }

    public IEnumerable<BO.Engineer> ReadAll()
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                select new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Name = doEngineer.Name,
                });

    }

    public void Update(BO.Engineer item)
    {
        throw new NotImplementedException();
    }
}
