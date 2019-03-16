using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD2
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] text = System.IO.File.ReadAllLines(@"test.txt"); // SPD1 -> BIN -> DEBUG
            string dataStart = text[0];
            LoadNumbersOfTasksAndMachines(dataStart);
            // creating machines
            List<Machine> Machines = new List<Machine>();
            for (int j = 0; j < Machine.numberOfMachines; j++)
            {
                Machines.Add(new Machine(Machine.countId++));
            }
            //get data into machine
            LoadDataIntoMachines(Machines, text);
            Algorithms.algorithmNEH(Machines);

            Console.ReadLine();
        }

        static void LoadNumbersOfTasksAndMachines(string dataStart)
        {
            string[] result = dataStart.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Machine.numberOfTasks = Convert.ToInt32(result[0]);
            Machine.numberOfMachines = Convert.ToInt32(result[1]);
        }
        static void LoadDataIntoMachines(List<Machine> Machines, string[] text)
        {
            string[] result;
            for (int i = 1; i <= Machine.numberOfTasks; i++)
            {
                result = text[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var machine in Machines)
                {
                    machine.TimeWithTask.Add(new TimeWithTask(i, Convert.ToInt32(result[machine.Id - 1])));
                }
            }
        }
    }
}
