using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD2
{
    class TimeWithTask
    {
        public TimeWithTask()
        {

        }
        public TimeWithTask(int _nrTask, int _time)
        {
            NumberTask = _nrTask;
            Time = _time;
        }
        public int NumberTask { get; private set; }
        public int Time { get; private set; }
    }
}
