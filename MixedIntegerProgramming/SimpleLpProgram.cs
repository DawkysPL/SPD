
// [START program]
using System;
using System.Collections.Generic;
using System.IO;
using Google.OrTools.LinearSolver;
using Google.OrTools.Sat;
using spd;

namespace Shrage
{
   
    class Program
    {
        private static void Witi_SOLUTION_CP()
        {
            string[] text = File.ReadAllLines(@"test.txt"); //SPD/SPD/BIN/DEBUG
            string dataStart = text[0];
            string[] result = dataStart.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            HowManyTasks = Convert.ToInt32(result[0]);
            List<WiTi> witiTasks = new List<WiTi>();

            for (int i = 1; i <= HowManyTasks; i++)
            {
                result = text[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                witiTasks.Add(new WiTi { P = Convert.ToInt32(result[0]), W = Convert.ToInt32(result[1]), D = Convert.ToInt32(result[2]) });
            }

            CpModel model = new CpModel();

            var variablesMaxValue = 0;
            var czas_pracy = 0;

            foreach(var element in witiTasks)
            {
                czas_pracy += element.P;
            }
            foreach (var element in witiTasks)
            {
                variablesMaxValue += element.W * (czas_pracy - element.D);
            }



            IntVar[,] alfas = new IntVar[witiTasks.Count, witiTasks.Count];

            for (int i = 0; i < witiTasks.Count; i++)
            {
                for (int j = 0; j < witiTasks.Count; j++)
                {
                    alfas[i, j] = model.NewIntVar(0, variablesMaxValue, $"{i}g{j}");
                }
            }

            IntVar[] delays = new IntVar[witiTasks.Count];
            for (int i = 0; i < witiTasks.Count; i++)
            {
                delays[i] = model.NewIntVar(0, variablesMaxValue, nameof(delays) + i.ToString());
            }

            IntVar[] starts = new IntVar[witiTasks.Count];
            IntVar[] ends = new IntVar[witiTasks.Count];
            for (int i = 0; i < witiTasks.Count; i++)
            {
                starts[i] = model.NewIntVar(0, variablesMaxValue, $"starts{i}");
                ends[i] = model.NewIntVar(0, variablesMaxValue, $"ends{i}");
            }


            for(int i=0; i<witiTasks.Count; i++)
            {
                model.Add(ends[i] == starts[i] + witiTasks[i].P);
                model.Add(delays[i] >= witiTasks[i].W * (ends[i] - witiTasks[i].D));
            }

            for (int i = 0; i < witiTasks.Count; i++)
            {
                for (int j = i + 1; j < witiTasks.Count; j++)
                {
                    model.Add(starts[i] + witiTasks[i].P <= starts[j] + alfas[i, j] * variablesMaxValue);
                    model.Add(starts[j] + witiTasks[j].P <= starts[i] + alfas[j, i] * variablesMaxValue);
                    model.Add(alfas[i, j] + alfas[j, i] == 1);
                }
            }
            var cummulated_delay = delays.Sum();
            model.Minimize(cummulated_delay);

            CpSolver solver = new CpSolver();

            var resultStatus = solver.Solve(model);
            if (resultStatus != CpSolverStatus.Optimal)
            {
                Console.WriteLine("Solve nie znalazl optymala");
            }
            else
            {
                List<Results> results = new List<Results>();
                Console.WriteLine("Obiect_value = " + solver.Value(cummulated_delay));
                for(int k=0; k <starts.Length; k++)
                {
                    results.Add(new Results { idTask = k+1, start = solver.Value(starts[k]), end = solver.Value(ends[k]) });
                }
                results.Sort((p,q) => p.start.CompareTo(q.start));
                foreach(var element in results)
                {
                    Console.WriteLine($"{element.idTask} | {element.start} | {element.end}");
                }
            }
        }

        private static void Witi_SOLUTION()
        {
            Solver solver = Solver.CreateSolver("SimpleMipProgram", "CBC_MIXED_INTEGER_PROGRAMMING");

            string[] text = File.ReadAllLines(@"test.txt"); //SPD/SPD/BIN/DEBUG
            string dataStart = text[0];
            string[] result = dataStart.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            HowManyTasks = Convert.ToInt32(result[0]);
            List<WiTi> witiTasks = new List<WiTi>();

            for (int i = 1; i <= HowManyTasks; i++)
            {
                result = text[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                witiTasks.Add(new WiTi { P = Convert.ToInt32(result[0]), W = Convert.ToInt32(result[1]), D = Convert.ToInt32(result[2]) });
            }
            int Time = 0;
            int variablesMaxValue = 0;
            foreach (var element in witiTasks)
            {
                Time += element.P;
                int kara = 0;
                if(element.D < Time)
                {
                    kara = element.W * (Time - element.D);
                }
                variablesMaxValue += kara;
                Console.WriteLine(variablesMaxValue +" | "+ Time);
            }
            var alfas = solver.MakeIntVarMatrix(witiTasks.Count, witiTasks.Count, 0, 1);

            var starts = solver.MakeIntVarArray(witiTasks.Count, 0, Time);

            var ends = solver.MakeIntVarArray(witiTasks.Count, 0, Time);

            var cmax = solver.MakeIntVar(0, variablesMaxValue, "cmax");



            //int helpTime = 0;
            var suma = new LinearExpr();
            for (int i = 0; i < witiTasks.Count; i++)
            {
                solver.Add(ends[i] >= starts[i] + witiTasks[i].P);
                solver.Add(ends[i] >= witiTasks[i].D);
                solver.Add(cmax >= witiTasks[i].W * (ends[i] - witiTasks[i].D) + suma);

                
                suma += witiTasks[i].W * (ends[i] - witiTasks[i].D);

            }

            for (int i = 0; i < witiTasks.Count; i++)
            {
                for (int j = i + 1; j < witiTasks.Count; j++)
                {
                    solver.Add(starts[i] + witiTasks[i].P <= starts[j] + alfas[i, j] * variablesMaxValue);
                    solver.Add(starts[j] + witiTasks[j].P <= starts[i] + alfas[j, i] * variablesMaxValue);
                    solver.Add(alfas[i, j] + alfas[j, i] == 1);
                }
            }

            solver.Minimize(cmax);
            Solver.ResultStatus resultStatus = solver.Solve();
            if (resultStatus != Solver.ResultStatus.OPTIMAL)
            {
                Console.WriteLine("Solve nie znalazl optymala");
            } else
            {
                
                Console.WriteLine("Obiect_value = " + solver.Objective().Value());

            }     
        }

        private static void RPQ_Solution(List<Task> Tasks)
        {
            Solver solver = Solver.CreateSolver("SimpleMipProgram","CBC_MIXED_INTEGER_PROGRAMMING");

            int variablesMaxValue = 0;
            foreach(var element in Tasks)
            {
                variablesMaxValue += element.R + element.P + element.Q;
            }

            var alfas = solver.MakeIntVarMatrix(Tasks.Count, Tasks.Count, 0, 1); //tutaj moze byc inaczej patrz argumenty

            var starts = solver.MakeIntVarArray(Tasks.Count, 0, variablesMaxValue);

            var cmax = solver.MakeIntVar(0, variablesMaxValue, "cmax");

            

            foreach(var element in Tasks)
            {
                solver.Add(starts[element.Id] >= element.R);
            }

            foreach (var element in Tasks)
            {
                solver.Add(cmax >= starts[element.Id] + element.P + element.Q);
            }

            for(int i=0; i<Tasks.Count; i++)
            {
                for(int j = i+1; j< Tasks.Count; j++)
                {
                    var task1 = Tasks[i];
                    var task2 = Tasks[j];
                    solver.Add(starts[task1.Id] + task1.P <= starts[task2.Id] + alfas[task1.Id, task2.Id] * variablesMaxValue);
                    solver.Add(starts[task2.Id] + task2.P <= starts[task1.Id] + alfas[task2.Id, task1.Id] * variablesMaxValue);
                    solver.Add(alfas[task1.Id, task2.Id] + alfas[task2.Id, task1.Id] == 1);
                }
            }
            solver.Minimize(cmax);
            Solver.ResultStatus resultStatus = solver.Solve();
            if (resultStatus != Solver.ResultStatus.OPTIMAL)
            {
                Console.WriteLine("Solve nie znalazl optymala");
            }
            Console.WriteLine("Obiect_value = " + solver.Objective().Value());
        }

        private static void RPQ_Solution_CP(List<Task> Tasks)
        {
            CpModel model = new CpModel();

            IntVar[,] alfas = new IntVar[Tasks.Count, Tasks.Count];

          

            int variablesMaxValue = 0;
            foreach (var element in Tasks)
            {
                variablesMaxValue += element.R + element.P + element.Q;
            }

            for (int i = 0; i < Tasks.Count; i++)
            {
                for (int j = 0; j < Tasks.Count; j++)
                {
                    alfas[i, j] = model.NewIntVar(0, variablesMaxValue, $"{i}g{j}");
                }
            }

            IntVar[] starts = new IntVar[Tasks.Count];
            for(int i=0; i<Tasks.Count; i++)
            {
                starts[i] = model.NewIntVar(0, variablesMaxValue, $"starts{i}");
            }

            

            var cmax = model.NewIntVar(0, variablesMaxValue, "cmax");

            foreach (var element in Tasks)
            {
                model.Add(starts[element.Id] >= element.R);
            }

            foreach (var element in Tasks)
            {
                model.Add(cmax >= starts[element.Id] + element.P + element.Q);
            }

            for (int i = 0; i < Tasks.Count; i++)
            {
                for (int j = i + 1; j < Tasks.Count; j++)
                {
                    var task1 = Tasks[i];
                    var task2 = Tasks[j];
                    model.Add(starts[task1.Id] + task1.P <= starts[task2.Id] + alfas[task1.Id, task2.Id] * variablesMaxValue);
                    model.Add(starts[task2.Id] + task2.P <= starts[task1.Id] + alfas[task2.Id, task1.Id] * variablesMaxValue);
                    model.Add(alfas[task1.Id, task2.Id] + alfas[task2.Id, task1.Id] == 1);
                }
            }
            Console.WriteLine("mysle1");
            model.Minimize(cmax);
            Console.WriteLine("mysle2");


            CpSolver solver = new CpSolver();

            


            var resultStatus = solver.Solve(model);
            Console.WriteLine("mysle3");
            if (resultStatus != CpSolverStatus.Optimal)
            {
                Console.WriteLine("Solve nie znalazl optymala");
            }
            Console.WriteLine("Obiect_value = " + solver.Value(cmax));
        }

        public static int HowManyTasks;
        public static int HowManyLoadColumns; //alltimes 3

        static void Main(string[] args)
        {
            /*
            string[] text = File.ReadAllLines(@"test.txt"); //SPD/SPD/BIN/DEBUG
            string dataStart = text[0];
            LoadNumbersOfTasksAndMachines(dataStart);

            List<Task> FinallyTasks = new List<Task>();
            List<Task> Tasks = new List<Task>();
            LoadDataIntoTasks(Tasks, text);

           foreach(var element in Tasks)
            {
                Console.WriteLine(element.Id);
            }
            RPQ_Solution_CP(Tasks);
            */
            Witi_SOLUTION_CP();
            

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
                Tasks.Add(new Task { Id = i-1, R = Convert.ToInt32(result[0]), P = Convert.ToInt32(result[1]), Q = Convert.ToInt32(result[2]) });
            }
        }

    }
}
// [END program]
