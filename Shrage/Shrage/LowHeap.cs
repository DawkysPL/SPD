using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shrage
{
    class LowHeap
    {
        private IList<Task> data;
        public int HeapSize;

        public LowHeap()
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

        public void Insert(Task element)
        {
            HeapSize++;
            data.Add(element);
            int index = HeapSize;
            while (index > 1)
            {
                if (element.R < data[index / 2].R) Swap(index, (index / 2));
                else break;
                index = index / 2;
            }
        }

        public void ReapairAfterDelete(int index)
        {
            int helpIndex = index;
            if (index <= HeapSize)
            {
                int value = data[index].R;
                while (helpIndex * 2 <= HeapSize)
                {
                    int newIndex;
                    if ((helpIndex * 2 < HeapSize) && (data[helpIndex * 2 + 1].R < data[helpIndex * 2].R))
                        newIndex = helpIndex * 2 + 1;
                    else
                        newIndex = helpIndex * 2;
                    if (value > data[newIndex].R)
                        Swap(helpIndex, newIndex);
                    else break;
                    helpIndex = newIndex;
                }
            }

        }

        public void DeleteMin()
        {
            data[1] = data[HeapSize];
            data.RemoveAt(HeapSize);
            HeapSize--;
            ReapairAfterDelete(1);
        }

        public Task returnMin()
        {
            return data[1];
        }

        public void Show()
        {
            foreach(Task element in data)
            {
                Console.WriteLine(element.R);
            }
        }
    }
}
