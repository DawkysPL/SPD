using System.Collections.Generic;

namespace SPD2
{
    class Machine
    {
        public Machine(int _id)
        {
            Id = _id;
            TimeWithTask = new List<TimeWithTask>();
        }

        public Machine()
        {
        }
        public static int countId = 1;
        public static int numberOfMachines = 0;
        public static int numberOfTasks = 0;
        public int Id { get; private set; }
        public List<TimeWithTask> TimeWithTask;
    }
}
