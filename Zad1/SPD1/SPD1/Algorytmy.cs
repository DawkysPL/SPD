using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPD1
{
    class Algorytmy
    {
        #region CompleteReviewProblem
        /*1. Poruszamy się od prawej strony do lewej, tak długo aż napotkamy element
          mniejszy od swojego prawego sąsiada. W tym przypadku elementem tym będzie
          5 o ideksie i=5 (liczonym od zera);

       2. Poruszając się ponownie od prawej strony szukamy pierwszego elementu
          większego niż ten znaleziony na pozycji "i", czyli 5. Elementem tym będzie
          7 o indeksie j=7.

       3. Zamieniamy miejscami elementy o indeksach "i" oraz "j". Otrzymamy szereg:
          1 4 6 2 9 7 8 5 3.

       4. Odbijamy lustrzanie szereg elementów od indeksu i+1 do końca szeregu.
          Otrzymamy szereg: 1 4 6 2 9 7 3 5 8. Wyznaczony szereg jest szukanym
          przez nas rozwiązaniem. */
        public static void Permutacje(int liczbaZadan, List<Maszyna> Maszyny)
        {
            int[] tablicaPermutacji = new int[liczbaZadan];
            string path = @"resultCompleteReviewProblem.txt";
            StreamWriter sw = new StreamWriter(path);
            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
            }
            for (int i = 0; i < liczbaZadan; i++)
            {
                tablicaPermutacji[i] = i;
            }
            for (int iterationIndex = 0; iterationIndex < (ObliczSilnie(liczbaZadan) - 1); iterationIndex++)
            {
                int i, j, k;
                if (liczbaZadan < 2)
                {
                    return;
                }
                i = liczbaZadan - 1;
                while ((i > 0) && (tablicaPermutacji[i - 1] >= tablicaPermutacji[i]))
                {
                    i--;
                }
                if (i > 0)
                {
                    j = liczbaZadan;
                    while ((j > 0) && (tablicaPermutacji[j - 1] <= tablicaPermutacji[i - 1]))
                    {
                        j--;
                    }
                }
                else
                {
                    j = -69;
                }
                if ((i > 0) && (j > 0))
                {
                    k = tablicaPermutacji[i - 1];
                    tablicaPermutacji[i - 1] = tablicaPermutacji[j - 1];
                    tablicaPermutacji[j - 1] = k;
                }
                for (i++, j = liczbaZadan; i < j; i++, j--)
                {
                    k = tablicaPermutacji[i - 1];
                    tablicaPermutacji[i - 1] = tablicaPermutacji[j - 1];
                    tablicaPermutacji[j - 1] = k;
                }
                int[,] listAllMaschine = calculateCMax(Maszyny, tablicaPermutacji);
                WynikAlgorytmu Wynik = getCmax(listAllMaschine);
                sw.Write("Permutacja: ");
                for (int b = 0; b < tablicaPermutacji.Length; b++)
                {
                    sw.Write($"{tablicaPermutacji[b]} | ");
                }
                sw.Write($"Czas: {Wynik.Cmax} | Cmax od maszyny: {Wynik.Index + 1}");
                sw.WriteLine();
            }
            sw.Close();
        }
        #endregion
        #region OperateFunctions
        public static int ObliczSilnie(int liczbaZadan)
        {
            if (liczbaZadan == 1)
            {
                return 1;
            }
            else
            {
                return liczbaZadan * ObliczSilnie(liczbaZadan - 1);
            }
        }
        public static WynikAlgorytmu getCmax(int[,] listAllMachine)
        {
            List<int> CmaxAllMachines = new List<int>();
            for (int i = 0; i < Maszyna.liczbaMaszyn; i++)
            {
                CmaxAllMachines.Add(listAllMachine[i, Maszyna.liczbaZadan - 1]);
            }

            var maxVal = CmaxAllMachines.Max();
            int index = CmaxAllMachines.IndexOf(maxVal);
            WynikAlgorytmu Wynik = new WynikAlgorytmu(index, maxVal);
            return Wynik;
        }
        public static void chooseOptimalPermutationForTwoMachine(List<Maszyna> Maszyny, int[] tableOfPermutation)
        {
            int[] table = new int[3]; // numer maszyny, numer zadania, najmniejsza wartosc
            int[] helpTable = new int[3];
            List<int[]> HelpList = new List<int[]>();
            for (int i = 0; i < Maszyna.liczbaZadan; i++)
            {
                if (Maszyny[0].TimeWithTask[i].Time <= Maszyny[1].TimeWithTask[i].Time)
                {
                    HelpList.Add(new int[] { 0, 0, 0 });
                    HelpList[i][0] = 1;
                    HelpList[i][1] = i;
                    HelpList[i][2] = Maszyny[0].TimeWithTask[i].Time;

                }
                else
                {
                    HelpList.Add(new int[] { 0, 0, 0 });
                    HelpList[i][0] = 2;
                    HelpList[i][1] = i;
                    HelpList[i][2] = Maszyny[1].TimeWithTask[i].Time;
                }
            }
            for (int i = 0; i < (Maszyna.liczbaZadan - 1); i++)// sortowanie bąbelkowe
            {
                for (int j = 0; j < (Maszyna.liczbaZadan - 1); j++)
                {
                    if (HelpList[j][2] > HelpList[j + 1][2])
                    {
                        helpTable = HelpList[j];
                        HelpList[j] = HelpList[j + 1];
                        HelpList[j + 1] = helpTable;
                    }

                }
            }
            for (int i = 0, j = (Maszyna.liczbaZadan - 1), k = 0; i <= j; k++)
            {
                if (HelpList[k][0] == 1)
                {
                    tableOfPermutation[i] = HelpList[k][1];
                    ++i;
                }
                else if (HelpList[k][0] == 2)
                {
                    tableOfPermutation[j] = HelpList[k][1];
                    --j;
                }
            }
        }
        public static int[,] calculateCMax(List<Maszyna> Maszyny, int[] tableOfPermutation)
        {
            int[,] listAllMaschine = new int[Maszyna.liczbaMaszyn, Maszyna.liczbaZadan];
            listAllMaschine[0, 0] = Maszyny[0].TimeWithTask[tableOfPermutation[0]].Time;
            for (int i = 1; i < Maszyna.liczbaZadan; i++)
            {
                listAllMaschine[0, i] = listAllMaschine[0, i - 1] + Maszyny[0].TimeWithTask[tableOfPermutation[i]].Time;
            }
            for (int j = 1; j < Maszyna.liczbaMaszyn; j++)
            {
                listAllMaschine[j, 0] = listAllMaschine[j - 1, 0] + Maszyny[j].TimeWithTask[tableOfPermutation[0]].Time;
                for (int i = 1; i < Maszyna.liczbaZadan; i++)
                {
                    listAllMaschine[j, i] = Math.Max(listAllMaschine[j, i - 1], listAllMaschine[j - 1, i]) + Maszyny[j].TimeWithTask[tableOfPermutation[i]].Time;
                }
            }
            return listAllMaschine;
        }
        #endregion
        #region Johnson
        public static void johnsonAlgoritmForTwoMachine(List<Maszyna> Maszyny)
        {
            int[] tableOfPermutation = new int[Maszyna.liczbaZadan];
            chooseOptimalPermutationForTwoMachine(Maszyny, tableOfPermutation);
            int[,] listAllMaschine = calculateCMax(Maszyny, tableOfPermutation);
            WynikAlgorytmu Result = getCmax(listAllMaschine);
            string path = @"resultJohnsonAlgoritmForTwoMachines.txt";
            StreamWriter sw = new StreamWriter(path);
            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
            }
            sw.Write("Permutacja: ");
            for (int b = 0; b < tableOfPermutation.Length; b++)
            {
                sw.Write($"{tableOfPermutation[b]} | ");
            }
            sw.Write($"Czas: {Result.Cmax} | Cmax od maszyny: {Result.Index + 1}");
            sw.WriteLine();
            sw.Close();
        }
        public static void johnsonAlgoritmForThreeMachine(List<Maszyna> Maszyny)
        {
            List<Maszyna> wirtualMachine = new List<Maszyna>();
            int[] tableOfPermutation = new int[Maszyna.liczbaZadan];

            for (int i = 0; i < (Maszyna.liczbaMaszyn - 1); i++)
            {
                wirtualMachine.Add(new Maszyna(i + 1));
                for (int j = 0; j < Maszyna.liczbaZadan; j++)
                {
                    wirtualMachine[i].TimeWithTask.Add(new TimeWithTask(j + 1, (Maszyny[i].TimeWithTask[j].Time + Maszyny[i + 1].TimeWithTask[j].Time)));
                }
            }
            chooseOptimalPermutationForTwoMachine(wirtualMachine, tableOfPermutation);
            int[,] listAllMaschine = calculateCMax(Maszyny, tableOfPermutation);
            WynikAlgorytmu Result = getCmax(listAllMaschine);
            string path = @"resultJohnsonAlgoritmForThreeMachines.txt";
            StreamWriter sw = new StreamWriter(path);
            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
            }
            sw.Write("Permutacja: ");
            for (int b = 0; b < tableOfPermutation.Length; b++)
            {
                sw.Write($"{tableOfPermutation[b]} | ");
            }
            sw.Write($"Czas: {Result.Cmax} | Cmax od maszyny: {Result.Index + 1}");
            sw.WriteLine();
            sw.Close();
        }
        public static void johnsonAlgoritm(List<Maszyna> Maszyny)
        {
            if (Maszyna.liczbaMaszyn == 2)
            {
                johnsonAlgoritmForTwoMachine(Maszyny);
            }
            else if (Maszyna.liczbaMaszyn == 3)
            {
                johnsonAlgoritmForThreeMachine(Maszyny);
            }
        }
        #endregion
    }
}
