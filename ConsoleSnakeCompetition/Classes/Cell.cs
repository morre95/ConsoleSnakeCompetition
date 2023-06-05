namespace ConsoleSnakeCompetition.Classes
{
    class Cell
    {
        public int X
        {
            get; set;
        }
        public int Y
        {
            get; set;
        }
        public int GCost
        {
            get; set;
        }
        public int HCost
        {
            get; set;
        }
        public int TotalCost
        {
            get
            {
                return GCost + HCost;
            }
        }
        public Cell Parent
        {
            get; set;
        }

        public Cell(int x, int y, int gCost, int hCost, Cell parent)
        {
            X = x;
            Y = y;
            GCost = gCost;
            HCost = hCost;
            Parent = parent;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Cell)obj;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
    }
}


