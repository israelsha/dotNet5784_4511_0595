namespace BO;
 
public enum Status
{
  Unscheduled,  //didn't start yet at all
  Scheduled,    //planed but the task didn't started 
  OnTrack,      //in he middle of the task
  InJeopardy,
  Done          //mission complate
}

public enum EngineerExperience
{
  Beginner,
  AdvancedBeginner,
  Intermediate,
  Advanced,
  Expert,
  None       
}