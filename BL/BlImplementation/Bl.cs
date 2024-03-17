namespace BlImplementation;
using BlApi;

internal class Bl : IBl
{
    public ITask Task => new TaskImplementation(this);

    public IEngineer Engineer => new EngineerImplementation();

    public IMilestone Milestone => new MilestoneImplementation();

    public IDates Dates => new DatesImplementation();

    #region Clock
    private static DateTime s_Clock = DateTime.Now.Date;
    public DateTime Clock
    { get { return s_Clock; } private set { s_Clock = value; } }

    public void addHour()
    {
        Clock = Clock.AddHours(1);
    }
    public void subHour()
    {
        Clock= Clock.AddHours(-1);
    }

    public void addDay()
    {
        Clock = Clock.AddDays(1);
    }
    public void subDay()
    {
        Clock = Clock.AddDays(-1);
    }

    public void addMonth()
    {
        Clock = Clock.AddMonths(1);
    }
    public void subMonth()
    {
        Clock = Clock.AddMonths(-1);
    }

    public void ResetClock()
    {
        Clock = DateTime.Now;
    }

    #endregion
}
