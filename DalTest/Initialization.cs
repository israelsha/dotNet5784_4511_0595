namespace DalTest;
using DO;
using System;
using Dal;
using System.Xml.Linq;

public static class Initialization
{
    private static IDal? s_dal; //stage 2
    private static readonly Random s_rand = new();

    /// <summary>
    /// create 20 tasks
    /// </summary>
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
        int[] arrDifficulty = new int[] { 0, 1, 3, 2, 2, 2, 3, 4, 2, 4, 3, 3, 1, 1, 1, 2, 1, 0, 0, 3 };
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

        int[] _EngineerId = { 324567891, 389012345,398765432,201987654,201987654,201987654,398765432,213074522,201987654,213074522,
            398765432,398765432,389012345, 389012345,389012345,201987654,389012345,201987654, 235678901,398765432 };

        Random rnd = new Random();
        for (int i = 0; i < 19; i++)
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

            Task task = new Task(100 + i, _Alias[i], DescriptionTasks[i], _CreatedAtDate, _IsMilestone, _RequiredEffortTime, _Copmlexity,
                _StartDate, _ScheduledDate, _DeadlineDate, _CompleteDate, _Deliverables, _Remarks, _EngineerId[i]);
            s_dal!.Task.Create(task); 
        }
    }

    /// <summary>
    /// create 40 Dependencies
    /// </summary>
    private static void createDependencies()
    {
        Random rnd = new Random();
        for (int i = 0; i < 40; i++)
        { 
            int _DependentTask = 100 + i / 2 ;
            int _DependsOnTask = 100 + (7 * i) % 19 + 1;

            Dependency dep = new Dependency(0, _DependentTask, _DependsOnTask);
            s_dal!.Dependency.Create(dep);
        }
    }

    /// <summary>
    /// create 6 engineers
    /// </summary>
    private static void createEngineers()
    {
        string[] EngineerNames =
        {
        "Dani Levi", "Eli Amar", "Shira Israelof" ,
        "Ariela Levin", "Israel Shaashua","Dina Klein"
        };
        int[] _id = { 324567891, 389012345, 201987654, 398765432, 213074522, 235678901 };
        int i = 0;
        Random rnd = new Random();
        foreach (var _name in EngineerNames)
        {
            string? _email = (_id[i] % 10000).ToString() + "@gmail.com";

            DO.EngineerExperience _level = EngineerExperience.Beginner + i % 5;
            double _cost = 200 + ((int)_level) * 50 + (double)(rnd.Next(-100, 300) / 3);

            Engineer Eng=new Engineer(_id[i], _email, _cost, _name, _level);
            s_dal!.Engineer.Create(Eng);
            i++;
        }
    }

    public static void Do(IDal dal) //stage 2
    {
        s_dal = dal ?? throw new NullReferenceException("DAL object can not be null!"); 
        createTasks();
        createDependencies();
        createEngineers();
    }

    public static void initialize()
    {
        const string s_xml_dir = @"..\xml\";
        
        XElement x_task = new XElement("ArrayOfTask","\n");
        x_task.Save($"{s_xml_dir + "tasks"}.xml");

        XElement x_eng = new XElement("ArrayOfEngineer","\n");
        x_eng.Save($"{s_xml_dir + "engineers"}.xml");

        XElement x_dep = new XElement("ArrayOfDependency", "\n");
        x_dep.Save($"{s_xml_dir + "dependencies"}.xml");

        XElement x_con_task = new XElement("NextTaskId", 100);
        XElement x_con_dep = new XElement("NextDependencyId", 100);
        XElement x_con = new XElement("config", x_con_task, x_con_dep);
        x_con.Save($"{ s_xml_dir + "data-config"}.xml");

    }
}
