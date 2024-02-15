using DalTest;
using System.Net.Mail;
namespace BlTest;
internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static DateTime? startProjectDate = null;  //the start project date 
    static bool chapter3 = false; //when it will be true create and delete task cannot be able anymore

    //get email and check his correctness
    internal static MailAddress getEmail()
    {
        while (true)
        {
            if (MailAddress.TryCreate(Console.ReadLine() ?? "", out MailAddress? res))
                return res;
            else Console.Write("Invalid MailAddress, plaese Enter Email: ");
        }    
    }

    //get date from user and check the input
    private static string? GetDate()
    {
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

    //get rnum from user and check the input
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

    //get Engineer's details
    static BO.Engineer GetEngineer()
    {
        Console.WriteLine("Enter Engineer's details:");
        Console.Write("ID: ");
        int id = int.Parse(Console.ReadLine());
        Console.Write("name: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Email: ");
        MailAddress mail = getEmail();
        Console.Write("cost: ");
        double price = double.Parse(Console.ReadLine());
        Console.Write("level, Rating between 1-5: ");
        BO.EngineerExperience level = (BO.EngineerExperience)(getEnum());
        Console.Write("task's ID: ");
        BO.TaskInEngineer taskInEngineer = new BO.TaskInEngineer();
        try
        {
            BO.Task ?task = s_bl.Task.Read(int.Parse(Console.ReadLine()));
            taskInEngineer = new BO.TaskInEngineer { Id = task.Id, Alias = task.Alias };
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }

        BO.Engineer item = new BO.Engineer { Id = id, Name = name, Email = mail.ToString(), Cost = price, Level = level, Task = taskInEngineer };
        return item;
    }

    //get task's details
    static BO.Task GetTask(bool update = false)
    {
        Console.WriteLine("Enter Task details:");
        if (update) Console.Write("ID: ");
        int id = (update == true) ? int.Parse(Console.ReadLine()) : 0;

        Console.Write("Alias: "); 
        string alias = Console.ReadLine() ?? "";

        Console.Write("Description: ");
        string description = Console.ReadLine() ?? "";

        Console.Write("press 1 to add dependecy or 0 to skip:");
        int? check=int.Parse(Console.ReadLine());
        List<BO.TaskInList> ? dependency = new List<BO.TaskInList>();
        while (check == 1)//get all the parameter for all the dependcies of this task 
        {
            Console.Write("Task's ID: "); int idTask = int.Parse(Console.ReadLine());
            try
            {
                BO.Task? dependentTask = s_bl.Task.Read(idTask);
                dependency.Add(new BO.TaskInList { Id = idTask, Description = dependentTask.Description, Alias = dependentTask.Alias, Status = dependentTask.Status ?? BO.Status.Unscheduled });
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
           
            Console.Write("press 1 to add dependecy or 0 to continue:");
            check = int.Parse(Console.ReadLine());
        }

        Console.Write("Required Effort Time (days): ");
        TimeSpan requiredEffortTime = TimeSpan.FromDays(double.Parse(Console.ReadLine()));

        Console.Write("Deliverables: ");
        string? deliverables = Console.ReadLine();

        Console.Write("Remarks: ");
        string? remark = Console.ReadLine();

        Console.Write("Enter Engineer's ID: ");
        BO.EngineerInTask engineerInTask=new BO.EngineerInTask();   
        try
        {
           BO.Engineer? engineer = s_bl.Engineer.Read(int.Parse(Console.ReadLine()));
           engineerInTask = new BO.EngineerInTask { Id = engineer.Id, Name = engineer.Name };
        }
        catch(Exception ex) { Console.WriteLine(ex.Message); }
        
        Console.Write("Enter Task's complexity, Rating between 1-5: ");
        BO.EngineerExperience complexity = (BO.EngineerExperience)(getEnum());

        BO.Task item = new BO.Task
        {
            Id = id,
            Alias = alias,
            Description = description,
            CreatedAtDate = DateTime.Now,
            Status = BO.Status.Unscheduled,
            Dependencies = dependency,
            Milestone = null,
            RequiredEffortTime = requiredEffortTime,
            StartDate = null,
            ScheduledDate = null,
            ForecastDate = null,
            DeadlineDate = null,
            Deliverables = deliverables,
            Remarks = remark,
            Engineer = engineerInTask,
            Copmlexity = complexity
        };
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
            Console.Write("Engineer's task: ");
            if (eng.Task == null) Console.WriteLine("Not specified");
            else Console.WriteLine($"\nID: {eng.Task.Id}, Alias: {eng.Task.Alias}");
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
            if (tsk.Dependencies == null) Console.WriteLine("Not specified");
            else foreach (var item in tsk.Dependencies)
                    Console.WriteLine($"ID: {item.Id}, Description: {item.Description}, Alias: {item.Alias}, Status: {item.Status}");
            Console.WriteLine($"Required Effort Time: {tsk.RequiredEffortTime?.ToString() ?? "Not specified"}");
            Console.WriteLine($"Start Date: {tsk.StartDate?.ToString() ?? "Not specified"}");
            Console.WriteLine($"Scheduled Date: {tsk.ScheduledDate?.ToString() ?? "Not specified"}");
            Console.WriteLine($"ForecastDate: {tsk.ForecastDate?.ToString() ?? "Not specified"}");
            Console.WriteLine($"Deadline Date: {tsk.DeadlineDate?.ToString() ?? "Not specified"}");
            Console.WriteLine($"Complete Date: {tsk.CompleteDate?.ToString() ?? "Not specified"}");
            Console.WriteLine($"Deliverables: {tsk.Deliverables ?? "Not specified"}");
            Console.WriteLine($"Remarks: {tsk.Remarks ?? "Not specified"}");
            Console.WriteLine("Engineer:");
            if (tsk.Engineer != null) Console.WriteLine($"ID: {tsk.Engineer.Id}, Name: {tsk.Engineer.Name}");
            else Console.WriteLine("Not specified");
            Console.WriteLine($"Engineer Complexity: {tsk.Copmlexity.ToString() ?? "Not specified"}");
        }
    }

    static int optionsSubMenu(string type, bool chapter3 = false) //Main sub menu options 
    {
        Console.WriteLine("Please press which action you want to take:");
        Console.WriteLine("0 - Exit");
        Console.WriteLine($"1 - Present an {type} by ID");
        Console.WriteLine($"2 - Display all {type}");
        if (chapter3 == false ) 
        {
            Console.WriteLine($"3 - Add a new {type}");
            Console.WriteLine($"4 - Update {type} data");
            Console.WriteLine($"5 - Delete an existing {type}");
            if (type == "Task")
            {
                Console.WriteLine($"6 - Update Task date");
            }
        }
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
                case 1: //read
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
                case 2: //readAll
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
                            Console.WriteLine();
                        }
                    break;
                case 3: //create
                    if (EngTask == 1)
                        try { s_bl.Engineer.Create(GetEngineer()); } //try to craete new Engineer
                        catch (Exception ex) { Console.WriteLine(ex.Message); } //ID is allredy exist
                    else if (EngTask == 2 && !chapter3 ) 
                        s_bl.Task.Create(GetTask());
                    break;
                case 4: //update
                    try
                    {
                        if (EngTask == 1)
                            s_bl.Engineer.Update(GetEngineer());
                        else if (EngTask == 2 && !chapter3)
                            s_bl.Task.Update(GetTask(true));
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
                        else if (EngTask == 2 && !chapter3)
                        {
                            Console.Write("Enter Task's ID: ");
                            s_bl.Task.Delete(int.Parse(Console.ReadLine()));
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                    break;
                case 6://update date
                    if (!chapter3)
                    {
                        Console.Write("Enter Task's ID: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.Write("Enter Task's Scheduled date: ");
                        string? tempDate = GetDate(); //recive and check date
                        if (tempDate == null)//the date didnt defined
                        {
                            Console.WriteLine("Date didn't defined");
                            break;
                        }
                        DateTime scheduledDate = DateTime.Parse(tempDate);
                        s_bl.Task.UpdateDate(id, scheduledDate);
                    }
                    break;

                default:  //if the user choose wrong number 
                    if(EngTask==1) Console.WriteLine("ERORR: choose numbers between 0-5");
                    else if (EngTask==2 && !chapter3) Console.WriteLine("ERORR: choose numbers between 0-6");
                    else Console.WriteLine("ERORR: choose numbers between 0-3");
                    userChoice = int.Parse(Console.ReadLine());
                    break;
            }
        } while (userChoice < 0 || userChoice > 6);
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
                Console.WriteLine("Press - 3 for Reset all dates");
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
                            int choice2 = optionsSubMenu("Task", chapter3);
                            choiceActivate(choice2, 2);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case 3:
                        Console.WriteLine("Enter the start date of the entire project: ");
                        string? checkDate = "1";
                        while (checkDate != "0")
                        {
                            checkDate = Console.ReadLine();
                            if (DateTime.TryParse(checkDate, out var date))
                            {
                                startProjectDate = date;
                                chapter3 = true;
                                checkDate = "0";
                            }
                            else Console.WriteLine("Invalid date. enter a date in the correct format, to exit press 0: ");
                        }
                        s_bl.Task.resetDate(startProjectDate??DateTime.Now);
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