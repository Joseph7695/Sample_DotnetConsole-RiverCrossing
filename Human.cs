namespace riverCrossing
{
    class Human
    {
        public bool canRowBoat()
        {
            if (this.type == HumanType.Dad ||
            this.type == HumanType.Mum ||
            this.type == HumanType.Policeman)
            { return true; }
            else { return false; }
        }
        public Human(HumanType type)
        {
            this.type = type;
            this.position = Position.Source;
        }
        public HumanType type;
        public Position position;
    }
}