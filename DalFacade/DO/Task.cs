namespace DO;

/// <param name="Id"></param>
/// <param name="Alias"></param>
/// <param name="Description"></param>
/// <param name="CreatedAtDate"></param>
/// <param name="IsMilestone"></param>
/// <param name="RequiredEffortTime"></param>
/// <param name="Copmlexity"></param>
/// <param name="StartDate"></param>
/// <param name="ScheduledDate"></param>
/// <param name="DeadlineDate"></param>
/// <param name="CompleteDate"></param>
/// <param name="Deliverables"></param>
/// <param name="Remarks"></param>
/// <param name="EngineerId"></param>
public record Task
(
    int Id,                                  // unique ID of the Task
    string Alias,
    string Description,
    DateTime CreatedAtDate,                  //Date when the task was added to the system
    bool IsMilestone,
    TimeSpan? RequiredEffortTime = null,     //how many men-days needed for the task (for MS it is null)
    DO.EngineerExperience? Copmlexity = null,//task: minimum expirience for engineer to assign
    DateTime? StartDate = null,              //the real start date
    DateTime? ScheduledDate = null,          //the planned start date
    DateTime? DeadlineDate = null,           //the latest complete date
    DateTime? CompleteDate = null,           // task: real completion date
    string? Deliverables = null,             //task: description of deliverables for MS copmletion
    string? Remarks = null,                  //free remarks from project meetings
    int? EngineerId = null
)
{
    public Task() : this(0, "", "", DateTime.Now, false) { }
}
