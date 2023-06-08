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

        public static Action DoNothing = () => { };

        public static Action NIY = () => { throw new NotImplementedException("Not Implemeted Yet"); };

        public Option(string name, Action? callback = null)
        {
            Name = name;
            Callback = callback == null ? DoNothing : callback;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}


