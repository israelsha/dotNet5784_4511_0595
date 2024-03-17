using BO;
using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

public class DatesImplementation : IDates
{

    private Dal.IDal _dal = DalApi.Factory.Get;

    static readonly IBl s_bl =Factory.Get();

    public DateTime? getStartProject()=>_dal.Dates.getStartProject();   
    
    public DateTime? setStartProject(DateTime? startProject)=>_dal.Dates.setStartProject(startProject);

    public DateTime? getEndProject() => _dal.Dates.getStartProject();

    public DateTime? setEndProject(DateTime? endProject) => _dal.Dates.setEndProject(endProject);


    //external function  to reset all the ScheduledDate and the deadline of all the task 
    public void resetDate(DateTime startProject)
    {
        //find all the task that is not depend on any other task ie: task.Dependencies = null
        IEnumerable<BO.Task> notDependentTask = from doTask in _dal.Task.ReadAll()
                                                let boTask = s_bl.Task.Read(doTask.Id)
                                                where boTask.Dependencies == null || boTask.Dependencies.Count() == 0
                                                select boTask;
        reset(startProject, notDependentTask);

        DateTime? endProject = startProject;
        foreach (var task in _dal.Task.ReadAll())
        {
            if (endProject < task!.DeadlineDate)
                endProject = task.DeadlineDate;
        }
        _dal.Dates.setEndProject(endProject);
    }

    //recursive function, reset all the ScheduledDate and the deadline of all the task
    public void reset(DateTime? prevDate, IEnumerable<BO.Task>? tasks)
    {
        if (tasks != null)
            foreach (var item in tasks)
            {
                //update the task whit the correct ScheduledDate and DeadlineDate
                _dal.Task.Update(Tools.boToDo(item) with
                {
                    ScheduledDate = (item.ScheduledDate == null || item.ScheduledDate < prevDate) ? prevDate : item.ScheduledDate,
                    DeadlineDate = ((item.ScheduledDate != null) ? item.ScheduledDate : prevDate) + item.RequiredEffortTime
                });
                //sending the tasks that is depending on this task
                reset(prevDate + item.RequiredEffortTime, from dep in _dal.Dependency.ReadAll()
                                                          where dep.DependsOnTask != null && dep.DependsOnTask == item.Id
                                                          select s_bl.Task.Read(dep.DependentTask ?? 0));
            }
    }
}
