using Dal;

namespace BlApi;
public interface IBl
{
    public ITask Task { get; }
    public IEngineer Engineer { get; }
    public IMilestone Milestone { get; }

    public IDates Dates { get; }

    #region clock
    public DateTime Clock { get; }
    public void addHour();
    public void subHour();
    public void addDay();
    public void subDay();
    public void addMonth();
    public void subMonth();
    public void ResetClock();
    #endregion

}



