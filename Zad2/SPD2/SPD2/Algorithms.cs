using System;
using System.Collections.Generic;
using System.Linq;

namespace SPD2
{
    class Algorithms
    {
        public static ResultOfAlgorithm getCmax(int[,] listAllMachine)
        {
            List<int> CmaxAllMachines = new List<int>();
            for (int i = 0; i < Machine.numberOfMachines; i++)
            {
                CmaxAllMachines.Add(listAllMachine[i, Machine.numberOfTasks - 1]);
            }

            var maxVal = CmaxAllMachines.Max();
            int index = CmaxAllMachines.IndexOf(maxVal);
            ResultOfAlgorithm Result = new ResultOfAlgorithm(index, maxVal);
            return Result;
        }

        public static int calculateCMax(List<Machine> Machines, List<int> tableOfPermutation)
        {
            int[,] listAllMaschine = new int[Machine.numberOfMachines, tableOfPermutation.Count];
            listAllMaschine[0, 0] = Machines[0].TimeWithTask[tableOfPermutation[0]].Time;
            for (int i = 1; i < tableOfPermutation.Count; i++)
            {
                listAllMaschine[0, i] = listAllMaschine[0, i - 1] + Machines[0].TimeWithTask[tableOfPermutation[i]].Time;
            }
            for (int j = 1; j < Machine.numberOfMachines; j++)
            {
                listAllMaschine[j, 0] = listAllMaschine[j - 1, 0] + Machines[j].TimeWithTask[tableOfPermutation[0]].Time;
                for (int i = 1; i < tableOfPermutation.Count; i++)
                {
                    listAllMaschine[j, i] = Math.Max(listAllMaschine[j, i - 1], listAllMaschine[j - 1, i]) + Machines[j].TimeWithTask[tableOfPermutation[i]].Time;
                }
            }
            return listAllMaschine[Machine.numberOfMachines-1,Machine.numberOfTasks-1];
        }

        public static List<TimeWithTask> getWeights(List<Machine> Machines)
        {
            List<TimeWithTask> weights = new List<TimeWithTask>();
            int helper = 0;
            foreach (var machine in Machines)
            {
                for (int i = 0; i < Machine.numberOfTasks; i++)
                {
                    if (helper<Machine.numberOfTasks)
                    {
                        weights.Add(new TimeWithTask(machine.TimeWithTask[i].NumberTask, machine.TimeWithTask[i].Time));
                        helper++;
                    }
                    else
                    {
                        weights[i].Time += machine.TimeWithTask[i].Time;
                    }
                }
            }
            weights.Sort();
            return weights;
        }

        public static void algorithmNEH(List<Machine> Machines)
        {
            List<TimeWithTask> weights = getWeights(Machines);
            List<int> tasks = new List<int>();
            List<int> sortedTasks = new List<int>();
            
            for (int i = 0; i < Machine.numberOfTasks; i++)
            {
                TimeWithTask a = weights[weights.Count - 1];
                foreach (var weight in weights)
                {
                    if (weight.Time == a.Time)
                    {
                        tasks.Add(weight.NumberTask);
                        weights.Remove(weight);
                        break;
                    }
                }
             //tutaj 
             if (i == 0)
             {
                 sortedTasks.Add(tasks[0]);
             }
             else
             {
                 int getCmax = 0;
                 int[] table = new int[tasks.Count];
                 for (int j = 0; j < tasks.Count; j++)
                 {
                    
                 }
             }  
            }
        }
    }
}
