using System.Collections.Generic;

namespace SPD1
{
    class Maszyna
    {
        public Maszyna(int _id)
        {
            Id = _id;
            TimeWithTask = new List<TimeWithTask>();
        }

        public Maszyna()
        {
        }
        public static int liczId = 1;
        public static int liczbaMaszyn = 0;
        public static int liczbaZadan = 0;

        public int Id { get; private set; }
        public List<TimeWithTask> TimeWithTask;
    }
}
