using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shrage
{
    class CarlierParametrs
    {
        public CarlierParametrs()
        {
            OptimalFinallyTasks = new List<Task>();
            a = 0;
            b = 0;
            c = -1;
            U = 0;
            UB = int.MaxValue;
            LB = 0;
            R_prim = int.MaxValue;
            P_prim = 0;
            Q_prim = int.MaxValue;
            R_mem = -2;
            Q_mem = -2;
            NR_mem = 0;
        }
        public List<Task> OptimalFinallyTasks;
        public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
        public int U { get; set; }
        public int UB { get; set; }
        public int LB { get; set; }
        public int R_prim { get; set; }
        public int P_prim { get; set; }
        public int Q_prim { get; set; }
        public int R_mem { get; set; }
        public int Q_mem { get; set; }
        public int NR_mem { get; set; }
    }
}
