namespace BO;

/// <summary>
/// class for the Engineer  that is going to do the task
/// </summary>
public class EngineerInTask
{
    public int Id {  get; init; }
    public string Name {  get; set; }
    public override string ToString() => this.ToStringProperty();
}
