namespace ConsoleSnakeCompetition.Classes.Game
{
    public abstract class Grid<T>
    {
        private T[,] gridRepository;

        public Grid(int rows, int columns)
        {
            gridRepository = new T[rows, columns];
        }

        virtual public T GetValue(int rowNumber, int columnNumber)
        {
            return gridRepository[rowNumber, columnNumber];
        }

        virtual public void SetValue(int rowNumber, int columnNumber, T inputItem)
        {
            gridRepository[rowNumber, columnNumber] = inputItem;
        }

        virtual public int RowCount()
        {
            return gridRepository.GetLength(0);
        }

        virtual public int ColumnCount()
        {
            return gridRepository.GetLength(1);
        }
    }
}


