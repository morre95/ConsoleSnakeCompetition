namespace ConsoleSnakeCompetition.Classes.Tournament
{
    public class Player
    {
        public Player(string name, int point)
        {
            Name = name;
            Points = point;
        }

        public string Name
        {
            get; set;
        }

        public int Points
        {
            get; set;
        }

        public static Player GetWinner(Player pA, Player pB)
        {
            return pB == null || pA.Points > pB.Points ? pA : pB;
        }
    }
}
