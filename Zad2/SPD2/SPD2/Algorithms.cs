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

        public static int[,] calculateCMax(List<Machine> Maszyny, int[] tableOfPermutation)
        {
            int[,] listAllMaschine = new int[Machine.numberOfMachines, Machine.numberOfTasks];
            listAllMaschine[0, 0] = Maszyny[0].TimeWithTask[tableOfPermutation[0]].Time;
            for (int i = 1; i < Machine.numberOfTasks; i++)
            {
                listAllMaschine[0, i] = listAllMaschine[0, i - 1] + Maszyny[0].TimeWithTask[tableOfPermutation[i]].Time;
            }
            for (int j = 1; j < Machine.numberOfMachines; j++)
            {
                listAllMaschine[j, 0] = listAllMaschine[j - 1, 0] + Maszyny[j].TimeWithTask[tableOfPermutation[0]].Time;
                for (int i = 1; i < Machine.numberOfTasks; i++)
                {
                    listAllMaschine[j, i] = Math.Max(listAllMaschine[j, i - 1], listAllMaschine[j - 1, i]) + Maszyny[j].TimeWithTask[tableOfPermutation[i]].Time;
                }
            }
            return listAllMaschine;
        }
    }
}
