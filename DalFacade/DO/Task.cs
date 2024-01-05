
using System;
using System.Security.Cryptography.X509Certificates;

namespace DO;

enum EngineerExperience
{
    Beginner,
  AdvancedBeginner,
  Intermediate,
  Advanced,
  Expert
}
public record Task
{
    int Id;
    string Alias;
    string Description;
    DateTime CreatedAtDate;                 //'Date when the task was added to the system']
    TimeSpan? RequiredEffortTime = null;    //'how many men-days needed for the task (for MS it is null)'
    bool IsMilestone;                       //'the real start date'
    DO.EngineerExperience ?Copmlexity=null; //task: minimum expirience for engineer to assign']
    DateTime? StartDate = null;
    DateTime? ScheduledDate = null;//[note: 'the planned start date']
    DateTime? DeadlineDate = null;//[note: 'the latest complete date']
    DateTime? CompleteDate = null; //[note: 'task: real completion date']
    string? Deliverables = null;
    string? Remarks= null;
    int? EngineerId = null;

    public Task()//empty constractor
    {
        Id = 0;
        Alias = "";
        Description = "";
        CreatedAtDate = DateTime.Now;
        IsMilestone = false;
    }
   //constractor with parameter
    public Task(int id,string alias, string description, DateTime createdAtDate, TimeSpan requiredEffortTime
        ,bool isMilestone, DateTime startDate,DateTime scheduledDate, DateTime deadlineDate,
        DateTime completeDate, string deliverables, string remarks, int engineerId)
    {
        Id = id;
        Alias = alias;
        Description = description;
        CreatedAtDate = createdAtDate;
        RequiredEffortTime= requiredEffortTime;
        IsMilestone = isMilestone;
        StartDate = startDate;
        ScheduledDate = scheduledDate;
        DeadlineDate= deadlineDate;
        DeadlineDate = completeDate;
        Deliverables = deliverables;
        Remarks = remarks;
        EngineerId = engineerId;
       // Copmlexity= a;
    }
}


    
    
    
