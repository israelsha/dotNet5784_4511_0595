using BO;
using Dal;
using DalTest;
using System.Collections.Generic;
using System.Threading.Channels;

namespace BlTest;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    private static string? GetDate()
    {
        Console.Write("to skip press 0: ");
        string? startDateInput = Console.ReadLine();
        while (startDateInput != "0")
        {
            if (DateTime.TryParse(startDateInput, out var date))//date was good
                return startDateInput;
            else//user enter invalid date
            {
                Console.Write($"Invalid date. enter a date in the correct format, to skip press 0: ");
                startDateInput = Console.ReadLine();
            }
        } ;//Continues as long as an invalid date is entered
        return null;
    }

    private static int getEnum()
    {
        int statusCheck = int.Parse(Console.ReadLine());
        while (statusCheck < 1 || statusCheck > 5)
        {
            Console.Write("ERROR: Rating between 1-5");
            statusCheck = int.Parse(Console.ReadLine());
        }
        return statusCheck-1; 
    }

   static BO.Engineer GetEngineer()
    {
        Console.WriteLine("Enter Engineer's details:");
        Console.Write("ID: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("name: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Email: ");
        string mail = Console.ReadLine() ?? "";
        Console.Write("cost: ");
        double price = double.Parse(Console.ReadLine());
        Console.Write("level, Rating between 1-5: ");
        BO.EngineerExperience level = (BO.EngineerExperience)(getEnum());
        Console.WriteLine("Engineer's task:");
        Console.Write("Task's ID: "); int taskId = int.Parse(Console.ReadLine());
        Console.Write("Task's alias: "); string taskAlias = Console.ReadLine() ?? "";
        BO.TaskInEngineer taskInEngineer = new BO.TaskInEngineer { Id = taskId, Alias = taskAlias };

        BO.Engineer item = new BO.Engineer { Id = id, Name = name, Email = mail, Cost = price, Level = level, Task = taskInEngineer };
        return item;
    }

    //get task details
    static BO.Task GetTask()
    {
        Console.WriteLine("Enter Task details:");
        Console.Write("ID: ");
        int id = int.Parse(Console.ReadLine());

        Console.Write("Alias: "); 
        string alias = Console.ReadLine() ?? "";

        Console.Write("Description: ");
        string description = Console.ReadLine() ?? "";

        Console.Write("Status, Rating between 1-5: ");
        BO.Status status = (BO.Status)(getEnum());

        Console.WriteLine("press 1 to add dependecy or 0 to skip:");
        int? check=int.Parse(Console.ReadLine());
        List<BO.TaskInList> ? dependency = new List<BO.TaskInList>();
        while (check == 1)//get all the parameter for all the dependcies of this task 
        {
            Console.Write("ID: "); int idTask = int.Parse(Console.ReadLine());
            Console.Write("Description: "); string descruptionTask = Console.ReadLine();
            Console.Write("Alias: "); string aliasTask = Console.ReadLine();
            Console.Write("Status: "); BO.Status statusTask = (BO.Status)(getEnum());
            dependency.Add(new BO.TaskInList { Id = idTask, Description = descruptionTask, Alias = aliasTask, Status = statusTask });
            Console.WriteLine("press 1 to add dependecy or 0 to continue:");
            check = int.Parse(Console.ReadLine());
        }

        Console.Write("Required Effort Time (days): ");
        TimeSpan requiredEffortTime = TimeSpan.FromDays(double.Parse(Console.ReadLine()));

        Console.Write("Start date (in the format dd/mm/yyyy): ");
        string? tempDate = GetDate();//recive and check date
        DateTime? startDate = (tempDate == null) ? null : DateTime.Parse(tempDate);

        Console.Write("Scheduled date (in the format dd/mm/yyyy): "); //receive Scheduled Date (additional)
        tempDate = GetDate();//recive and check date
        DateTime? scheduledDate = (tempDate == null) ? null : DateTime.Parse(tempDate);

        Console.Write("DeadLine date (in the format dd/mm/yyyy): ");  //receive deadline Date (additional)
        tempDate=GetDate();//recive and check date
        DateTime? deadLine = (tempDate==null)?null:DateTime.Parse(tempDate);

        Console.Write("Complete date (in the format dd/mm/yyyy): ");
        tempDate = GetDate();
        DateTime? completeDate = (tempDate == null) ? null : DateTime.Parse(tempDate);

        Console.Write("Deliverables: ");
        string? deliverables = Console.ReadLine();

        Console.Write("Remarks: ");
        string? remark = Console.ReadLine();

        Console.WriteLine("Enter Engineer's details: ");
        Console.Write("ID: ");int idEngneer=int.Parse(Console.ReadLine());
        Console.Write("Name: "); string nameEngineer = Console.ReadLine();
        BO.EngineerInTask? engineer = new BO.EngineerInTask { Id = idEngneer, Name = nameEngineer };

        Console.Write("Enter Task's complexity, Rating between 1-5: ");
        BO.EngineerExperience complexity = (BO.EngineerExperience)(getEnum());

        BO.Task item = new BO.Task{Id=id, Alias= alias,Description= description, CreatedAtDate= DateTime.Now,
            Status= status , Dependencies= dependency,Milestone= null,  RequiredEffortTime = requiredEffortTime,
            StartDate=startDate,ScheduledDate=scheduledDate,ForecastDate=scheduledDate+requiredEffortTime,DeadlineDate=deadLine,
            Deliverables=deliverables,Remarks=remark,Engineer= engineer, Copmlexity = complexity } ;
        return item;
    }

    static void printEng(BO.Engineer? eng)
    {
        if (eng != null)
        {
            Console.WriteLine($"Engineer's name: {eng.Name}");
            Console.WriteLine($"Engineer's ID: {eng.Id}");
            Console.WriteLine($"Engineer's mail: {eng.Email}");
            Console.WriteLine($"Engineer's price: {eng.Cost}");
            Console.WriteLine($"Engineer's level:{eng.Level}");
            Console.WriteLine($"Engineer's task:\nID: {eng.Task.Id}, Alias: {eng.Task.Alias}");
        }
    }

    static void printTask(BO.Task? tsk)
    {
        if (tsk != null)
        {
            Console.WriteLine($"Task ID: {tsk.Id}");
            Console.WriteLine($"Description: {tsk.Description}");
            Console.WriteLine($"Alias: {tsk.Alias}");
            Console.WriteLine($"Created At Date: {tsk.CreatedAtDate}");
            Console.WriteLine($"Status: {tsk.Status}");
            Console.WriteLine("Dependencies:");
            foreach (var item in tsk.Dependencies)
                Console.WriteLine($"ID: {item.Id}, Description: {item.Description}, Alias: {item.Alias}, Status: {item.Status}");
            Console.WriteLine($"Required Effort Time: {tsk.RequiredEffortTime?.ToString() ?? "Not specified"}");
            Console.WriteLine($"Start Date: {tsk.StartDate?.ToString() ?? "Not specified"}");
            Console.WriteLine($"Scheduled Date: {tsk.ScheduledDate?.ToString() ?? "Not specified"}");
            Console.WriteLine($"ForecastDate: {tsk.ForecastDate?.ToString() ?? "Not specified"}");
            Console.WriteLine($"Deadline Date: {tsk.DeadlineDate?.ToString() ?? "Not specified"}");
            Console.WriteLine($"Complete Date: {tsk.CompleteDate?.ToString() ?? "Not specified"}");
            Console.WriteLine($"Deliverables: {tsk.Deliverables ?? "Not specified"}");
            Console.WriteLine($"Remarks: {tsk.Remarks ?? "Not specified"}");
            if (tsk.Engineer is not null)
            {
                Console.WriteLine("Engineer:"); Console.WriteLine($"ID: {tsk.Engineer.Id}, Name: {tsk.Engineer.Name}");
            }
            Console.WriteLine($"Engineer Complexity: {tsk.Copmlexity.ToString() ?? "Not specified"}");
        }
    }

    static int optionsSubMenu(string type)  //Main sub menu options 
    {
        Console.WriteLine("Please press which action you want to take:");
        Console.WriteLine("0 - Exit");
        Console.WriteLine($"1 - Add a new {type}");
        Console.WriteLine($"2 - Present an {type} by ID");
        Console.WriteLine($"3 - Display all {type}");
        Console.WriteLine($"4 - Update {type} data");
        Console.WriteLine($"5 - Delete an existing {type}");

        return int.Parse(Console.ReadLine());
    }

    //userChoice - what action to do, EngTask meens one of: engineer = 1, task = 2
    static int choiceActivate(int userChoice, int EngTask)
    {
        do
        {
            switch (userChoice)
            {
                case 0:     //exit
                    break;
                case 1: //create
                    if (EngTask == 1)
                        try { s_bl.Engineer.Create(GetEngineer()); } //try to craete new Engineer
                        catch (Exception ex) { Console.WriteLine(ex.Message); } //ID is allredy exist
                    else if (EngTask == 2)
                        s_bl.Task.Create(GetTask());
                    break;
                case 2: //read
                    if (EngTask == 1)
                    {
                        Console.Write("Enter Engineer's ID: ");
                        printEng(s_bl.Engineer.Read(int.Parse(Console.ReadLine())));
                    }
                    else if (EngTask == 2)
                    {
                        Console.Write("Enter Task's ID: ");
                        printTask(s_bl.Task.Read(int.Parse(Console.ReadLine())));
                    }
                    break;
                case 3: //read all
                    if (EngTask == 1)
                        foreach (var item1 in s_bl.Engineer.ReadAll())
                        {
                            printEng(item1);
                            Console.WriteLine();
                        }
                    else if (EngTask == 2)
                        foreach (var item2 in s_bl.Task.ReadAll())
                        {
                            Console.WriteLine($"Task ID: {item2.Id}");
                            Console.WriteLine($"Description: {item2.Description}");
                            Console.WriteLine($"Alias: {item2.Alias}");
                            Console.WriteLine($"Status: {item2.Status}");
                        }
                    break;
                case 4: //update
                    try
                    {
                        if (EngTask == 1)
                            s_bl.Engineer.Update(GetEngineer());
                        else if (EngTask == 2)
                            s_bl.Task.Update(GetTask());
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                    break;
                case 5: //delete
                    try
                    {
                        if (EngTask == 1)
                        {
                            Console.Write("Enter Engineer's ID: ");
                            s_bl.Engineer.Delete(int.Parse(Console.ReadLine()));
                        }
                        else if (EngTask == 2)
                        {
                            Console.Write("Enter Task's ID: ");
                            s_bl.Task.Delete(int.Parse(Console.ReadLine()));
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                    break;
                default:  //if the user choose wrong number 
                    Console.WriteLine("ERORR: choose numbers betwin 1-6");
                    userChoice = int.Parse(Console.ReadLine());
                    break;
            }
        } while (userChoice < 0 || userChoice > 5);

        return 1;
    }

    static void Main()
    {
        Console.Write("Would you like to create Initial data? (Y/N) ");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
        {
            Initialization.initialize();
            DalTest.Initialization.Do();
        }
        try
        {
            int i = 0;
            do
            {
                Console.WriteLine("Press - 0 for exit");
                Console.WriteLine("Press - 1 for Engineers");
                Console.WriteLine("Press - 2 for Tasks");
                i = int.Parse(Console.ReadLine());
                switch (i)
                {
                    case 0:
                        break;
                    case 1:     //engineer
                        try
                        {
                            int choice1 = optionsSubMenu("Engineer");
                            choiceActivate(choice1, 1);
                        }
                        catch(Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case 2:     //task
                        try
                        {
                            int choice2 = optionsSubMenu("Task");
                            choiceActivate(choice2, 2);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    default:   //if the user choose wrong number 
                        Console.WriteLine("ERROR: choose number between 1-3");
                        i = int.Parse(Console.ReadLine());
                        break;
                }
                Console.WriteLine();
            } while (i != 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
}
