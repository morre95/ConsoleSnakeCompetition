namespace ConsoleSnakeCompetition.Classes.Tournament
{
    public class Matchup
    {
        public Matchup(Player pA, Player pB = null)
        {
            PlayerA = pA;
            PlayerB = pB;
        }

        public Player PlayerA
        {
            get; set;
        }
        public Player PlayerB
        {
            get; set;
        }

        public override string ToString()
        {
            return PlayerB == null ? $"{PlayerA.Name} has a free run" : $"{PlayerA.Name} vs. {PlayerB.Name}";
        }

        public Player GetFavored()
        {
            return Player.GetWinner(PlayerA, PlayerB);
        }
    }
}
