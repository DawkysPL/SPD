
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SPD_z_wizualizacją
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 

       
    public partial class MainWindow : Window
    {
   
        public MainWindow()
        {
             KlasaLosujaca losuj = new KlasaLosujaca();
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
            int[,] graphValues = new int[Maszyna.liczbaMaszyn, Maszyna.liczbaZadan];
            graphValues = Algorytmy.johnsonAlgoritm(Maszyny);
            //Algorytmy.Permutacje(Maszyna.liczbaZadan,Maszyny);
            InitializeComponent();

            List<int> CmaxAllMachines = new List<int>();
            for (int i = 0; i < Maszyna.liczbaMaszyn; i++)
            {
                CmaxAllMachines.Add(graphValues[i, Maszyna.liczbaZadan - 1]);
            }
            var maxVal = CmaxAllMachines.Max();

             TextBox graphCmax = new TextBox();
            graphCmax.Margin = new System.Windows.Thickness(300, 0, 0, 0);
            graphCmax.Width = 200;
            graphCmax.Height = 30;
            graphCmax.Text = "Cmax = " + maxVal.ToString();
            graphCmax.Foreground = Brushes.RoyalBlue;
            graphCmax.FontSize = 22;
            graphCmax.TextAlignment = TextAlignment.Center;
            RootGird.Children.Add(graphCmax);

            TextBox[,] graph = new TextBox[Maszyna.liczbaMaszyn, Maszyna.liczbaZadan];


            graph[0,0] = new TextBox();
            graph[0,0].Margin = new System.Windows.Thickness(0, 100, 0, 0);
            graph[0,0].Width = graphValues[0,0]*10;
            graph[0,0].Height = 30;
            graph[0,0].Background = Brushes.RoyalBlue; 
            graph[0,0].Text = graphValues[0,0].ToString();
            graph[0,0].TextAlignment = TextAlignment.Center;
            RootGird.Children.Add(graph[0,0]);

            for(int j = 1; j<Maszyna.liczbaZadan; j++){
            graph[0,j] = new TextBox();
            graph[0,j].Margin = new System.Windows.Thickness(graphValues[0,j-1]*10, 100, 0, 0);
            graph[0,j].Width = (graphValues[0,j] - graphValues[0,j-1])*10;
            graph[0,j].Height = 30;
            graph[0,j].Background = Brushes.RoyalBlue; 
            graph[0,j].Text = (graphValues[0,j] - graphValues[0,j-1]).ToString();
            graph[0,j].TextAlignment = TextAlignment.Center;
            RootGird.Children.Add(graph[0,j]);
                }

            for(int k = 1; k<Maszyna.liczbaMaszyn; k++){

            graph[k,0] = new TextBox();
            graph[k,0].Margin = new System.Windows.Thickness(graphValues[k-1,0]*10, (k+1)*100, 0, 0);
            graph[k,0].Width = (graphValues[k,0] - graphValues[k-1,0])*10;
            graph[k,0].Height = 30;
            graph[k,0].Background = Brushes.RoyalBlue; 
            graph[k,0].Text = (graphValues[k,0] - graphValues[k-1,0]).ToString();
            graph[k,0].TextAlignment = TextAlignment.Center;
            RootGird.Children.Add(graph[k,0]);
            
            for(int j = 1; j<Maszyna.liczbaZadan; j++){
            
            graph[k,j] = new TextBox();
            graph[k,j].Margin = new System.Windows.Thickness(Math.Max(graphValues[k,j-1], graphValues[k-1,j])*10, (k+1)*100, 0, 0);
            graph[k,j].Width = (graphValues[k,j] - Math.Max(graphValues[k,j-1], graphValues[k-1,j]))*10;
            graph[k,j].Height = 30;
            graph[k,j].Background = Brushes.RoyalBlue; 
            graph[k,j].Text = (graphValues[k,j] - Math.Max(graphValues[k,j-1], graphValues[k-1,j])).ToString();
            graph[k,j].TextAlignment = TextAlignment.Center;
            RootGird.Children.Add(graph[k,j]);
           
            }
        }

            }
        static string LaczStringi(int a, int b)
        {
            string laczymy = a.ToString() + b.ToString();
            return laczymy;
        }

        static void WczytajLiczbeZadanMaszyn(string daneStartowe)
        {
           
            string[] result = daneStartowe.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Maszyna.liczbaZadan = Convert.ToInt32(result[0]);
            Maszyna.liczbaMaszyn = Convert.ToInt32(result[1]);
        }
        static void WczytajDaneDoMaszyn(List<Maszyna> Maszyny, string[] text)
        {
            string[] result;
            for (int i = 1; i <= Maszyna.liczbaZadan; i++)
            {
                result = text[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var maszyna in Maszyny)
                {
                    maszyna.TimeWithTask.Add(new TimeWithTask(i, Convert.ToInt32(result[maszyna.Id - 1])));
                }
            }
        }

    }
}
