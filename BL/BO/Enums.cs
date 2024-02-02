namespace BO;
 
public enum Status
{
  Unscheduled,  //didnt start yet at all
  Scheduled,    //planed but the task didnt started 
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
  Expert
}