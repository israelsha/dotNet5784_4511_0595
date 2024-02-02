
namespace BO;

/// <summary>
/// short list of all the tasks that the task depend on
/// </summary>
public class TaskInList
{
  public int Id {  get; init; }
  public string Description { get; init; }
  public string Alias { get; init; }
  public BO.Status Status { get; init; }  //note: calculated
}
