namespace SPD2
{
    class ResultOfAlgorithm
    {
        public ResultOfAlgorithm()
        {

        }
        public ResultOfAlgorithm(int index, int cmax)
        {
            IndexMachineWhoLastEnd = index;
            Cmax = cmax;
        }
        public int IndexMachineWhoLastEnd { get; private set; }
        public int Cmax { get; private set; }
    }
}
