
using System.Diagnostics.Contracts;

namespace BO;

enum Status
{
  Unscheduled,
  Scheduled,
  OnTrack,
  InJeopardy,
  Done
}

enum EngineerExperience {
  Beginner,
  AdvancedBeginner,
  Intermediate,
  Advanced,
  Expert
}