using System.Drawing;

namespace ConsoleSnakeCompetition;

internal class Program
{
    static void Main(string[] args)
    {
        int rows = 20, cols = 80;
        string toPrint = "";
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                if (row == 0 || row > rows - 2 || col == 0 || col > cols - 2)
                {
                    toPrint += "*";
                }
                else
                {
                    toPrint += " ";
                }
            }
            toPrint += "\n";
        }

        Console.WriteLine(toPrint);
    }

    
}
