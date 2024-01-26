using Dal;


namespace DalTest
{
    internal class Program
    {

        static readonly IDal s_dal = new DalXml(); //stage 3

        static int optionsSubMenu(string type)  //Main sub menu options 
        {
            Console.WriteLine("Please press which action you want to take:");
            Console.WriteLine("0 - Exit");
            Console.WriteLine($"1 - Add a new {type} to the list");
            Console.WriteLine($"2 - Present an {type} by ID");
            Console.WriteLine($"3 - Display all {type}");
            Console.WriteLine($"4 - Update {type} data");
            Console.WriteLine($"5 - Delete an existing {type} from a list");

            return int.Parse(Console.ReadLine());
        }

        /// <summary>
        /// get all parameter for Engineer
        /// </summary>
        static DO.Engineer GetEngineer()
        {
            Console.Write("Enter Engineer's details:");
            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("name: ");
            string name = Console.ReadLine() ?? "";
            Console.Write("Email: ");
            string mail = Console.ReadLine() ?? "";
            Console.Write("cost: ");
            double price = double.Parse(Console.ReadLine());
            Console.Write("Enter Engineer's level, Rating between 1-5: ");
            DO.EngineerExperience level = (DO.EngineerExperience)((int.Parse(Console.ReadLine()) - 1) % 5 + 1);

            DO.Engineer item = new DO.Engineer(id, mail, price, name, level);
            return item;
        }

        /// <summary>
        /// get all parameter for Task
        /// </summary>
        static DO.Task GetTask()
        {
            Console.WriteLine("Enter Task details:");

            Console.Write("Alias: ");
            string alias = Console.ReadLine() ?? "";

            Console.Write("Description: ");
            string description = Console.ReadLine() ?? "";

            Console.Write("Is Milestone (true/false): ");
            string? isMilestoneCheck = Console.ReadLine();
            bool isMilestone = false;
            if (isMilestoneCheck == "1" || isMilestoneCheck == "true")
                isMilestone = true;

            Console.Write("Required Effort Time (days): ");
            TimeSpan requiredEffortTime = TimeSpan.FromDays(double.Parse(Console.ReadLine()));

            Console.Write("Scheduled Date (in the format dd/mm/yyyy): "); //receive Scheduled Date (additional)
            string? startDateInput = Console.ReadLine();
            DateTime? ScheduledDate = null;
            do
            {
                if (DateTime.TryParse(startDateInput, out var date))//date was good
                {
                    ScheduledDate = DateTime.Parse(startDateInput);
                    break;
                }
                else//user enter invalid date
                {
                    Console.Write("Invalid date. enter a date in the correct format, to continue without Scheduled Date press 0: ");
                    startDateInput = Console.ReadLine();
                }
            } while (startDateInput != "0");//Continues as long as an invalid date is entered

            Console.Write("DeadLine Date (in the format dd/mm/yyyy): ");  //receive deadline Date (additional)
            DateTime? deadLine = null;
            string? dateOfEnding = Console.ReadLine();
            do
            {
                if (DateTime.TryParse(dateOfEnding, out var date))//date was good
                {
                    deadLine = DateTime.Parse(dateOfEnding);
                    break;
                }
                else//user enter invalid date
                {
                    Console.Write("Invalid date. enter a date in the correct format, to continue without Deadline press 0: ");
                    dateOfEnding = Console.ReadLine();
                }
            } while (dateOfEnding != "0");//Continues as long as an invalid date is entered

            Console.Write("Enter Task's complexity, Rating between 1-5: ");
            DO.EngineerExperience complexity = (DO.EngineerExperience)(int.Parse(Console.ReadLine()) % 5 + 1);

            Console.Write("Deliverables: ");
            string? deliverables = Console.ReadLine();

            Console.Write("Engineer ID: ");
            int engineerID = int.Parse(Console.ReadLine());

            DO.Task item = new DO.Task(0, alias, description, DateTime.Now, isMilestone, requiredEffortTime,
                complexity, null, ScheduledDate, deadLine, null, deliverables, null, engineerID); ;
            return item;
        }

        /// /// <summary>
        /// get all parameter for Dependency
        /// </summary>
        static DO.Dependency GetDependency()
        {
            Console.WriteLine("Enter Dependency details:");
            Console.Write("Dependent Task: ");
            int dependentTask = Console.Read();
            Console.Write("\nDependent on Task: ");
            int dependentOnTask = int.Parse(Console.ReadLine());

            DO.Dependency item = new DO.Dependency(0, dependentTask, dependentOnTask);
            return item;
        }

        /// <summary>
        /// print all Engineer parameter
        /// </summary>
        static void printEng(DO.Engineer? eng)
        {
            if (eng != null)
            {
                Console.WriteLine($"Engineer name: {eng.Name}");
                Console.WriteLine($"Engineer ID: {eng.Id}");
                Console.WriteLine($"Engineer mail: {eng.Email}");
                Console.WriteLine($"Engineer price: {eng.Cost}");
                Console.WriteLine($"Engineer level:{eng.Level}");
            }
        }

        /// <summary>
        ///  print all Task parameter
        /// </summary>
        static void printTask(DO.Task? tsk)
        {
            if (tsk != null)
            {
                Console.WriteLine($"Task ID: {tsk.Id}");
                Console.WriteLine($"Alias: {tsk.Alias}");
                Console.WriteLine($"Description: {tsk.Description}");
                Console.WriteLine($"Created At Date: {tsk.CreatedAtDate}");
                Console.WriteLine($"Is Milestone: {tsk.IsMilestone}");
                Console.WriteLine($"Required Effort Time: {tsk.RequiredEffortTime?.ToString() ?? "Not specified"}");
                Console.WriteLine($"Engineer Complexity: {tsk.Copmlexity?.ToString() ?? "Not specified"}");
                Console.WriteLine($"Start Date: {tsk.StartDate?.ToString() ?? "Not specified"}");
                Console.WriteLine($"Scheduled Date: {tsk.ScheduledDate?.ToString() ?? "Not specified"}");
                Console.WriteLine($"Deadline Date: {tsk.DeadlineDate?.ToString() ?? "Not specified"}");
                Console.WriteLine($"Complete Date: {tsk.CompleteDate?.ToString() ?? "Not specified"}");
                Console.WriteLine($"Deliverables: {tsk.Deliverables ?? "Not specified"}");
                Console.WriteLine($"Remarks: {tsk.Remarks ?? "Not specified"}");
                Console.WriteLine($"Engineer ID: {tsk.EngineerId?.ToString() ?? "Not specified"}");
            }
        }

        /// <summary>
        ///  print all Dependency parameter
        /// </summary>
        static void printDep(DO.Dependency? dep)
        {
            if (dep != null)
            {
                Console.WriteLine($"Dependency ID: {dep.Id}");
                Console.WriteLine($"Dependent Task: {dep.DependentTask}");
                Console.WriteLine($"Depends On Task: {dep.DependsOnTask}");
            }
        }

        /// <summary>
        /// get from user his 2 choices, userChoice - what action to do, EngTaskDep - Engineer/Task/Dependsy
        /// </summary>
        static int choiceActivate(int userChoice, int EngTaskDep)//userChoice - what action to do, EngTaskDep meens one of: engineer = 1, task = 2, dependecy = 3
        {
            do
            {
                switch (userChoice)
                {
                    case 0:     //exit
                        break;
                    case 1: //creat 
                        if (EngTaskDep == 1)
                            try { s_dal.Engineer.Create(GetEngineer()); } //try to craete new Engineer
                            catch (Exception ex) { Console.WriteLine(ex.Message); } //ID is allredy exist
                        else if (EngTaskDep == 2)
                            s_dal.Task.Create(GetTask());
                        else if (EngTaskDep == 3)
                            s_dal.Dependency.Create(GetDependency());
                        break;
                    case 2: //read
                        if (EngTaskDep == 1)
                        {
                            Console.Write("Enter Engineer's ID: ");
                            printEng(s_dal.Engineer.Read(int.Parse(Console.ReadLine())));
                        }
                        else if (EngTaskDep == 2)
                        {
                            Console.Write("Enter Task's ID: ");
                            printTask(s_dal.Task.Read(int.Parse(Console.ReadLine())));
                        }
                        else if (EngTaskDep == 3)
                        {
                            Console.Write("Enter Dependency's ID: ");
                            printDep(s_dal.Dependency.Read(int.Parse(Console.ReadLine())));
                        }
                        break;
                    case 3: //read all
                        if (EngTaskDep == 1)
                            foreach (var item1 in s_dal.Engineer.ReadAll())
                            {
                                printEng(item1);
                                Console.WriteLine();
                            }
                        else if (EngTaskDep == 2)
                            foreach (var item2 in s_dal.Task.ReadAll())
                            {
                                printTask(item2);
                                Console.WriteLine();
                            }

                        else if (EngTaskDep == 3)
                            foreach (var item3 in s_dal.Dependency.ReadAll())
                            {
                                printDep(item3);
                                Console.WriteLine();
                            }
                        break;
                    case 4: //update
                        try
                        {
                            if (EngTaskDep == 1)
                                s_dal.Engineer.Update(GetEngineer());
                            else if (EngTaskDep == 2)
                                s_dal.Task.Update(GetTask());
                            else if (EngTaskDep == 3)
                                s_dal.Dependency.Update(GetDependency());
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case 5: //delete
                        try
                        {
                            if (EngTaskDep == 1)
                            {
                                Console.Write("Enter Engineer's ID: ");
                                s_dal.Engineer.Delete(int.Parse(Console.ReadLine()));
                            }
                            else if (EngTaskDep == 2)
                            {
                                Console.Write("Enter Task's ID: ");
                                s_dal.Task.Delete(int.Parse(Console.ReadLine()));
                            }
                            else if (EngTaskDep == 3)
                            {
                                Console.Write("Enter Dependency's ID: ");
                                s_dal.Dependency.Delete(int.Parse(Console.ReadLine()));
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

        static void Main(string[] args)
        {
            try
            {
                int i = 0;

                do
                {
                    Console.WriteLine("Press - 0 for exit");
                    Console.WriteLine("Press - 1 for Engineers");
                    Console.WriteLine("Press - 2 for Tasks");
                    Console.WriteLine("Press - 3 for Dependencies");
                    Console.WriteLine("Press - 4 for Initialize data");
                    i = int.Parse(Console.ReadLine());
                    switch (i)
                    {
                        case 0:
                            break;
                        case 1:     //engineer
                            int choice1 = optionsSubMenu("Engineer");
                            choiceActivate(choice1, 1);
                            break;
                        case 2:     //task
                            int choice2 = optionsSubMenu("Task");
                            choiceActivate(choice2, 2);
                            break;
                        case 3:     //dependsy
                            int choice3 = optionsSubMenu("Dependency");
                            choiceActivate(choice3, 3);
                            break;
                        case 4:
                            Console.Write("Would you like to create Initial data? (Y/N) "); //stage 3
                            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                            if (ans == "Y")
                            {
                                Initialization.initialize();
                                Initialization.Do(s_dal);
                            }
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
}