using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleSnakeCompetition.Classes.Game;

namespace ConsoleSnakeCompetition.Pages.GamePlay
{
    internal class AStar
    {
        public static Stack<Cell> Search(Grid<char> grid, int startX, int startY, int goalX, int goalY)
        {
            int gridRows = grid.RowCount();
            int gridColumns = grid.ColumnCount();

            List<Cell> openList = new List<Cell>();
            List<Cell> closedList = new List<Cell>();

            Cell start = new Cell(startX, startY, 0, CalculateHeuristic(startX, startY, goalX, goalY), null);
            Cell goal = new Cell(goalX, goalY, 0, 0, null);

            openList.Add(start);

            while (openList.Count > 0)
            {
                Cell current = openList[0];
                int currentIndex = 0;
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].TotalCost < current.TotalCost)
                    {
                        current = openList[i];
                        currentIndex = i;
                    }
                }

                openList.RemoveAt(currentIndex);
                closedList.Add(current);

                // Om målet är nått, returnera vägen
                if (current.Equals(goal))
                {
                    Stack<Cell> path = new Stack<Cell>();
                    while (current != null)
                    {
                        if (path.Contains(current))
                        {
                            Console.WriteLine("Circular reference detected. Unable to find path.");
                            return new Stack<Cell>();
                        }

                        path.Push(current);
                        current = current.Parent;
                    }

                    return path;
                }

                // (upp, ned, vänster, höger)
                int[] dx = { -1, 1, 0, 0 };
                int[] dy = { 0, 0, -1, 1 };

                for (int i = 0; i < 4; i++)
                {
                    int newX = current.X + dx[i];
                    int newY = current.Y + dy[i];

                    if (newX >= 0 && newX < gridRows && newY >= 0 && newY < gridColumns && grid.GetValue(newX, newY) != '*' && !CellInList(newX, newY, closedList))
                    {
                        int newCost = current.GCost + 1;

                        Cell neighbor = new Cell(newX, newY, newCost, CalculateHeuristic(newX, newY, goalX, goalY), current);

                        // Om grann noden redan finns i öppen lista och den har en högre kostnad, ignorera den
                        if (CellInList(newX, newY, openList) && newCost >= neighbor.GCost)
                        {
                            continue;
                        }

                        openList.Add(neighbor);
                    }
                }
            }

            return new Stack<Cell>();
        }

        public static bool CellInList(int x, int y, List<Cell> list)
        {
            foreach (Cell cell in list)
            {
                if (cell.X == x && cell.Y == y)
                {
                    return true;
                }
            }
            return false;
        }

        public static int CalculateHeuristic(int x, int y, int goalX, int goalY)
        {
            return Math.Abs(x - goalX) + Math.Abs(y - goalY);
        }
    }
}

