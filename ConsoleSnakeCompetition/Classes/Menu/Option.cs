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


