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
           // Console.WriteLine("cos"); TUTAJ DEBUG POINT ABY SPRAWDZIC CMax
        }

        public static void AlgorithmShrageWithSegregatedTasks(List<Task> Tasks, List<Task> FinallyTasks)
        {
            int t = 0, Cmax = 0, l = 0, q0 = int.MaxValue - 1 ;
            bool helper = true;
            List<Task> ReadyTasks = new List<Task>(); //G
            

            while (ReadyTasks.Count != 0 || Tasks.Count != 0)
            {
                while (Tasks.Count != 0 && AreTasksReadyToGo(Tasks, t))
                {
                    GetReadyTasksWithModification(Tasks, ReadyTasks, ref t, l, FinallyTasks);
                    helper = true;
                }

                if (ReadyTasks.Count == 0)
                {
                    t = GetTime(Tasks);
                    helper = false;
                }

                if (helper)
                {
                    GetFromReadyTasksMaxQTaskWithModification(ReadyTasks, FinallyTasks, ref t, ref Cmax,ref l);
                }
            }
            // Console.WriteLine("cos"); TUTAJ DEBUG POINT ABY SPRAWDZIC CMax

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

        public static void GetReadyTasksWithModification(List<Task> Tasks, List<Task> ReadyTasks,ref int t, int l, List<Task> FinallyTask)
        {
            int helper = t;
            int IndexTask = Tasks.FindIndex(x => x.R <= helper);
            int Qe = Tasks[IndexTask].Q;
            int Re = Tasks[IndexTask].R;
            ReadyTasks.Add(Tasks[IndexTask]);
            Tasks.RemoveAt(IndexTask);

            if(l != 0 && FinallyTask.Count != 0 )
            {
                int IndexTaskActual = FinallyTask.FindIndex(x => x.Id == l);
                if (IndexTaskActual == -1)
                    return;
                int Ql = FinallyTask[IndexTaskActual].Q;
                if (Qe > Ql)
                {
                    FinallyTask[IndexTaskActual].P = t - Re;
                    t = Re;
                    
                    if (FinallyTask[IndexTaskActual].P > 0)
                    {
                        ReadyTasks.Add(FinallyTask[IndexTaskActual]);
                        FinallyTask.RemoveAt(IndexTaskActual);
                    }
                }
            } 
        }

        public static void GetFromReadyTasksMaxQTaskWithModification(List<Task> ReadyTasks, List<Task> FinallyTasks, ref int t, ref int Cmax, ref int l)
        {
            int MaxValueQ = ReadyTasks.Max(x => x.Q);
            int IndexTask = ReadyTasks.FindIndex(x => x.Q == MaxValueQ);

            FinallyTasks.Add(ReadyTasks[IndexTask]);
            t += ReadyTasks[IndexTask].P;
            Cmax = Math.Max(Cmax, t + ReadyTasks[IndexTask].Q);
            l = ReadyTasks[IndexTask].Id;
            ReadyTasks.RemoveAt(IndexTask);
        }

        public static void AlgorithmShrageUseHeap(List<Task> Tasks, List<Task> FinallyTasks)
        {
            //init

            int t = 0, k = 0, Cmax = 0;
            bool helper = true;
            LowHeap unassignedTasks = new LowHeap();
            for(int i = 0; i< Tasks.Count; i++)
            {
                unassignedTasks.Insert(Tasks[i]);
            }
            MaxHeap ReadyTasks = new MaxHeap(); //G


            while (ReadyTasks.HeapSize != 0 || unassignedTasks.HeapSize != 0)
            {
                while (unassignedTasks.HeapSize != 0 && unassignedTasks.returnMin().R <= t)
                {
                    //GetReadyTasks(Tasks, ReadyTasks, t);
                    ReadyTasks.Insert(unassignedTasks.returnMin());
                    unassignedTasks.DeleteMin();
                    helper = true;
                }

                if (ReadyTasks.HeapSize == 0)
                {
                    t = unassignedTasks.returnMin().R;
                    helper = false;
                }

                if (helper)
                {
                    GetFromReadyTasksMaxQTask(ReadyTasks, FinallyTasks, ref t, ref Cmax);
                    k++;
                }
            }
            // Console.WriteLine("cos"); TUTAJ DEBUG POINT ABY SPRAWDZIC CMax
        }

        public static void GetFromReadyTasksMaxQTask(MaxHeap ReadyTasks, List<Task> FinallyTasks ,ref int t, ref int Cmax)
        {
            int MaxValueQ = ReadyTasks.returnMax().Q;
            //int IndexTask = ReadyTasks.FindIndex(x => x.Q == MaxValueQ);

            FinallyTasks.Add(ReadyTasks.returnMax());
            t += ReadyTasks.returnMax().P;
            Cmax = Math.Max(Cmax, t + ReadyTasks.returnMax().Q);
            ReadyTasks.DeleteMax();
        }
    }

}
