using System.Text;

namespace ConsoleSnakeCompetition.Utilities
{
    public interface ITextRow
    {
        string Output();
        void Output(StringBuilder sb);
        object Tag
        {
            get; set;
        }
    }
}


