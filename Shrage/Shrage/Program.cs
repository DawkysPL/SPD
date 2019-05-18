using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Shrage
{
    class Program
    {
        public static int HowManyTasks;
        public static int HowManyLoadColumns; //alltimes 3

        static void Main(string[] args)
        {
            string[] text; 
            string [] names = {"test1.txt", "test2.txt", "test3.txt", "test4.txt", "test5.txt", "test50.txt", "test100.txt", "test200.txt"};
            string dataStart;

            Stopwatch stopWatch = new Stopwatch();
            for(int i = 0; i < names.Length; i++){
            text = File.ReadAllLines(@names[i]); //SPD/SPD/BIN/DEBUG
            dataStart= text[0];
            LoadNumbersOfTasksAndMachines(dataStart);
            List<Task> FinallyTasks = new List<Task>();
            List<Task> Tasks = new List<Task>();
            LoadDataIntoTasks(Tasks, text);
            CarlierParametrs obiect = new CarlierParametrs();
                  stopWatch.Reset();
            stopWatch.Start();
            Console.WriteLine(Algorithms.CarlierWildLeft(Tasks, FinallyTasks, obiect));
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds);
            Console.WriteLine("Czas pracy algorytmu WildLeft: " + elapsedTime);
            }
            for(int i = 0; i < names.Length; i++){
            text = File.ReadAllLines(@names[i]); //SPD/SPD/BIN/DEBUG
            dataStart= text[0];
            LoadNumbersOfTasksAndMachines(dataStart);
            List<Task> FinallyTasks = new List<Task>();
            List<Task> Tasks = new List<Task>();
            LoadDataIntoTasks(Tasks, text);
            CarlierParametrs obiect = new CarlierParametrs();
                stopWatch.Reset();
            stopWatch.Start();
            Console.WriteLine(Algorithms.CarlierDeepLeft(Tasks, FinallyTasks, obiect));
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds);
            Console.WriteLine("Czas pracy algorytmu DeepLeft: " + elapsedTime);
            }
            for(int i = 0; i < names.Length; i++){
            text = File.ReadAllLines(@names[i]); //SPD/SPD/BIN/DEBUG
            dataStart= text[0];
            LoadNumbersOfTasksAndMachines(dataStart);
            List<Task> FinallyTasks = new List<Task>();
            List<Task> Tasks = new List<Task>();
            LoadDataIntoTasks(Tasks, text);
            CarlierParametrs obiect = new CarlierParametrs();
                  stopWatch.Reset();
                stopWatch.Start();
            Console.WriteLine(Algorithms.CarlierGreedy(Tasks, FinallyTasks, obiect));
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds);
            Console.WriteLine("Czas pracy algorytmu Greedy: " + elapsedTime);
            }
            for(int i = 0; i < names.Length; i++){
            text = File.ReadAllLines(@names[i]); //SPD/SPD/BIN/DEBUG
            dataStart= text[0];
            LoadNumbersOfTasksAndMachines(dataStart);
            List<Task> FinallyTasks = new List<Task>();
            List<Task> Tasks = new List<Task>();
            LoadDataIntoTasks(Tasks, text);
            CarlierParametrs obiect = new CarlierParametrs();
                  stopWatch.Reset();
            stopWatch.Start();
            Console.WriteLine(Algorithms.CarlierOnlyLeft(Tasks, FinallyTasks, obiect));
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds );
            Console.WriteLine("Czas pracy algorytmu OnlyLeft: " + elapsedTime);
            }
            Console.ReadLine();
        }


        static void LoadNumbersOfTasksAndMachines(string dataStart)
        {
            string[] result = dataStart.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            HowManyTasks = Convert.ToInt32(result[0]);
            HowManyLoadColumns = Convert.ToInt32(result[1]);
        }  
        static void LoadDataIntoTasks(List<Task> Tasks, string[] text)
        {
            string[] result;
            for (int i = 1; i <= HowManyTasks; i++)
            {
                result = text[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Tasks.Add(new Task { Id = i ,R = Convert.ToInt32(result[0]), P = Convert.ToInt32(result[1]), Q = Convert.ToInt32(result[2]) });
            }
        }
        
    }
}
