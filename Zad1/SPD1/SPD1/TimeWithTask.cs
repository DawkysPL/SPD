namespace SPD1
{
    class TimeWithTask
    {
        public TimeWithTask()
        {

        }

        public TimeWithTask(int _nrTaska, int _time)
        {
            NumerTaska = _nrTaska;
            Time = _time;
        }
        public int NumerTaska { get; set; }
        public int Time { get; set; }
    }
}
