namespace BlApi;

/// <summary>
/// functhion for Milestone
/// </summary>
public interface IMilestone
{
 
    public BO.Milestone? Create(int id);
    
    public void Update(BO.Milestone item);

}
