﻿namespace BO;

using BlApi;
using BlImplementation;
using Dal;
using DalApi;
using DO;
using System.Net.Mail;

internal static class Tools
{
    private static Dal.IDal _dal = DalApi.Factory.Get;

    internal static string IsValidEmail(string email)
    {
        try
        {
            MailAddress mailAddress = new MailAddress(email);
            return "";
        }
        catch
        {
            return "Email";
        }
    }

    internal static DO.Task? findTask(int ingineerId)
    {
        IEnumerable<DO.Task> tasks = _dal.Task.ReadAll();//bring the list of all tasks
        return (from item in tasks     //select the task that this (our) engineer is doing (ie: id=EngineerId)
                where (item.EngineerId == ingineerId)
                select item).FirstOrDefault();
    }

    //calc the the status by the dates
    internal static BO.Status calcStatus(DO.Task doTask, DateTime clockDate)
    {
        if (doTask.ScheduledDate == null) return Status.Unscheduled;
        if (doTask.StartDate == null)
            if (doTask.DeadlineDate != null && clockDate > doTask.DeadlineDate)
                return Status.InJeopardy;
            else
                return Status.Scheduled;
        if (doTask.CompleteDate == null)
            if (clockDate > doTask.DeadlineDate)
                return Status.InJeopardy;
            else
                return Status.OnTrack;
        return Status.Done;
    }

    //converting from BO.Engineer to DO.Engineer
    internal static DO.Engineer boToDo(BO.Engineer boEngineer)
    {
        return new DO.Engineer
            (boEngineer.Id, boEngineer.Email, boEngineer.Cost, boEngineer.Name, (DO.EngineerExperience)boEngineer.Level);
    }

    //converting from DO.Engineer to BO.Engineer
    internal static BO.Engineer doToBo(DO.Engineer doEngineer)
    { 
        return new BO.Engineer()
        {
            Id = doEngineer.Id,
            Name = doEngineer.Name,
            Email = doEngineer.Email,
            Task =(BO.Tools.findTask(doEngineer.Id)==null)?null:
            new BO.TaskInEngineer() { Id = BO.Tools.findTask(doEngineer.Id).Id, Alias = BO.Tools.findTask(doEngineer.Id).Alias },
            Cost = doEngineer.Cost,
            Level = (BO.EngineerExperience)doEngineer.Level
        };
     }
        

    //converting from BO.Task to DO.Task
    internal static DO.Task boToDo(BO.Task boTask)
    {
        return new DO.Task(boTask.Id, boTask.Alias, boTask.Description, boTask.CreatedAtDate, false, boTask.RequiredEffortTime,
        (DO.EngineerExperience)boTask.Copmlexity, boTask.StartDate, boTask.ScheduledDate, boTask.DeadlineDate, boTask.CompleteDate,
        boTask.Deliverables, boTask.Remarks, (boTask.Engineer==null)?null: boTask.Engineer.Id);
    }

    //converting from DO.Task to BO.Task
    internal static BO.Task doToBo(DO.Task doTask,DateTime clockDate)
    {   
        //all the tasks that this task tepends on 
        var dependedId = from DO.Dependency doDependency in _dal.Dependency.ReadAll()
                 where doDependency.DependentTask==doTask.Id select doDependency.DependsOnTask;
        //make TaskInList
        List<BO.TaskInList> ?taskInList = new List<BO.TaskInList> () ;
        foreach(var item1 in dependedId)
        {
            DO.Task? dependedTask = _dal.Task.Read(item1 ?? 0);
            if (dependedTask is not null)
            {
                BO.TaskInList item2 = new TaskInList { Id = dependedTask.Id, Alias = dependedTask.Alias, Description = dependedTask.Description, Status = calcStatus(dependedTask, clockDate) };
                taskInList.Add(item2);
            }
        }
        return new BO.Task()
        {
            Id = doTask.Id,
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
            Copmlexity = (BO.EngineerExperience)doTask.Copmlexity,
            Engineer = ( _dal.Engineer.Read(doTask.EngineerId??0)==null) ? null :
            new BO.EngineerInTask { Id = doTask.EngineerId ?? 0, Name = _dal.Engineer.Read(doTask.EngineerId ?? 0).Name },
            ForecastDate = (doTask.StartDate == null || doTask.RequiredEffortTime == null) ? null : doTask.StartDate + doTask.RequiredEffortTime,
            Dependencies = (taskInList.Count==0)?null: taskInList,
            Status = calcStatus(doTask, clockDate)

        };
    }

    //check that the data of the task is correct 
    internal static void checkTaskData(BO.Task boTask)
    {
        //check if one of the parameter is invalid
        string error = "";
        if (boTask.Id < 0) error = "Id";
        else if (boTask.Alias == "") error = "Alias";
        else if (boTask.Description == "") error = "Description";
        if (error != "") //there is invalid data
            throw new BO.BlInvalidDataException($"Invalid {error}");
    }

    public static string ToStringProperty<T>(this T obj)
    {
        string result = "";
        var x = obj.GetType();
        var ls= x.GetProperties();
        foreach ( var prop in ls )
        {
            if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
            {
                result += $"{prop.Name}: ";
                var list = (IEnumerable <T>)prop.GetValue(obj);
                foreach (var item in list)
                {
                    result += $"{item}. \n ";
                }
            }
            else
                result += $"{prop.Name}: {prop.GetValue(obj)}. \n";
        }
        return result;
    }

    /// <summary>
    /// Checks whether there will be loops of tasks that depend on each other
    /// intended to be useful to know if we can add the dependency to the task
    /// </summary>
    /// <returns>true if there is a loop and false if there is no loops </returns>
    public static bool areThereLoops(int taskId, int ?dependOnTaskId)
    {
        var dependedId = from DO.Dependency doDependency in _dal.Dependency.ReadAll()
                         where doDependency.DependentTask == dependOnTaskId
                         select doDependency.DependsOnTask;
        if (dependedId == null) return false;

        foreach(var depId in dependedId )
        {
            int? dependsOnTask = _dal.Dependency.Read(depId ?? 0).DependsOnTask;
            if (dependsOnTask == taskId) return true;
            areThereLoops(taskId, dependsOnTask);
        }

        return false;
    }

}
