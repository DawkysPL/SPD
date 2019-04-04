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

        //   public static int numberOfException;

        /* Zastosowanie metodyki grafu z zapisywanie czasu zakoczenia kazdego zadania w tablicy 2-wymiarowej */
        public static int calculateCMax(List<Machine> Machines, List<int> tableOfPermutation)
        {
            int[,] listAllMaschine = new int[Machine.numberOfMachines, tableOfPermutation.Count]; // deklaracja grafu
            listAllMaschine[0, 0] = Machines[0].TimeWithTask[tableOfPermutation[0] - 1].Time; // pierwszy element
            for (int i = 1; i < tableOfPermutation.Count; i++) // skrajny przypadek maszyna 0
            {
                listAllMaschine[0, i] = listAllMaschine[0, i - 1] + Machines[0].TimeWithTask[tableOfPermutation[i] - 1].Time;
            }
            for (int j = 1; j < Machine.numberOfMachines; j++) // skrajny przypadek zadanie 0
            {
                listAllMaschine[j, 0] = listAllMaschine[j - 1, 0] + Machines[j].TimeWithTask[tableOfPermutation[0] - 1].Time;
                for (int i = 1; i < tableOfPermutation.Count; i++)
                {
                    listAllMaschine[j, i] = Math.Max(listAllMaschine[j, i - 1], listAllMaschine[j - 1, i]) + Machines[j].TimeWithTask[tableOfPermutation[i] - 1].Time;
                } // przy pomcy metody max przyznawanie odpowiednich wartości pośrednim elementom
            }
            return listAllMaschine[Machine.numberOfMachines - 1, tableOfPermutation.Count - 1];
        }

        // wyznaczanie wag dla konkretnych danych
        public static List<TimeWithTask> getWeights(List<Machine> Machines)
        {
            List<TimeWithTask> weights = new List<TimeWithTask>();
            int helper = 0;
            foreach (var machine in Machines) // wyznaczanie wag (całkiem sprytne)
            {
                for (int i = 0; i < Machine.numberOfTasks; i++)
                {
                    if (helper < Machine.numberOfTasks)
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
            weights.Sort(); // sortowanie listy
            weights.Reverse(); // odwrócenie listy (niestety nie jest w 100% tak jak w programiku pokazowym)
            // naprawa sortowania
            int k = 1;
            int m = 0;
            for (; k != m;)
            { // dopoki nie dokona zmiany w liscie sie wykonuje
                k = 0;
                m = 0;
                for (int i = 0; (i < weights.Count - 1); i++)
                {

                    if (weights[i].Time == weights[i + 1].Time)
                    {
                        if (weights[i].NumberTask > weights[i + 1].NumberTask)
                        {
                            int taskMustChange = weights[i].NumberTask; // zmienia miejscami numer zadania dla takich samych wag aby byly w kolejnosci rosnacej
                            weights[i].NumberTask = weights[i + 1].NumberTask;
                            weights[i + 1].NumberTask = taskMustChange;
                            ++k;
                        }
                    }
                }
            }
            return weights;
        }
        // podstawowy algorytm NEH z wyliczeniem wszystkch elementow bez akceleracji
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
                for (int j = 0; j < i; j++)
                { // zamiana miejsca dla nowo dodanego elementu
                    int variable = tasks[j];
                    tasks[j] = tasks[j + 1];
                    tasks[j + 1] = variable;
                    if (calculateCMax(Machines, sortedTasks) > calculateCMax(Machines, tasks))
                    { // sprawdzenie czy po zamianie Cmax jest mniejsze
                      //     sortedTasks = tasks;
                        for (int k = 0; k < tasks.Count; k++)
                        {
                            sortedTasks[k] = tasks[k];
                        }
                    }

                }
                for (int k = 0; k < tasks.Count; k++)
                {
                    tasks[k] = sortedTasks[k];
                }  // przypisanie wartosci najlepszej po całym przejsciu do listy roboczej 
            }
            Cmax = calculateCMax(Machines, sortedTasks); // wyliczenie Cmax
            Console.WriteLine(Cmax);
        }
        // wylicznie Cmax z zastosowaniem akceleracji ostatni element w podanej liscie jest przekladany
        public static int calculateCMaxWithAcceleration(List<Machine> Machines, List<int> tableOfPermutation)
        {

            GraphVertice[,] listAllMaschine = new GraphVertice[Machine.numberOfMachines, (tableOfPermutation.Count - 1)];
            // stworzenie tablicy z wiezlami grafu (czas skonczenia zadania, numer zadania, czas trwania, czas przejscia od konca)
            for (int i = 0; i < Machine.numberOfMachines; i++)
            {
                for (int j = 0; j < (tableOfPermutation.Count - 1); j++)
                {
                    listAllMaschine[i, j] = new GraphVertice();
                }
            }// uzupelnienie listy pustymi wezlami
            listAllMaschine[0, 0].FinishTime = Machines[0].TimeWithTask[tableOfPermutation[0] - 1].Time;
            for (int i = 1; i < (tableOfPermutation.Count - 1); i++)
            {
                listAllMaschine[0, i].FinishTime = listAllMaschine[0, i - 1].FinishTime + Machines[0].TimeWithTask[tableOfPermutation[i] - 1].Time;
            }
            for (int j = 1; j < Machine.numberOfMachines; j++)
            {
                listAllMaschine[j, 0].FinishTime = listAllMaschine[j - 1, 0].FinishTime + Machines[j].TimeWithTask[tableOfPermutation[0] - 1].Time;
                for (int i = 1; i < (tableOfPermutation.Count - 1); i++)
                {
                    listAllMaschine[j, i].FinishTime = Math.Max(listAllMaschine[j, i - 1].FinishTime, listAllMaschine[j - 1, i].FinishTime) + Machines[j].TimeWithTask[tableOfPermutation[i] - 1].Time;
                }
            }
            // tak jak w zwyklym wyliczamy czasy zakonczenia zadania (dlugośc drogi od wezla(0,0))

            listAllMaschine[(Machine.numberOfMachines - 1), (tableOfPermutation.Count - 2)].ReverseTime = Machines[(Machine.numberOfMachines - 1)].TimeWithTask[tableOfPermutation[(tableOfPermutation.Count - 2)] - 1].Time;
            for (int i = (tableOfPermutation.Count - 3); i >= 0; i--)
            {
                listAllMaschine[(Machine.numberOfMachines - 1), i].ReverseTime = listAllMaschine[(Machine.numberOfMachines - 1), i + 1].ReverseTime + Machines[(Machine.numberOfMachines - 1)].TimeWithTask[tableOfPermutation[i] - 1].Time;
            }
            for (int j = (Machine.numberOfMachines - 2); j >= 0; j--)
            {
                listAllMaschine[j, (tableOfPermutation.Count - 2)].ReverseTime = listAllMaschine[j + 1, (tableOfPermutation.Count - 2)].ReverseTime + Machines[j].TimeWithTask[tableOfPermutation[(tableOfPermutation.Count - 2)] - 1].Time;
                for (int i = (tableOfPermutation.Count - 3); i >= 0; i--)
                {
                    listAllMaschine[j, i].ReverseTime = Math.Max(listAllMaschine[j, i + 1].ReverseTime, listAllMaschine[j + 1, i].ReverseTime) + Machines[j].TimeWithTask[tableOfPermutation[i] - 1].Time;
                }
            }// wyliczamy dla kazdego wezla dlugosc drogi od ostaniego wezla

            int[] helpTable = new int[Machine.numberOfMachines]; // definicja tablicy z czasami nowo dodanego elementu
            int[] helpFinishTable = new int[Machine.numberOfMachines]; // czasy zakonczenia zadania nowo dodanego elementu
            // wyliczenie czasow zakonczenia dla nowego elementu przypadek gdy wstawiony element jest na pierwszej pozycji
            for (int i = 0; i < Machine.numberOfMachines; i++)
            {
                helpTable[i] = Machines[i].TimeWithTask[tableOfPermutation[tableOfPermutation.Count - 1] - 1].Time;
            }

            helpFinishTable[0] = helpTable[0];

            for (int i = 1; i < Machine.numberOfMachines; i++)
            {
                helpFinishTable[i] = helpFinishTable[i - 1] + helpTable[i];
            }
            int nrOfLastWeight = 0; // numer na ktory ma wyladowac przekladany element 
            int Cmax = 0; // Cmax dla kazdego ustawienia
            int Cmax2 = 0; // Cmax optymalne dla permutacji ustwaien
            int CmaxHelp; // pomocny Cmax dla kazdego prypadku 

            //variable = tableOfPermutation[0];
            //tableOfPermutation[0] = tableOfPermutation[tableOfPermutation.Count - 1];
            //tableOfPermutation[tableOfPermutation.Count - 1] = variable;

            for (int i = 0; i < tableOfPermutation.Count; i++) //  wszystkie ustawienia nowego zadania
            {
                CmaxHelp = 0; // zerowanie Cmax pomocne
                Cmax = 0; // zerowanie Cmax dla jednego ustawienia 
                for (int j = 0; j < Machine.numberOfMachines; j++) // wszystkie maszyny
                {
                    if (i == 0) // przypadek gdy nowy element jest a pierwszej pozycji
                    {

                        CmaxHelp = Math.Max(CmaxHelp, helpFinishTable[j] + listAllMaschine[j, i].ReverseTime);

                        if (CmaxHelp > Cmax)
                        {
                            Cmax = CmaxHelp;
                        }
                        if ((j == (Machine.numberOfMachines - 1)))
                        {
                            Cmax2 = Cmax;
                            nrOfLastWeight = i;
                        }
                    }
                    else if (i == (tableOfPermutation.Count - 1)) // przypadek gdy nowy element jest na ostatniej pozycji
                    {
                        if (j == 0)
                        {
                            helpFinishTable[0] = helpTable[0] + listAllMaschine[0, i - 1].FinishTime;
                        }
                        else
                        {
                            helpFinishTable[j] = helpTable[j] + Math.Max(helpFinishTable[j - 1], listAllMaschine[j, i - 1].FinishTime);
                        }// wylicznie wartosci tabeli help finish table zawierajacej moment zakonczenia zadania przez zadania nowego elementu

                        CmaxHelp = Math.Max(CmaxHelp, helpFinishTable[j]);
                        if (CmaxHelp > Cmax) // Cmax jest ajdluża sciezką dla danego przejscia
                        {
                            Cmax = CmaxHelp;
                        }
                        if ((Cmax < Cmax2) && (j == (Machine.numberOfMachines - 1))) // najlepsze cmax jest najmniejsza wartoscią dla wszystkich przejść
                        {
                            Cmax2 = Cmax;
                            nrOfLastWeight = i;
                        }
                    }
                    else // przypadki gdy nowy element nie jest na krancach
                    {
                        if (j == 0) // uzupelnienie tabeli z czasami koncow wstawionego elementu
                        {
                            helpFinishTable[0] = helpTable[0] + listAllMaschine[0, i - 1].FinishTime;
                        }
                        else
                        {
                            helpFinishTable[j] = helpTable[j] + Math.Max(helpFinishTable[j - 1], listAllMaschine[j, i - 1].FinishTime);
                        }

                        CmaxHelp = Math.Max(CmaxHelp, helpFinishTable[j] + listAllMaschine[j, i].ReverseTime);
                        if (CmaxHelp > Cmax) // wyliczanie najdluzszej sciezki dla danego przejcia  
                        {
                            Cmax = CmaxHelp;
                        }
                        if ((Cmax < Cmax2) && (j == (Machine.numberOfMachines - 1)))
                        {
                            Cmax2 = Cmax;
                            nrOfLastWeight = i;
                        }

                    }
                }
            }


            tableOfPermutation.Insert(nrOfLastWeight, tableOfPermutation[tableOfPermutation.Count - 1]);
            tableOfPermutation.RemoveAt(tableOfPermutation.Count - 1);
            //numberOfException = nrOfLastWeight; w razie gdyby bylo potrzebne
            // przerobienie listy permutacji na ta z odpowiednim ustawieniem

            return Cmax2;
        }

        public static void algorithmNEHwithAcceleration(List<Machine> Machines)
        {
            List<TimeWithTask> weights = getWeights(Machines);
            List<int> tasks = new List<int>(); // lista z roboczą listą
            int Cmax = -1;

            tasks.Add(weights[0].NumberTask); // dodanie pierwszego elementu
            for (int i = 1; i < Machine.numberOfTasks; i++)
            {
                tasks.Add(weights[i].NumberTask); // dodawanie kolejnych elementów
                Cmax = calculateCMaxWithAcceleration(Machines, tasks); // wyliczanie Cmax

            }
            Console.WriteLine(Cmax);
        }

        public static void algorithmNEHwithModificationFour(List<Machine> Machines)
        {
            List<TimeWithTask> weights = getWeights(Machines);
            List<int> tasks = new List<int>(); // lista z roboczą listą
            List<int> newTasks = new List<int>(); // lista z pocna listą
            int Cmax = -1;  // Cmax
            int Cmax2 = -1; // Cmax  pomocne przy usuwaniu
            int numberOfDelete = 0; // numer usuwanego zadania

            tasks.Add(weights[0].NumberTask); // dodanie pierwszego elementu
            newTasks.Add(0);
            for (int i = 1; i < Machine.numberOfTasks; i++)
            {
                tasks.Add(weights[i].NumberTask); // dodawanie kolejnych elementów
                newTasks.Add(0);
                Cmax = calculateCMaxWithAcceleration(Machines, tasks); // wyliczanie Cmax
                Cmax2 = Cmax;
                // kod usuwajcy po jednym tasku i dokonujacy ponownie nowego ustawienia 
                for (int j = 0; j < tasks.Count; j++)
                {
                    if (tasks[j] == weights[i].NumberTask)
                    {
                    }
                    else
                    {
                        for (int k = 0; k < tasks.Count; k++)
                        {

                            newTasks[k] = tasks[k];
                        }
                        newTasks.RemoveAt(j);
                        if (Cmax2 > calculateCMax(Machines, newTasks)) // szukanie najbardziej zmniejszonego Cmax
                        {
                            Cmax2 = calculateCMax(Machines, newTasks);
                            numberOfDelete = j;
                        }
                        newTasks.Add(0);
                    }

                }
                tasks.Add(tasks[numberOfDelete]); // przezucenie wybraneo zadania na koniec listy w celu wykorzystania tak jak NEH z przyspieszeniem
                tasks.RemoveAt(numberOfDelete);
                Cmax = calculateCMaxWithAcceleration(Machines, tasks);

            }

            Console.WriteLine(Cmax);
        }


        public static void addNewCriticalPath(GraphVertice[,] listAllMaschine2, List<GraphVertice> criticalPath2, int index1, int index2)
        {
            // rekurencyjna metoda tworzaca scieżkę krytyczną idąca po największych czasach zakonczenia
            if (index1 == 0)
            {
                if (index2 == 0) { } // natrafienie na koniec
                else // poruszanie sie krancem
                {
                    criticalPath2.Add(listAllMaschine2[index1, index2 - 1]);
                    addNewCriticalPath(listAllMaschine2, criticalPath2, index1, (index2 - 1));
                }
            }
            else if (index2 == 0)
            {
                if (index1 == 0) { } // natrafienie na koniec 
                else
                { // poruszanie sie po krancu
                    criticalPath2.Add(listAllMaschine2[index1 - 1, index2]);
                    addNewCriticalPath(listAllMaschine2, criticalPath2, (index1 - 1), index2);
                }
            }
            else
            { // poruszanie sie nie krancowymi, sprawdzamy w przecinym kierunku do skierowania grafu który czas wezłów wczesniej jest wiekszy
                if (listAllMaschine2[index1, index2 - 1].FinishTime > listAllMaschine2[index1 - 1, index2].FinishTime)
                {
                    criticalPath2.Add(listAllMaschine2[index1, index2 - 1]);
                    addNewCriticalPath(listAllMaschine2, criticalPath2, index1, (index2 - 1));
                }
                else
                {
                    criticalPath2.Add(listAllMaschine2[index1 - 1, index2]);
                    addNewCriticalPath(listAllMaschine2, criticalPath2, (index1 - 1), index2);
                }
            }
        }


        public static void createCriticelPath(List<Machine> Machines, List<int> tableOfPermutation, int notModification, int modificationNumber)
        {
            GraphVertice[,] listAllMaschine = new GraphVertice[Machine.numberOfMachines, tableOfPermutation.Count]; // deklaracja grafu
            List<GraphVertice> criticalPath = new List<GraphVertice>();
            int hightTime = 0; // najwieksza wartość czasu, sumy czasów, ilości wystąpień
            int deleteId = -1; // id elementu który bedzie zamieniany

            for (int i = 0; i < Machine.numberOfMachines; i++)
            {
                for (int j = 0; j < (tableOfPermutation.Count); j++)
                {
                    listAllMaschine[i, j] = new GraphVertice();
                }
            }// uzupelnienie listy pustymi wezlami

            // stworzenie grafu z czasem zakonczenia pracy, z czasem pracy i numeem taska 

            listAllMaschine[0, 0].FinishTime = Machines[0].TimeWithTask[tableOfPermutation[0] - 1].Time; // pierwszy element uzupelniamy czas zakonczenia, cas pracy i numer zadania 
            listAllMaschine[0, 0].TaskTime = Machines[0].TimeWithTask[tableOfPermutation[0] - 1].Time;
            listAllMaschine[0, 0].TaskId = Machines[0].TimeWithTask[tableOfPermutation[0] - 1].NumberTask;
            for (int i = 1; i < tableOfPermutation.Count; i++) // skrajny przypadek maszyna 0
            {
                listAllMaschine[0, i].FinishTime = listAllMaschine[0, i - 1].FinishTime + Machines[0].TimeWithTask[tableOfPermutation[i] - 1].Time; // wszystkie zadania dla maszyny zerowej tak jak powyzej
                listAllMaschine[0, i].TaskTime = Machines[0].TimeWithTask[tableOfPermutation[i] - 1].Time;
                listAllMaschine[0, i].TaskId = Machines[0].TimeWithTask[tableOfPermutation[i] - 1].NumberTask;
            }
            for (int j = 1; j < Machine.numberOfMachines; j++) // skrajny przypadek zadanie 0
            {
                listAllMaschine[j, 0].FinishTime = listAllMaschine[j - 1, 0].FinishTime + Machines[j].TimeWithTask[tableOfPermutation[0] - 1].Time; // wszystkie maszyny dla zadania zerowego
                listAllMaschine[j, 0].TaskTime = Machines[j].TimeWithTask[tableOfPermutation[0] - 1].Time;
                listAllMaschine[j, 0].TaskId = Machines[j].TimeWithTask[tableOfPermutation[0] - 1].NumberTask;
                for (int i = 1; i < tableOfPermutation.Count; i++)
                {
                    listAllMaschine[j, i].FinishTime = Math.Max(listAllMaschine[j, i - 1].FinishTime, listAllMaschine[j - 1, i].FinishTime) + Machines[j].TimeWithTask[tableOfPermutation[i] - 1].Time; // pozostale wezly grafu
                    listAllMaschine[j, i].TaskTime = Machines[j].TimeWithTask[tableOfPermutation[i] - 1].Time;
                    listAllMaschine[j, i].TaskId = Machines[j].TimeWithTask[tableOfPermutation[i] - 1].NumberTask;
                } // przy pomcy metody max przyznawanie odpowiednich wartości pośrednim elementom
            }

            // sciezka krytyczna stworzenie
            criticalPath.Add(listAllMaschine[(Machine.numberOfMachines - 1), (tableOfPermutation.Count - 1)]); //pierwszy element
            addNewCriticalPath(listAllMaschine, criticalPath, (Machine.numberOfMachines - 1), (tableOfPermutation.Count - 1));

            if (modificationNumber == 1) // pierwsza modyfikacja szukamy najdluzszego czasu wykonywania
            {
                for (int m = 0; m < criticalPath.Count; m++)
                {
                    if (criticalPath[m].TaskId == notModification) { } // jesli to jest ten task który był wczesniej rotowany to ne sprawdzamy
                    else if (criticalPath[m].TaskTime > hightTime) // jesli czas wiekszy to zamienamy hightTime i zapisujemy dla jakiego elementu sciezki
                    {
                        hightTime = criticalPath[m].TaskTime;
                        deleteId = m;
                    }
                }
            }
            else if (modificationNumber == 2) // szukanie po najwiekszej sumie czasow wykonywania 
            {
                int[] sumTable = new int[(Machine.numberOfTasks + 1)];
                for (int k = 0; k < (Machine.numberOfTasks + 1); k++) // stworznie tabeli zapisujacej sumy czasow na swoich numerach indeksow
                {
                    sumTable[k] = 0;
                }
                for (int m = 0; m < criticalPath.Count; m++)
                {
                    if (criticalPath[m].TaskId == notModification) { }
                    else
                    {
                        sumTable[criticalPath[m].TaskId] += criticalPath[m].TaskId; // za kazdym natrafieniem zwiekszenie sumy o natrafiony czas dla danego tasku
                    }
                }
                for (int k = 0; k < (Machine.numberOfTasks + 1); k++) // odszukanie tego z najwiekszym czasem
                {
                    if (hightTime < sumTable[k])
                    {
                        hightTime = sumTable[k];
                        deleteId = k;
                    }
                }
            }
            else if (modificationNumber == 3) //szuaknie po największej liczbie wystapień
            {
                int[] sumTable = new int[(Machine.numberOfTasks + 1)];
                for (int k = 0; k < (Machine.numberOfTasks + 1); k++)
                {
                    sumTable[k] = 0;
                }
                for (int m = 0; m < criticalPath.Count; m++) //zwększanie liczby wystapie za kazdym napotkaniem
                {
                    if (criticalPath[m].TaskId == notModification) { }
                    else
                    {
                        ++sumTable[criticalPath[m].TaskId];
                    }
                }
                for (int k = 0; k < (Machine.numberOfTasks + 1); k++)
                {
                    if (hightTime < sumTable[k])
                    {
                        hightTime = sumTable[k];
                        deleteId = k;
                    }
                }
            }


            if (modificationNumber == 1) // zamiana na ostatnie miejsce wyszukanego elementu (dwa rodaje bo raz po indeksie sciezki, a raz po wartosci id taska)
            {
                for (int i = 0; i < (tableOfPermutation.Count); i++)
                {
                    if (tableOfPermutation[i] == criticalPath[deleteId].TaskId)
                    {
                        tableOfPermutation.RemoveAt(i);
                    }
                }
                tableOfPermutation.Add(criticalPath[deleteId].TaskId);
            }
            else
            {
                {
                    for (int i = 0; i < (tableOfPermutation.Count); i++)
                    {
                        if (tableOfPermutation[i] == deleteId)
                        {
                            tableOfPermutation.RemoveAt(i);
                        }
                    }
                    tableOfPermutation.Add(deleteId);
                }
            }
        }

        public static void algorithmNEHwithModification(List<Machine> Machines, int modificationNumber)
        {
            List<TimeWithTask> weights = getWeights(Machines);
            List<int> tasks = new List<int>(); // lista z roboczą listą
            int Cmax = -1;
            int Cmax2 = -1;

            tasks.Add(weights[0].NumberTask); // dodanie pierwszego elementu
            for (int i = 1; i < Machine.numberOfTasks; i++)
            {
                tasks.Add(weights[i].NumberTask); // dodawanie kolejnych elementów
                Cmax = calculateCMaxWithAcceleration(Machines, tasks); // wyliczanie Cmax
                createCriticelPath(Machines, tasks, weights[i].NumberTask, modificationNumber);
                //create crytical path

                Cmax2 = calculateCMaxWithAcceleration(Machines, tasks); // wyliczenie po znalezieniu elementu modifikacji

            }
            Console.WriteLine(Cmax2);
        }

        // zamiana dwoch losowych elementow     
        public static void swapTasks(List<int> tasks)
        {
            Random rand = new Random();
            int firstElementToSwap = rand.Next(0, tasks.Count);
            int secondElementToSwap = rand.Next(0, tasks.Count);
            int helper = tasks[firstElementToSwap];
            tasks[firstElementToSwap] = tasks[secondElementToSwap];
            tasks[secondElementToSwap] = helper;

        }

        // zamana dwoch lsowych sąsiadów
        public static void swapNeighboringTasks(List<int> tasks)
        {
            Random rand = new Random();
            int firstElementToSwap = rand.Next(0, tasks.Count - 1);
            int secondElementToSwap = firstElementToSwap + 1;
            int helper = tasks[firstElementToSwap];
            tasks[firstElementToSwap] = tasks[secondElementToSwap];
            tasks[secondElementToSwap] = helper;
        }

        // wstawainie losowego elementu w losowe miejsce
        public static void insertTasks(List<int> tasks)
        {
            Random rand = new Random();
            int firstElementToIinsert = rand.Next(0, tasks.Count);
            int newPlace = rand.Next(0, tasks.Count + 1);
            tasks.Insert(newPlace, tasks[firstElementToIinsert]);
            tasks.RemoveAt(firstElementToIinsert);
        }

        // funkcja akceptacji
        public static double acceptanceFunction(int Cmax, int NewCmax, double temperature)
        {
            if (Cmax <= NewCmax)
            {
                return Math.Exp((Cmax - NewCmax) / temperature);
                //return 0;
            }
            else
            {
                return 1;
            }
        }

        public static void simulatedAnnealing(List<Machine> Machines, int numberOfModification)
        {
            int helpCmax = 0; // Cmax pomocne przy porównaniu nowo otrzymanego
            double temperature = 10000; // temperatura poczatkowa

            // testy dla roznych temperatur poczatkowych
            // if(numberOfModification == 1){ temperature = 5000;}
            //else if(numberOfModification == 2){ temperature = 1000;}

            Random rand = new Random(); // obiekt do wykonywanie operacji rand 

            List<int> tasks = new List<int>(); // lista z taskami
            List<int> helpTasks = new List<int>(); // lista robocza
            int Cmax = -1; // Cmax podstawowe

            /*if(numberOfModification == 1){
            // lista zadan w kolejnosci najmniejszy najwiekszy
            for(int i = 1; i<=Machine.numberOfTasks; i++){
                tasks.Add(i);
            }
            Cmax = calculateCMaxWithAcceleration(Machines, tasks);
            //koniec chronologiczne listy zadan
            }*/

            //lista zadan w kolejnosci losowej 
            List<int> randTasksHelper = new List<int>();
            for (int i = 1; i <= Machine.numberOfTasks; i++)
            {
                randTasksHelper.Add(i);
            }
            for (int i = 1; i <= Machine.numberOfTasks; i++)
            {
                int randInt = rand.Next(0, (randTasksHelper.Count));
                tasks.Add(randTasksHelper[randInt]);
                randTasksHelper.RemoveAt(randInt);
            }


            Cmax = calculateCMaxWithAcceleration(Machines, tasks);
            //koniec listy losowej

            /* if(numberOfModification == 2){
            //poczatek NEH
              List<TimeWithTask> weights = getWeights(Machines);
             tasks.Add(weights[0].NumberTask); // dodanie pierwszego elementu
             for (int i = 1; i < Machine.numberOfTasks; i++)
             {
                 tasks.Add(weights[i].NumberTask); // dodawanie kolejnych elementów
                 Cmax = calculateCMaxWithAcceleration(Machines, tasks); // wyliczanie Cmax
             }
             //koniec
             }*/

            for (int j = 0; j < tasks.Count; j++)
            {
                helpTasks.Add(tasks[j]);
            } // uzupelnienie listy pomocniczej tak jak podstawowa
            int ipp = 0;

            // testy przy zmianie wspolczynnika chlodzenia
            //double fi = 0;
            //if(numberOfModification == 1){ fi = 0.8;}
            //else if(numberOfModification == 2){ fi = 0.9;}
            //else if(numberOfModification == 3){ fi = 0.99;}

            // testy z liczba iteracji
            //int numberOfItterations = 0;
            //if(numberOfModification == 1){ numberOfItterations = 50000;}
            //else if(numberOfModification == 2){ numberOfItterations = 10000;}

            for (; ipp < 100000;) // petla wykonywania SW
            {
                swapTasks(helpTasks); // zadanie stworznia nowej listy taskow

                // testy z róznymi funkcjami tworzenia nowych list taskow
                //if(numberOfModification == 1){ swapTasks(helpTasks);}
                //else if(numberOfModification == 2){ swapNeighboringTasks(helpTasks);}
                //else if(numberOfModification == 3){ insertTasks(helpTasks);}


                helpCmax = calculateCMax(Machines, helpTasks); // wyliczenie nowego Cmax
                if (Cmax != helpCmax) // warunek do testow
                {
                    if (acceptanceFunction(Cmax, helpCmax, temperature) >= rand.NextDouble()) // akceptacja nowej wartosci
                    {
                        for (int j = 0; j < tasks.Count; j++) // spelnienie warunku i przypisanie podstawowej tablicy tej pomocnej
                        {
                            tasks[j] = helpTasks[j];
                        }
                        Cmax = helpCmax;

                    }
                    else
                    { // niespelnienie warunku i odwrotne przypisanie tablic
                        for (int j = 0; j < tasks.Count; j++)
                        {
                            helpTasks[j] = tasks[j];
                        }
                    }
                    ipp++; // zwiekszenie iteracji
                    temperature *= 0.95; // zmiana temperatury
                }
            }
            Console.WriteLine(Cmax); // wypisanie Cmax
        }

    }
}

