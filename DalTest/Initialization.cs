
namespace DalTest;
using DalApi;
using DO;
using System;
using Dal;

public static class Initialization
{
    private static ITask? s_dalTask;             //stage 1
    private static IEngineer? s_dalEngineer;     //stage 1
    private static IDependency? s_dalDependency; //stage 1

    private static readonly Random s_rand = new();

    private static void createTasks()
    {
        string[] _Alias =
        {
            "Project Management", "Data Processing", "System Integration",
            "Quality Assurance", "R&D (Research & Development)", "Prototyping",
            "Testing", "Algorithm Development", "Network Management", "Cybersecurity",
            "Software Development", "Hardware Design", "Environmental Assessment",
            "Structural Design", "Energy Optimization", "Process Automation",
            "Supply Chain Management", "Risk Management", "Human Factors Engineering",
            "Renewable Energy Solutions"
        };
        string[] DescriptionTasks = new string[]
        {
          "Planning, controlling, and leading a project from its beginning through to completion.",
          "Focus on handling and processing data as part of organizational processes.",
          "Combining various components or systems to create a unified and efficient operation.",
          "Overseeing processes and outputs to ensure high-quality and reliable results.",
          "Activity involving search, discovery, and technological or knowledge development.",
          "Creating initial or scaled models used for experimentation and testing.",
          "Processes of evaluating the functionality or quality of systems and software.",
          "Creating sequences of computational steps to solve a specific problem.",
          "Administration and control of computer networks.",
          "Protection against information and computer security threats.",
          "Creating software applications tailored to client needs.",
          "Planning and developing hardware components.",
          "Studying and evaluating the impact of activities on the environment.",
          "Planning and constructing architectural structures.",
          "Creating systems that are integrated into larger systems or products.",
          "Implementing processes to operate machinery or systems with minimal human intervention.",
          "Applying principles of engineering to healthcare and medical fields.",
          "Designing and managing telecommunication systems.",
          "Developing algorithms and models for machine learning applications.",
           "Designing, building, and maintaining robots for various applications."
        };
        int[] arrDifficulty = new int[] { 1, 2, 4, 3, 3, 3, 4, 5, 3, 5, 4, 4, 2, 2, 2, 3, 2, 1, 1, 4 };
        string[] prudoctTasks = new string[]
        {
            "Detailed project plan","Automated data processing system","Integration process documentation",
            "Quality testing reports","Research reports with findings and recommendations","Physical or digital prototype",
            "Test results and notes","Source code for algorithms","Reports on cyber attacks and solutions","Source code for the software"
            ,"Report analyzing environmental impacts","Structural design plans","Plan to reduce energy consumption",
            "Automated process system","Supply chain management plan","List of risks and workflow diagrams","Report on human-system interface design"
            ,"Renewable energy solution proposals","Detailed project plan"
        };
        string[] RamarksTasks = new string[]
        {
           "Efficient resource and timeline organization for project success.","Automated handling of data for accurate and efficient results."
           ,"","Rigorous testing and continuous improvement for high-quality standards.","","Creation of preliminary models for testing and refinement."
            ,"Thorough examination of systems to identify and rectify issues.","","Oversight and optimization of network infrastructure.",
            "Protection against cyber threats through robust security measures.","","","Planning and drafting designs for physical structures."
            ,"Strategies to reduce energy consumption and enhance efficiency.","Implementation of automated systems for process efficiency.",
            "","Identification, assessment, and mitigation of potential risks.","Designing systems considering human interaction for optimal usability.",""
        };
        Random rnd = new Random();
        for (int i = 0; i < _Alias.Length; i++)
        {
            DateTime _CreatedAtDate = DateTime.Now;
            DateTime? _StartDate = _CreatedAtDate + TimeSpan.FromDays(rnd.Next(1, 100)) + TimeSpan.FromHours(rnd.Next(0, 24)) + TimeSpan.FromSeconds((rnd.Next(0, 3600)));
            bool _IsMilestone = false;
            TimeSpan? _RequiredEffortTime = TimeSpan.FromDays(rnd.Next(20, 100));
            DO.EngineerExperience? _Copmlexity = (EngineerExperience)arrDifficulty[i];

            DateTime? _ScheduledDate = null;

            DateTime? _CompleteDate = _StartDate + _RequiredEffortTime;

            DateTime? _DeadlineDate = _CompleteDate + TimeSpan.FromDays((rnd.Next(0, 30)));
            string? _Deliverables = prudoctTasks[i];
            string? _Remarks = RamarksTasks[i];
            int? _EngineerId = 0;

            Task task = new Task(100 + i, _Alias[i], DescriptionTasks[i], _CreatedAtDate, _IsMilestone, _RequiredEffortTime, _Copmlexity,
                _StartDate, _ScheduledDate, _DeadlineDate, _CompleteDate, _Deliverables, _Remarks, _EngineerId);
            s_dalTask!.Create(task);
        }
    }


    private static void createDependencies()
    {
        Random rnd = new Random();
        for (int i = 0; i < 40; i++)
        { 
            int _DependentTask = 101 + i / 2 - i / 38;
            int _DependsOnTask = rnd.Next(100, _DependentTask);

            Dependency dep = new Dependency(0, _DependentTask, _DependsOnTask);
            s_dalDependency!.Create(dep);
        }
    }

    private static void createEngineers()
    {
        string[] EngineerNames =
        {
        "Dani Levi", "Eli Amar", "Yair Cohen",
        "Ariela Levin", "Dina Klein", "Shira Israelof"
        };
        Random rnd = new Random();
        foreach (var _name in EngineerNames)
        {
            int _id;
            do
                _id = rnd.Next(200000000, 400000000);
            while (s_dalEngineer!.Read(_id) != null);

            string? _email = EngineerNames[_id].Take(3) + (_id % 100000).ToString() + "@gmail.com";

            DO.EngineerExperience _level = EngineerExperience.Beginner + rnd.Next(0, 5);
            double _cost = 200 + ((int)_level) * 50 + (double)(rnd.Next(-100,300)/3);

            Engineer Eng=new Engineer(_id, _email, _cost, _name, _level);
            s_dalEngineer!.Create(Eng);
        }
    }



    public static void Do(ITask? dalTask, IEngineer? dalEngineer, IDependency? dalDependency)
    {
        s_dalTask = dalTask ?? throw new NullReferenceException("DAL can not be null!");
        s_dalEngineer = dalEngineer ?? throw new NullReferenceException("DAL can not be null!");
        s_dalDependency = dalDependency ?? throw new NullReferenceException("DAL can not be null!");

        createTasks();
        createDependencies();
        createEngineers();
    }

}





