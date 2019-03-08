using System;
using System.Collections.Generic;
namespace SPD1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] text = System.IO.File.ReadAllLines(@"test.txt"); // SPD1 -> BIN -> DEBUG
            string daneStartowe = text[0];
            WczytajLiczbeZadanMaszyn(daneStartowe);
            // tworzenie maszyn
            List<Maszyna> Maszyny = new List<Maszyna>();
            for (int j = 0; j < Maszyna.liczbaMaszyn; j++)
            {
            Maszyny.Add(new Maszyna(Maszyna.liczId++));
            }
            //poprawny podzial czasu na dane zadania
            WczytajDaneDoMaszyn(Maszyny,text);
            Algorytmy.johnsonAlgoritm(Maszyny);
            Algorytmy.Permutacje(Maszyna.liczbaZadan,Maszyny);
        }
        #region MyLoadFunctions
        static string LaczStringi(int a, int b)
        {
            string laczymy = a.ToString() + b.ToString();
            return laczymy;
        }

        static void WczytajLiczbeZadanMaszyn(string daneStartowe)
        {
            string[] result = daneStartowe.Split(new char[] {' '},StringSplitOptions.RemoveEmptyEntries);
            Maszyna.liczbaZadan = Convert.ToInt32(result[0]);
            Maszyna.liczbaMaszyn = Convert.ToInt32(result[1]);
        }
        static void WczytajDaneDoMaszyn(List<Maszyna> Maszyny, string[] text)
        {
            string[] result;
            for (int i = 1; i <= Maszyna.liczbaZadan; i++)
            {
                result = text[i].Split(new char[] {' '},StringSplitOptions.RemoveEmptyEntries);
                foreach (var maszyna in Maszyny)
                {
                    maszyna.TimeWithTask.Add(new TimeWithTask(i,Convert.ToInt32(result[maszyna.Id-1])));
                }
            }
        }
        #endregion
    }
}
