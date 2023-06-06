namespace ConsoleSnakeCompetition.Classes.Menu
{
    public class Option
    {
        public string Name
        {
            get;
        }
        public Action? Callback
        {
            get;
        }

        public Option(string name, Action? callback = null)
        {
            Name = name;
            Callback = callback;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}


