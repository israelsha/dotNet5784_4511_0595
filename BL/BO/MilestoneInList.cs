
namespace BO;

public class MilestoneInList
{
  int Id {  get; init; }
  string Description {  get; set; }
  string Alias { get; set; }
  BO.Status Status { get; set; }            //note: 'calculated'
    double? CompletionPercentage { get; set; } //note: 'percentage of completed tasks - calculated']
}
