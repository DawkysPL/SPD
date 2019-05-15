using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shrage
{
    class Algorithms
    {
        public static int Carlier(List<Task> Tasks, List<Task> FinallyTasks, CarlierParametrs obiect)
        {
            int i = 0;
            //KROK 1
            obiect.U = AlgorithmShrage(Tasks, FinallyTasks);

            //KROK 2
            if (obiect.U < obiect.UB)
            {
                obiect.UB = obiect.U;
                obiect.OptimalFinallyTasks = FinallyTasks;
            }

            //KROK 3
            find_B(obiect);
            find_A(obiect);
            find_C(obiect);

            //KROK 4
            if (obiect.c == -1)
            {
                return obiect.U; // znaczy ze zwykly Shrage znalazl rozwiazanie optymalne
            }
            //KROK 5
            for (i = obiect.c + 1; i <= obiect.b; i++)
            {
                obiect.R_prim = Math.Min(obiect.R_prim, obiect.OptimalFinallyTasks[i].R);
                obiect.P_prim += obiect.OptimalFinallyTasks[i].P;
                obiect.Q_prim = Math.Min(obiect.Q_prim, obiect.OptimalFinallyTasks[i].Q);
            }

            //KROK 6
            if (obiect.R_mem == -2)
            {
                obiect.R_mem = obiect.OptimalFinallyTasks[obiect.c].R;
                obiect.NR_mem = obiect.OptimalFinallyTasks[obiect.c].Id;
            }
            obiect.OptimalFinallyTasks[obiect.c].R = Math.Max(obiect.OptimalFinallyTasks[obiect.c].R, obiect.R_prim + obiect.P_prim);

            //KROK 7
            //nie przemyslany kod i takie cos musi byc ;c
            List<Task> CopyTable = new List<Task>();
            List<Task> CopyTable1 = new List<Task>();
            foreach (var element in obiect.OptimalFinallyTasks)
            {
                CopyTable1.Add(element);
            }
            obiect.LB = AlgorithmShrageWithSegregatedTasksUseHeap(CopyTable1, CopyTable);

            // KROK 8
            List<Task> CopyTable2 = new List<Task>();
            List<Task> CopyTable3 = new List<Task>();
            foreach (var element in obiect.OptimalFinallyTasks)
            {
                CopyTable3.Add(element);
            }

            if (obiect.LB < obiect.UB)
            {
                //KROK 9
                CarlierParametrs obiectTestowy = new CarlierParametrs();
                obiectTestowy.UB = obiect.UB;
                obiect.UB = Carlier(CopyTable3, CopyTable2, obiectTestowy);
            }

            //KROK 10
            foreach (var element in obiect.OptimalFinallyTasks)
            {
                if (obiect.NR_mem == element.Id)
                {
                    element.R = obiect.R_mem;
                }
            }
            obiect.R_mem = -2;

            //KROK 11
            if (obiect.Q_mem == -2)
            {
                obiect.NR_mem = obiect.OptimalFinallyTasks[obiect.c].Id;
                obiect.Q_mem = obiect.OptimalFinallyTasks[obiect.c].Q;
            }
            obiect.OptimalFinallyTasks[obiect.c].Q = Math.Max(obiect.OptimalFinallyTasks[obiect.c].Q, obiect.Q_prim + obiect.P_prim);

            // KROK 12
            //nie przemyslany kod i takie cos musi byc ;c
            List<Task> CopyTable4 = new List<Task>();
            List<Task> CopyTable5 = new List<Task>();
            foreach (var element in obiect.OptimalFinallyTasks)
            {
                CopyTable5.Add(element);
            }
            obiect.LB = AlgorithmShrageWithSegregatedTasksUseHeap(CopyTable5, CopyTable4);

            //KROK 13
            List<Task> CopyTable6 = new List<Task>();
            List<Task> CopyTable7 = new List<Task>();
            foreach (var element in obiect.OptimalFinallyTasks)
            {
                CopyTable7.Add(element);
            }

            if (obiect.LB < obiect.UB)
            {
                //KROK 14
                CarlierParametrs obiectTestowy1 = new CarlierParametrs();
                obiectTestowy1.UB = obiect.UB;
                obiect.UB = Carlier(CopyTable7, CopyTable6, obiectTestowy1);
            }

            // KROK 15
            foreach (var element in obiect.OptimalFinallyTasks)
            {
                if (obiect.NR_mem == element.Id)
                {
                    element.Q = obiect.Q_mem;
                }
            }
            obiect.Q_mem = -2;

            return obiect.U;
        }

        public static void find_B(CarlierParametrs obiect)
        {
            for (int i = obiect.OptimalFinallyTasks.Count - 1; i > 0; i--)
            {
                if (obiect.U == obiect.OptimalFinallyTasks[i].Time + obiect.OptimalFinallyTasks[i].Q)
                {
                    obiect.b = i;
                    break;
                }
            }
        }

        public static void find_A(CarlierParametrs obiect)
        {
            int suma, i;
            int a = 0;
            int b = obiect.b;

            for (; a < b; a++)
            {
                suma = 0;
                for (i = a; i <= b; i++)
                {
                    suma += obiect.OptimalFinallyTasks[i].P;
                }

                if (obiect.U == obiect.OptimalFinallyTasks[a].R + obiect.OptimalFinallyTasks[b].Q + suma)
                {
                    obiect.a = a;
                    return;
                }
            }
            obiect.a = a;
            return;
        }

        public static void find_C(CarlierParametrs obiect)
        {
            int c = obiect.c, i;

            for (i = obiect.b; i >= obiect.a; i--)
            {
                if (obiect.OptimalFinallyTasks[i].Q < obiect.OptimalFinallyTasks[obiect.b].Q)
                {
                    c = i;
                    break;
                }
            }
            obiect.c = c;
        }

        public static int AlgorithmShrage(List<Task> Tasks, List<Task> FinallyTasks)
        {
            //init
            int t = 0, k = 0, Cmax = 0;
            bool helper = true;
            List<Task> ReadyTasks = new List<Task>(); //G


            while (ReadyTasks.Count != 0 || Tasks.Count != 0)
            {
                while (Tasks.Count != 0 && AreTasksReadyToGo(Tasks, t))
                {
                    GetReadyTasks(Tasks, ReadyTasks, t);
                    helper = true;
                }

                if (ReadyTasks.Count == 0)
                {
                    t = GetTime(Tasks);
                    helper = false;
                }

                if (helper)
                {
                    GetFromReadyTasksMaxQTask(ReadyTasks, FinallyTasks, ref t, ref Cmax);
                    k++;
                }
            }
            // Console.WriteLine("cos"); TUTAJ DEBUG POINT ABY SPRAWDZIC CMax
            return Cmax;
        }

        public static int AlgorithmShrageWithSegregatedTasks(List<Task> Tasks, List<Task> FinallyTasks)
        {
            int t = 0, Cmax = 0, l = 0, q0 = int.MaxValue - 1;
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
                    GetFromReadyTasksMaxQTaskWithModification(ReadyTasks, FinallyTasks, ref t, ref Cmax, ref l);
                }
            }
            // Console.WriteLine("cos"); TUTAJ DEBUG POINT ABY SPRAWDZIC CMax
            return Cmax;

        }

        public static bool AreTasksReadyToGo(List<Task> Tasks, int t)
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

        public static void GetFromReadyTasksMaxQTask(List<Task> ReadyTasks, List<Task> FinallyTasks, ref int t, ref int Cmax)
        {
            int MaxValueQ = ReadyTasks.Max(x => x.Q);
            int IndexTask = ReadyTasks.FindIndex(x => x.Q == MaxValueQ);

            FinallyTasks.Add(ReadyTasks[IndexTask]);
            t += ReadyTasks[IndexTask].P;
            Cmax = Math.Max(Cmax, t + ReadyTasks[IndexTask].Q);
            ReadyTasks.RemoveAt(IndexTask);
        }

        public static void GetReadyTasksWithModification(List<Task> Tasks, List<Task> ReadyTasks, ref int t, int l, List<Task> FinallyTask)
        {
            int helper = t;
            int IndexTask = Tasks.FindIndex(x => x.R <= helper);
            int Qe = Tasks[IndexTask].Q;
            int Re = Tasks[IndexTask].R;
            ReadyTasks.Add(Tasks[IndexTask]);
            Tasks.RemoveAt(IndexTask);

            if (l != 0 && FinallyTask.Count != 0)
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

        public static void AlgorithmShrageUseHeap(List<Task> Tasks, List<Task> FinallyTasks) // Schrage na 4.5
        {
            //init

            int t = 0, Cmax = 0;
            bool helper = true; 
            LowHeap unassignedTasks = new LowHeap(); // inicjalizacja nieposortowanych zadan
            for (int i = 0; i < Tasks.Count; i++)
            {
                unassignedTasks.Insert(Tasks[i]);
            }
            MaxHeap ReadyTasks = new MaxHeap(); // inicjalizacja zadan gotowych do realizacji


            while (ReadyTasks.HeapSize != 0 || unassignedTasks.HeapSize != 0) // dopoki sa zadania 
            {
                while (unassignedTasks.HeapSize != 0 && unassignedTasks.returnMin().R <= t) // kiedy jest jakies zadanie czekajace
                {
                    //GetReadyTasks(Tasks, ReadyTasks, t);
                    ReadyTasks.Insert(unassignedTasks.returnMin()); // przezucenie zadania z nieuporzadkowanych do gotowych
                    unassignedTasks.DeleteMin();
                    helper = true;
                }

                if (ReadyTasks.HeapSize == 0) // gdy nie ma zadan gotowych zmiana wartosci t
                {
                    t = unassignedTasks.returnMin().R;
                    helper = false;
                }

                if (helper)
                {
                    GetFromReadyTasksMaxQTask(ReadyTasks, FinallyTasks, ref t, ref Cmax); // obliczenie Cmax oraz przezucenie zadanaia z gotowych na liste wykonanych
                }
            }
             Console.WriteLine("cos"); //TUTAJ DEBUG POINT ABY SPRAWDZIC CMax
        }

        public static void GetFromReadyTasksMaxQTask(MaxHeap ReadyTasks, List<Task> FinallyTasks, ref int t, ref int Cmax) // obliczenie Cmax i przezucenie zadania z gotowych na wykonane
        {
            int MaxValueQ = ReadyTasks.returnMax().Q;
            //int IndexTask = ReadyTasks.FindIndex(x => x.Q == MaxValueQ);

            FinallyTasks.Add(ReadyTasks.returnMax()); // dodanie zadania z gotowych na wykonane
            t += ReadyTasks.returnMax().P; // wylicznie nowego t
            Cmax = Math.Max(Cmax, t + ReadyTasks.returnMax().Q); // obliczenie Cmax
            ReadyTasks.DeleteMax(); // usuniecie wykonanego zadania z gotowych
        }

        public static int AlgorithmShrageWithSegregatedTasksUseHeap(List<Task> Tasks, List<Task> FinallyTasks) // Schrage na 5.0
        {
            int t = 0, Cmax = 0, l = 0;
            bool helper = true;
            LowHeap unassignedTasks = new LowHeap(); // inicjalizacja nieposortowanych zadan
            for (int i = 0; i < Tasks.Count; i++)
            {
                unassignedTasks.Insert(Tasks[i]);
            }
            MaxHeap ReadyTasks = new MaxHeap(); // inicjalizacja zadan gotowych do realizacji


            while (ReadyTasks.HeapSize != 0 || unassignedTasks.HeapSize != 0) // dopoki sa zadania 
            {
                while (unassignedTasks.HeapSize != 0 && unassignedTasks.returnMin().R <= t) // kiedy jest jakies zadanie czekajace
                {
                   
                    GetReadyTasksWithModificationUseHeap(unassignedTasks, ReadyTasks, ref t, l, FinallyTasks); // zastosowanie przerwania lub zwykle przekaznie zadania do gotowych 
                    helper = true;
                }

                if (ReadyTasks.HeapSize == 0) // gdy nie ma zadan gotowych zmiana wartosci t
                {
                    t = unassignedTasks.returnMin().R;
                    helper = false;
                }

                if (helper)
                {
                    GetFromReadyTasksMaxQTaskWithModificationUseHeap(ReadyTasks, FinallyTasks, ref t, ref Cmax, ref l);
                }
            }
            return Cmax; //TUTAJ DEBUG POINT ABY SPRAWDZIC CMax

        }


        public static void GetReadyTasksWithModificationUseHeap(LowHeap Tasks, MaxHeap ReadyTasks, ref int t, int l, List<Task> FinallyTask) // przerwanie i/lub przezucenie zadania z nieporzadkowanej do gotowych
        {
            int helper = t;
            int Qe = Tasks.returnMin().Q; // wartosc q dla zadania ktore ma przejsc na gotowe
            int Re = Tasks.returnMin().R; // wartosc r dla zadania ktore ma przejsc na gotowe
            ReadyTasks.Insert(Tasks.returnMin()); // dodanie zadania do gotowych
            Tasks.DeleteMin(); // usuniecie zadania z nieuporzadkowanego kopca

            if (l != 0 && FinallyTask.Count != 0) //  jesli aktualnie wykonuje sie zadanie
            {
                int IndexTaskActual = FinallyTask.FindIndex(x => x.Id == l);
                if (IndexTaskActual == -1)
                    return;
                int Ql = FinallyTask[IndexTaskActual].Q;
                if (Qe > Ql) // jezeli q nowego zadania jest dluzsze
                {
                    FinallyTask[IndexTaskActual].P = t - Re; // zredukowanie p wykonywanego zadania
                    t = Re;

                    if (FinallyTask[IndexTaskActual].P > 0)
                    {
                        ReadyTasks.Insert(FinallyTask[IndexTaskActual]); // wrzucenie zaaktualizowanego zadania wykonywanego na kopiec gotowych
                        FinallyTask.RemoveAt(IndexTaskActual);
                    }
                }
            }
        }

        public static void GetFromReadyTasksMaxQTaskWithModificationUseHeap(MaxHeap ReadyTasks, List<Task> FinallyTasks, ref int t, ref int Cmax, ref int l) // obliczenie Cmax i przezucenie zadania z gotowych na wykonane
        {
            int MaxValueQ = ReadyTasks.returnMax().Q; 

            FinallyTasks.Add(ReadyTasks.returnMax()); // dodanie zadania do wykonanych 
            t += ReadyTasks.returnMax().P; // nowe t
            Cmax = Math.Max(Cmax, t + ReadyTasks.returnMax().Q); //oblicznie Cmax
            l = ReadyTasks.returnMax().Id; // id aktualnie wykonywanego zadnia
            ReadyTasks.DeleteMax(); // usuniecie zadania z gotowych
        }

    }

}
