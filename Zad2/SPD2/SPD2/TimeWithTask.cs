using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD2
{
    class TimeWithTask : IComparable<TimeWithTask>
    {
        public TimeWithTask()
        {

        }
        public TimeWithTask(int _nrTask, int _time)
        {
            NumberTask = _nrTask;
            Time = _time;
        }
        public int NumberTask { get; set; }
        public int Time { get; set; }

        public int CompareTo(TimeWithTask comparePart)
        {
            if (comparePart == null)
                return 1;
            else
            {
                return this.Time.CompareTo(comparePart.Time);
            }
        }
    }
}
