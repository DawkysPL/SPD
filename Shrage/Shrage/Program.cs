using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shrage
{
    class Program
    {
        public static int HowManyTasks;
        public static int HowManyLoadColumns; //alltimes 3

        static void Main(string[] args)
        {
            string[] text = File.ReadAllLines(@"test.txt"); //SPD/SPD/BIN/DEBUG
            string dataStart = text[0];
            LoadNumbersOfTasksAndMachines(dataStart);

            List<Task> FinallyTasks = new List<Task>();
            List<Task> Tasks = new List<Task>();
            LoadDataIntoTasks(Tasks, text);


            CarlierParametrs obiect = new CarlierParametrs();
            Algorithms.Carlier(Tasks, FinallyTasks, obiect);

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
                Tasks.Add(new Task { Id = i ,R = Convert.ToInt32(result[0]), P = Convert.ToInt32(result[1]), Q = Convert.ToInt32(result[2]) });
            }
        }
        
    }
}
