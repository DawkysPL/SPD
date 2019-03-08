using System;
using System.IO;

namespace SPD1
{
    class KlasaLosujaca
    {
        public KlasaLosujaca()
        {
            LosowanieZadanMaszyn();
            czasZadan = new int[liczbaZadan,liczbaMaszyn];
            LosowanieCzasow();
            ZapiysywanieDoPliku();
        }

        private int liczbaZadan;
        private int liczbaMaszyn;
        private int[,] czasZadan;


        private void LosowanieZadanMaszyn()
        {
            Random rnd = new Random();
            int poczatek = 1;
            int koniec = 11;
            liczbaZadan = rnd.Next(poczatek, koniec);
            poczatek = 1;
            koniec = 4;
            liczbaMaszyn = rnd.Next(poczatek, koniec); 
        }
        private void LosowanieCzasow()
        {
            int poczatek = 2;
            int koniec = 50;
            Random rnd = new Random();
            for (int i = 0; i < liczbaZadan; i++)
            {
                for (int j = 0; j < liczbaMaszyn; j++)
                {
                    czasZadan[i,j] = rnd.Next(poczatek, koniec);
                }
            }
        }
        private void ZapiysywanieDoPliku()
        {
            string path = @"test.txt";
            StreamWriter sw = new StreamWriter(path);
            if (!File.Exists(path))
            {
                sw = File.CreateText(path);
            }
            sw.WriteLine($"  {liczbaZadan}  {liczbaMaszyn}");
            for (int i = 0; i < liczbaZadan; i++)
            {
                for (int j = 0; j < liczbaMaszyn; j++)
                {
                    sw.Write($"  {czasZadan[i,j]}");
                }
                sw.WriteLine();
            }
            sw.Close();
        }
    }
}
