using System;
using System.Collections.Generic;
using System.Linq;

namespace SPD2
{
    class Algorithms
    {
        /*public static ResultOfAlgorithm getCmax(int[,] listAllMachine)
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
        }*/

        public static int calculateCMax(List<Machine> Machines, List<int> tableOfPermutation)
        {
            int[,] listAllMaschine = new int[Machine.numberOfMachines, tableOfPermutation.Count];
            listAllMaschine[0, 0] = Machines[0].TimeWithTask[tableOfPermutation[0]-1].Time;
            for (int i = 1; i < tableOfPermutation.Count; i++)
            {
                listAllMaschine[0, i] = listAllMaschine[0, i - 1] + Machines[0].TimeWithTask[tableOfPermutation[i]-1].Time;
            }
            for (int j = 1; j < Machine.numberOfMachines; j++)
            {
                listAllMaschine[j, 0] = listAllMaschine[j - 1, 0] + Machines[j].TimeWithTask[tableOfPermutation[0]-1].Time;
                for (int i = 1; i < tableOfPermutation.Count; i++)
                {
                    listAllMaschine[j, i] = Math.Max(listAllMaschine[j, i - 1], listAllMaschine[j - 1, i]) + Machines[j].TimeWithTask[tableOfPermutation[i]-1].Time;
                }
            }
            return listAllMaschine[Machine.numberOfMachines-1, tableOfPermutation.Count-1];
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
            weights.Reverse();
            return weights;
        }

        public static void algorithmNEH(List<Machine> Machines)
        {
            List<TimeWithTask> weights = getWeights(Machines);
            List<int> tasks = new List<int>(); // lista z roboczą listą
            List<int> sortedTasks = new List<int>(); // lista z najlepszym ustawieniem w danej chwili
            int Cmax;

            tasks.Add(weights[0].NumberTask); // dodanie pierwszego elementu
            sortedTasks.Add(weights[0].NumberTask);
            for (int i = 1; i < Machine.numberOfTasks; i++)
            {
               tasks.Insert(0, weights[i].NumberTask); // dodawanie kolejnych elementów
                sortedTasks.Insert(0, weights[i].NumberTask);
               for(int j =0; j<i; j++){ // zamiana miejsca dla nowo dodanego elementu
                    int variable = tasks[j];
                    tasks[j] = tasks[j+1];
                    tasks[j+1] = variable;
                    if(calculateCMax(Machines, sortedTasks)>calculateCMax(Machines, tasks)){ // sprawdzenie czy po zamianie Cmax jest mniejsze
                   //     sortedTasks = tasks;
                        for(int k = 0; k<tasks.Count; k++){
                            sortedTasks[k] = tasks[k];
                        }   
                    }

                } 
               for(int k = 0; k<tasks.Count; k++){
                 tasks[k] = sortedTasks[k];
               }  // przypisanie wartosci najlepszej po całym przejsciu do listy roboczej 
            }
            Cmax = calculateCMax(Machines,sortedTasks); // wyliczenie Cmax
        }
    }
}
