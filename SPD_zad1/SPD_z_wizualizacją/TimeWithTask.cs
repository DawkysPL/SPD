namespace SPD_z_wizualizacją
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
        public int NumerTaska { get; private set; }
        public int Time { get; private set; } //zmienilem z private set by miec latwiejszy dostep
    }
}
