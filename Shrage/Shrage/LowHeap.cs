using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shrage
{
    class LowHeap
    {
        private IList<int> data;
        public int HeapSize;

        public LowHeap()
        {
            data = new List<int>();
            HeapSize = 0;
            data.Add(0);
        }

        public void Swap(int index1, int index2)
        {
            int help = data[index1];
            data[index1] = data[index2];
            data[index2] = help;
        }

        public void Insert(int element)
        {
            HeapSize++;
            data.Add(element);
            int index = HeapSize;
            while (index > 1)
            {
                if (element < data[index / 2]) Swap(index, (index / 2));
                else break;
                index = index / 2;
            }
        }

        public void ReapairAfterDelete(int index)
        {
            int helpIndex = index;
            int value = data[index];
            while (helpIndex * 2 <= HeapSize)
            {
                int newIndex;
                if ((helpIndex * 2 < HeapSize) && (data[helpIndex * 2 + 1] < data[helpIndex * 2]))
                    newIndex = helpIndex * 2 + 1;
                else
                    newIndex = helpIndex * 2;
                if (value > data[newIndex])
                    Swap(helpIndex, newIndex);
                else break;
                helpIndex = newIndex;
            }

        }

        public void DeleteMin()
        {
            data[1] = data[HeapSize];
            data.RemoveAt(HeapSize);
            HeapSize--;
            ReapairAfterDelete(1);
        }

        public int returnMin()
        {
            return data[1];
        }

        public void Show()
        {
            foreach(int element in data)
            {
                Console.WriteLine(element);
            }
        }
    }
}
