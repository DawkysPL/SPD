using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shrage
{
    class Algorithms
    {
        public static void AlgorithmShrage(List<Task> Tasks, List<Task> FinallyTasks)
        {
            //init
            int t = 0, k = 0, Cmax = 0;
            bool helper = true;
            List<Task> ReadyTasks = new List<Task>(); //G
            

            while(ReadyTasks.Count != 0 || Tasks.Count != 0)
            {
                while (Tasks.Count != 0 && AreTasksReadyToGo(Tasks, t))
                {
                    GetReadyTasks(Tasks,ReadyTasks,t);
                    helper = true;
                }

                if(ReadyTasks.Count == 0)
                {
                    t = GetTime(Tasks);
                    helper = false;
                }

                if(helper)
                {
                    GetFromReadyTasksMaxQTask(ReadyTasks, FinallyTasks,ref t, ref Cmax);
                    k++; 
                }
            }
            Console.WriteLine("cos");
        }

        public static bool AreTasksReadyToGo (List<Task> Tasks, int t)
        {
           var task = Tasks.FirstOrDefault(x => x.R <= t);
            if (task != null)
                return true;
            return false;
        }

        public static void GetReadyTasks(List<Task> Tasks, List<Task> ReadyTasks, int t)
        {
            int IndexTask = Tasks.FindIndex(x => x.R <= t);

            ReadyTasks.Add(Tasks[IndexTask]);
            Tasks.RemoveAt(IndexTask);
        }

        public static int GetTime(List<Task> Tasks)
        {
            int task = Tasks.Min(x => x.R);
            return task;
        }

        public static void  GetFromReadyTasksMaxQTask (List<Task> ReadyTasks,List<Task> FinallyTasks,ref int t, ref int Cmax)
        {
            int MaxValueQ = ReadyTasks.Max(x => x.Q);
            int IndexTask = ReadyTasks.FindIndex(x => x.Q == MaxValueQ);

            FinallyTasks.Add(ReadyTasks[IndexTask]);
            t += ReadyTasks[IndexTask].P;
            Cmax = Math.Max(Cmax, t + ReadyTasks[IndexTask].Q);
            ReadyTasks.RemoveAt(IndexTask);
        }
    }
}
