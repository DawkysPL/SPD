using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD_z_wizualizacją
{
    class WynikAlgorytmu
    {
       public WynikAlgorytmu()
        {

        }
       public  WynikAlgorytmu(int index, int cmax)
        {
            Index = index;
            Cmax = cmax;
        }
        public int Index { get; private set; }
        public int Cmax { get; private set; }
    }
}
