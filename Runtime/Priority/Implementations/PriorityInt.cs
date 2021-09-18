namespace DoubleDash.CodingTools.Priority
{
    class PriorityInt : IPriority
    {
        public int Priority { get; set; }

        public PriorityInt(int priority)
        {
            this.Priority = priority;
        }
    }
}