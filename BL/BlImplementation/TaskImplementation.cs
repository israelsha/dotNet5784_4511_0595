using BlApi;
using BO;
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
        else if (boTask.Alias=="") error = "Alias";
        if (error != "") //there is invalid data
        {
            throw new BO.BlInvalidDataException($"Invalid {error}");
        }

        DO.Task doTask = new DO.Task(boTask.Id, boTask.Alias, boTask.Description, boTask.CreatedAtDate, false, boTask.RequiredEffortTime,
            (DO.EngineerExperience)boTask.Status, boTask.StartDate, boTask.ScheduledDate, boTask.DeadlineDate, boTask.CompleteDate,
            boTask.Deliverables, boTask.Remarks, boTask.Engineer.Id);

        try
        {
            int idTask = _dal.Task.Create(doTask);
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
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"Task with ID={id} does Not exist");


        return new BO.Task()
        {
            Id = id,
            Description = doTask.Description,
            Alias = doTask.Alias,
            CreatedAtDate = doTask.CreatedAtDate,

            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Copmlexity = (BO.EngineerExperience)doTask.Copmlexity
            //Engineer=
            //Dependencies
            //ForecastDate= doTask.
            //Dependencies = doTask,
            //Status = 

        };
    }


        };

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
