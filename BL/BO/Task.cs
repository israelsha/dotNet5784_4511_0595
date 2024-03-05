namespace BO;


/// <summary>
/// all the properties for Task 
/// </summary>
public class Task
{
    public int Id { get; init; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public DateTime CreatedAtDate { get; init; }       // Date when the task was added to the system;
    public BO.Status? Status { get; set; }               // calculated;
    public List<BO.TaskInList> ?Dependencies { get; set; }//relevant only before schedule is built;
    public BO.MilestoneInTask? Milestone { get; set; } = null;  //calculated when building schedule, populated if there is milestone in dependency, relevant only after schedule is built;
    public TimeSpan? RequiredEffortTime { get; set; }   //how many men-days needed for the task;
    public DateTime ?StartDate { get; set; }            //the real start date;
    public DateTime ?ScheduledDate { get; set; }        //the planned start date;
    public DateTime? ForecastDate { get; set; } = null;        //calcualed planned completion date;
    public DateTime ?DeadlineDate { get; set; }         //the latest complete date;
    public DateTime ?CompleteDate { get; set; }         //real completion date;
    public string ?Deliverables { get; set; }           //description of deliverables for MS copmletion;
    public string? Remarks { get; set; }                //free remarks from project meetings;
    public BO.EngineerInTask? Engineer {  get; set; }   
    public BO.EngineerExperience Copmlexity { get; set; }//minimum expirience for engineer to assign;
    public override string ToString() => this.ToStringProperty();

}
