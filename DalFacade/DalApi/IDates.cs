namespace Dal;

public interface IDates
{
   public DateTime? setStartProject(DateTime? startProject);
   public DateTime? getStartProject();

   public DateTime? setEndProject(DateTime? endProject);
    public DateTime? getEndProject();
}