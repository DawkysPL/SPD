namespace SPD2
{
    class GraphVertice
    {
        public GraphVertice()
        {
            FinishTime = 0;
            ReverseTime = 0;
            TaskId = 0;
            TaskTime = 0;
        }
        public GraphVertice(int time, int time2)
        {
            FinishTime = time;
            ReverseTime = time2;
        }
        public int FinishTime { get; set; }
        public int ReverseTime { get; set; }
        public int TaskId { get; set; }
        public int TaskTime { get; set; }
    }
}
