using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shrage
{
    class MaxHeap
    {
        //Kopiec majacy w korzeniu maksymalna wartosc
        private IList<Task> data; 
        public int HeapSize;

        public MaxHeap() // inicjalizacja kopca
        {
            data = new List<Task>();
            HeapSize = 0;
            Task task = new Task(); // 0 indeks nie uwzgledniamy
            data.Add(task);
        }

        public void Swap(int index1, int index2) // zamiana dwuch elementow
        {
            Task help = data[index1];
            data[index1] = data[index2];
            data[index2] = help;
        }

        public void Insert( Task element) // Wstawianie nowego zadania w odpowiednie miesjsce, kryterium jest wartosc Q
        {
            HeapSize++;
            data.Add(element);
            int index = HeapSize;
            while (index > 1)
            {
                if (element.Q > data[index / 2].Q) Swap(index, (index / 2)); // dopuki q danego elementu jest wieksze niż przodek zamina
                else break;
                index = index / 2;
            }
        }

        public void ReapairAfterDelete(int index) // naprawa po usunieciu ktoregos z elementow
        {
            int helpIndex = index;
            if (index <= HeapSize) // wartosc indeksu nie przekroczyla dlugosci kopca
            {
                int value = data[index].Q; // wartoscia odniesienia jest q

                while (helpIndex * 2 <= HeapSize) // sprawdzamy czy nie przekraczamy zakresu
                {
                    int newIndex;
                    if ((helpIndex * 2 < HeapSize) && (data[helpIndex * 2 + 1].Q > data[helpIndex * 2].Q)) // decyzja co ma wylodaowac na miejscu zamienianego wartosc wieksza
                        newIndex = helpIndex * 2 + 1;
                    else
                        newIndex = helpIndex * 2;
                    if (value < data[newIndex].Q) // zamiana z przodkiem jesli warunek spelniony
                        Swap(helpIndex, newIndex);
                    else break;
                    helpIndex = newIndex;
                }
            }
        }

        public void DeleteMax() // usuniecie korzenia i zmniejszenie HeapSize
        {
            data[1] = data[HeapSize];
            data.RemoveAt(HeapSize);
            HeapSize--;
            ReapairAfterDelete(1); // naprawa po usunieciu
        }

        public Task returnMax() // zwraca wartosc korznia
        {
            return data[1];
        }


        public void Show() // wypisuje wszystkie elementy kopca
        {
            foreach (Task element in data)
            {
                Console.WriteLine(element.Q);
            }
        }
    }
}
