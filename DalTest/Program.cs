﻿using Dal;
using DalApi;
using DO;
using System.Data.Common;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DalTest
{
    internal class Program
    {
        private static ITask? s_dalTask = new TaskImplementation();
        private static IEngineer? s_dalEngineer = new EngineerImplementation();
        private static IDependency? s_dalDependency = new DependencyImplementation();
       
        static int optionsSubMenu(string type)  //Main sub menu options 
        {
            Console.WriteLine("Select the method you want to perform");
            Console.WriteLine("to exsit Press 1");
            Console.WriteLine($"To add a new {type} to the list press 2");
            Console.WriteLine($"To present an {type} by ID press 3");
            Console.WriteLine($"To display all {type} press 4");
            Console.WriteLine($"To update {type} data press 5");
            Console.WriteLine($"To delete an existing {type} from a list press 6");

            return int.Parse(Console.ReadLine());
        }

        /// <summary>
        /// get all parameter for Engineer
        /// </summary>
        /// <returns></returns>
        static DO.Engineer GetEngineer()
        {
            Console.Write("Enter Engineer's details:");
            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("name: ");
            string name=Console.ReadLine() ?? "";
            Console.Write("Email: ");
            string mail = Console.ReadLine() ?? "";
            Console.Write("cost: ");
            double price = double.Parse(Console.ReadLine());
            Console.Write("Enter Engineer's level, Rating between 1-5: ");
            DO.EngineerExperience level=(DO.EngineerExperience)(int.Parse(Console.ReadLine()) % 5+1);

            DO.Engineer item = new DO.Engineer(id, mail, price, name, level);
            return item;    
        }

        /// <summary>
        /// get all parameter for Task
        /// </summary>
        /// <returns></returns>
        static DO.Task GetTask()
        {
            Console.WriteLine("Enter Task details:");

            Console.Write("Alias: ");
            string alias = Console.ReadLine()??"";

            Console.Write("\nDescription: ");
            string description = Console.ReadLine() ?? "";

            Console.Write("\nIs Milestone (true/false): ");
            bool isMilestone = bool.Parse(Console.ReadLine());

            
            Console.Write("\nRequired Effort Time (days): ");
            TimeSpan requiredEffortTime = TimeSpan.FromDays(double.Parse(Console.ReadLine()));

            Console.Write("\nScheduled Date (in the format dd/mm/yyyy): "); //receive Scheduled Date (additional)
            string? startDateInput = Console.ReadLine();
            DateTime? ScheduledDate = null;
            do
            {
                if (DateTime.TryParse(startDateInput, out var date))
                {
                    ScheduledDate = DateTime.Parse(startDateInput);
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid date. enter a date in the correct format, to continue without Scheduled Date press 0");
                    startDateInput = Console.ReadLine();
                }
            } while (startDateInput != "0");


            Console.Write("\nDeadLine Date (in the format dd/mm/yyyy): ");  //receive deadline Date (additional)
            DateTime? deadLine = null;
            string? dateOfEnding = Console.ReadLine();
            do
            {
                if (DateTime.TryParse(dateOfEnding, out var date))
                {
                    deadLine = DateTime.Parse(dateOfEnding);
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid date. enter a date in the correct format, to continue without Deadline press 0");
                    dateOfEnding = Console.ReadLine();
                }
            } while (dateOfEnding != "0");


            Console.WriteLine("\nEnter Task's complexity, Rating between 1-5: ");
            DO.EngineerExperience complexity = (DO.EngineerExperience)(int.Parse(Console.ReadLine()) % 5 + 1);

            Console.Write("\nDeliverables: ");
            string? deliverables = Console.ReadLine();

            Console.Write("\nEngineer ID: ");
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
            int dependentTask= Console.Read();
            Console.Write("\nDependent on Task: ");
            int dependentOnTask = int.Parse(Console.ReadLine());

            DO.Dependency item = new DO.Dependency(0,dependentTask,dependentOnTask);
            return item;
        }

        /// <summary>
        /// print all Engineer parameter
        /// </summary>
        /// <param name="eng"></param>
        static void printEng(DO.Engineer? eng)
        {
            if(eng != null)
            {
                Console.WriteLine($"Engineer name: { eng.Name}");
                Console.WriteLine($"Engineer ID: {eng.Id}");
                Console.WriteLine($"Engineer mail: {eng.Email}");
                Console.WriteLine($"Engineer price: {eng.Cost}");
                Console.WriteLine($"Engineer level:{eng.Level}");
            }
        }

        /// <summary>
        ///  print all Task parameter
        /// </summary>
        /// <param name="tsk"></param>
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
        /// <param name="dep"></param>
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
        /// get from user his 2 choices, choice1 - what action to do, choice2 - Engineer/Task/Dependsy
        /// </summary>
               static int choiceActivate(int choice1 ,int choice2)
        {
            do
            {
                switch (choice1)
                {
                    case 1:     //exit
                        break;
                    case 2:     //creat 
                        if (choice2 == 1)
                            s_dalEngineer.Create(GetEngineer());
                        else if (choice2 == 2)
                            s_dalTask.Create(GetTask());
                        else if (choice2 == 3)
                            s_dalDependency.Create(GetDependency());   
                            break;
                    case 3:     //read
                        if (choice2 == 1)
                        {
                            Console.Write("Enter Engineer's ID: ");
                            printEng(s_dalEngineer.Read(int.Parse(Console.ReadLine())));
                        }
                        else if (choice2 == 2)
                        {
                            Console.Write("Enter Task's ID: ");
                            printTask(s_dalTask.Read(int.Parse(Console.ReadLine())));
                        }
                        else if (choice2 == 3)
                        {
                            Console.Write("Enter Dependency's ID: ");
                            printDep(s_dalDependency.Read(int.Parse(Console.ReadLine())));
                        }
                        break;
                    case 4:      //read all
                        if (choice2 == 1)
                            foreach (var item1 in s_dalEngineer.ReadAll())
                            {
                                printEng(item1);
                                Console.WriteLine();
                            }
                        else if (choice2 == 2)
                            foreach (var item2 in s_dalTask.ReadAll())
                            {
                                printTask(item2);
                                Console.WriteLine();
                            }

                        else if (choice2 == 3)
                            foreach (var item3 in s_dalDependency.ReadAll())
                            {
                                printDep(item3);
                                Console.WriteLine();
                            }
                        break;
                    case 5:     //update
                        if (choice2 == 1)
                            s_dalEngineer.Update(GetEngineer());
                        else if (choice2 == 2)
                            s_dalTask.Update(GetTask());
                        else if (choice2 == 3)
                            s_dalDependency.Update(GetDependency());
                        break;
                    case 6:    // delete
                        if (choice2 == 1)
                        {
                            Console.Write("Enter Engineer's ID: ");
                            s_dalEngineer.Delete(int.Parse(Console.ReadLine()));
                        }
                        else if (choice2 == 2)
                        {
                            Console.Write("Enter Task's ID: ");
                            s_dalTask.Delete(int.Parse(Console.ReadLine()));
                        }
                        else if (choice2 == 3)
                        {
                            Console.Write("Enter Dependency's ID: ");
                            s_dalDependency.Delete(int.Parse(Console.ReadLine()));
                        }
                        break;
                    default:    //if the user choose wrong number 
                        Console.WriteLine("ERORR: choose numbers betwin 1-6");
                        choice1 = int.Parse(Console.ReadLine());
                        break;
                }
            } while (choice1 < 1 || choice1 > 6);

            return 1;
        }

        static void Main(string[] args)
        {
            try
            {
                Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency);
                int i = 0;
               
                do
                {
                    Console.WriteLine("\nto exit press 0");
                    Console.WriteLine("for Engineers Press 1");
                    Console.WriteLine("for Tasks Press 2");
                    Console.WriteLine("for Dependencies Press 3");
                    i = int.Parse(Console.ReadLine());
                        switch (i)
                        {
                        case 0:
                            break;
                        case 1:     //engineer
                            int choice1= optionsSubMenu("engineer");
                            choiceActivate(choice1, 1);
                            break;
                        case 2:     //task
                            int choice2 = optionsSubMenu("task");
                            choiceActivate(choice2, 2);
                            break;
                        case 3:     //dependsy
                            int choice3 = optionsSubMenu("Dependency");
                            choiceActivate(choice3, 3);
                            break;
                        default:   //if the user choose wrong number 
                            Console.WriteLine("ERROR: choose number between 1-3");
                            i = int.Parse(Console.ReadLine());  
                            break;                            
                          }
                } while (i != 0);


            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex);
            }
        }
    }
}
