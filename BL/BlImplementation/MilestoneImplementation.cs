using BlApi;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private Dal.IDal _dal = DalApi.Factory.Get;

    public BO.Milestone? Create(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Milestone item)
    {
        throw new NotImplementedException();
    }
}
