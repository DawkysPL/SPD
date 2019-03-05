using System;
using System.Collections.Generic;
namespace SPD1
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] text = System.IO.File.ReadAllLines(@"test.txt"); // SPD1 -> BIN -> DEBUG
            string daneStartowe = text[0].TrimStart(' ');
            WczytajLiczbeZadanMaszyn(daneStartowe);
            //Console.WriteLine($"To jest liczba zadan: {Maszyna.liczbaZadan}, a to maszyny: {Maszyna.liczbaMaszyn}");

            // tworzenie maszyn
            List<Maszyna> Maszyny = new List<Maszyna>();
            for (int j = 0; j < Maszyna.liczbaMaszyn; j++)
            {
            Maszyny.Add(new Maszyna(Maszyna.liczId++));
            }

            //poprawny podzial czasu na dane zadania
            WczytajDaneDoMaszyn(Maszyny,text);
            Console.ReadLine();
        }
        #region MyLoadFunctions
        static string LaczStringi(int a, int b)
        {
            string laczymy = a.ToString() + b.ToString();
            return laczymy;
        }
        static void WczytajLiczbeZadanMaszyn(string daneStartowe)
        {
            if (daneStartowe[1].ToString() == " ")
                Maszyna.liczbaZadan = Convert.ToInt32(daneStartowe[0].ToString()); //DODAC OBSLUGE WIEKSZEJ LICZBY ZADAN I MASZYN NARAZIE DLA POJEDYNCZYCH CYFR
            else
            {
                if (daneStartowe[2].ToString() == " ")
                {
                    Maszyna.liczbaZadan = Convert.ToInt32(LaczStringi(
                        Convert.ToInt32(daneStartowe[0].ToString()),
                        Convert.ToInt32(daneStartowe[1].ToString())));
                }
                else
                {
                    string dwaZlaczone = LaczStringi(
                        Convert.ToInt32(daneStartowe[0].ToString()),
                        Convert.ToInt32(daneStartowe[1].ToString()));
                    Maszyna.liczbaZadan = Convert.ToInt32(dwaZlaczone + daneStartowe[2]);
                }
            }

            if (daneStartowe[daneStartowe.Length - 2].ToString() == " ")
                Maszyna.liczbaMaszyn = Convert.ToInt32(daneStartowe[daneStartowe.Length - 1].ToString());
            else
            {
                Maszyna.liczbaMaszyn = Convert.ToInt32(LaczStringi(
                    Convert.ToInt32(daneStartowe[daneStartowe.Length - 2].ToString()),
                    Convert.ToInt32(daneStartowe[daneStartowe.Length - 1].ToString())));
            }
        }
        static void WczytajDaneDoMaszyn(List<Maszyna> Maszyny, string[] text)
        {
            string NewString;
            for (int i = 1; i < text.Length; i++)
            {
                NewString = text[i];
                foreach (var maszyna in Maszyny)
                {
                    NewString = NewString.TrimStart(' ');
                    int czas = -1;
                    if (NewString[1] != ' ')
                    {
                        czas = Convert.ToInt32(NewString[0].ToString() + NewString[1].ToString());
                    }
                    else
                    {
                        czas = Convert.ToInt32(NewString[0].ToString());
                    }
                    maszyna.TimeWithTask.Add(new TimeWithTask(i, czas));
                    NewString = NewString.Remove(0, 2); 
                }
            }
        }
        #endregion
    }
}
