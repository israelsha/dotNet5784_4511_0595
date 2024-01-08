
namespace DalTest;
using DalApi;
using DO;
using System;

public static class Initialization
{
    private static ITask? s_dalTask;             //stage 1
    private static IEngineer? s_dalEngineer;     //stage 1
    private static IDependency? s_dalDependency; //stage 1

    private static readonly Random s_rand = new();


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


    private static void createDependencies()
    {
        int _id = 10;
        int _DependentTask =3;
        int _DependsOnTask =2;
        for (int i = 0; i < 40; i++)
        {
            if(_DependentTask > _DependsOnTask)
            {
               
            }
            Dependency dep = new Dependency(_id, _DependentTask, _DependsOnTask);
            s_dalDependency!.Create(dep);

            _id++;
        }
    }
    }

