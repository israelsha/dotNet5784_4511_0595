
namespace BO;

public class Milestone
{
  public int Id {  get; init; }
  public string Description {  get; set; }
  public string Alias {  get; set; }
  public DateTime CreatedAtDate { get; init; }
  public BO.Status Status { get; set; }                   //note: 'calculated'
  public DateTime ?ForecastDate { get; init; }            //note: 'a revised scheduled completion date'
  public DateTime ?DeadlineDate { get; set; }             //note: 'the latest complete date'
  public DateTime ?CompleteDate  { get; init; }           //note: 'real completion date'
  public double ?CompletionPercentage { get; set; }       //note: 'percentage of completed tasks - calculated'
  public string ?Remarks { get; set; }                    //note: 'free remarks from project meetings'
  public List<BO.TaskInList> ? Dependencies { get; set; } //ref: > BOTIL.Id
}
