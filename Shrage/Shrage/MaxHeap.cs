using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shrage
{
    class MaxHeap
    {
        private IList<Task> data;
        public int HeapSize;

        public MaxHeap()
        {
            data = new List<Task>();
            HeapSize = 0;
            Task task = new Task();
            data.Add(task);
        }

        public void Swap(int index1, int index2)
        {
            Task help = data[index1];
            data[index1] = data[index2];
            data[index2] = help;
        }

        public void Insert( Task element)
        {
            HeapSize++;
            data.Add(element);
            int index = HeapSize;
            while (index > 1)
            {
                if (element.Q > data[index / 2].Q) Swap(index, (index / 2));
                else break;
                index = index / 2;
            }
        }

        public void ReapairAfterDelete(int index)
        {
            int helpIndex = index;
            if (index <= HeapSize)
            {
                int value = data[index].Q;

                while (helpIndex * 2 <= HeapSize)
                {
                    int newIndex;
                    if ((helpIndex * 2 < HeapSize) && (data[helpIndex * 2 + 1].Q > data[helpIndex * 2].Q))
                        newIndex = helpIndex * 2 + 1;
                    else
                        newIndex = helpIndex * 2;
                    if (value < data[newIndex].Q)
                        Swap(helpIndex, newIndex);
                    else break;
                    helpIndex = newIndex;
                }
            }
        }

        public void DeleteMax()
        {
            data[1] = data[HeapSize];
            data.RemoveAt(HeapSize);
            HeapSize--;
            ReapairAfterDelete(1);
        }

        public Task returnMax()
        {
            return data[1];
        }


        public void Show()
        {
            foreach (Task element in data)
            {
                Console.WriteLine(element.Q);
            }
        }
    }
}
