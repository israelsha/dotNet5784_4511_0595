
using System;

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

    int Id; // unique ID of the Task
    string Alias;
    string Description;
    DateTime CreatedAtDate;//Date when the task was added to the system
    TimeSpan? RequiredEffortTime = null; //how many men-days needed for the task (for MS it is null)
    bool IsMilestone;
    DO.EngineerExperience? Copmlexity = null; //task: minimum expirience for engineer to assign
    DateTime? StartDate = null; //the real start date
    DateTime? ScheduledDate = null;   //the planned start date
    DateTime? DeadlineDate = null;  //the latest complete date
    DateTime? CompleteDate = null; // task: real completion date
    string? Deliverables = null; //task: description of deliverables for MS copmletion
    string? Remarks = null;  //free remarks from project meetings
    int? EngineerId = null;
  


public Task() { Id = 0; Alias = ""; Description = ""; CreatedAtDate = DateTime.Now; IsMilestone = false; }
    public Task(int id, string alias, string description, DateTime createdAtDate, bool isMilestone,
        TimeSpan? requiredEffortTime, DateTime? startDate, DateTime? scheduledDate, DateTime? deadlineDate,
       DateTime? completeDate, string? deliverables, string? remarks, int? engineerId) 
    {
        Id = id;
        Alias = alias;
        Description = description; 
        CreatedAtDate = createdAtDate; 
        IsMilestone = isMilestone;
        RequiredEffortTime = requiredEffortTime;
        StartDate = startDate;      
        ScheduledDate = scheduledDate;
        DeadlineDate = deadlineDate;
        CompleteDate = completeDate;
        Deliverables = deliverables;
        Remarks = remarks;
        EngineerId = engineerId;
    }



}
