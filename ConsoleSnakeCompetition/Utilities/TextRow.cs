using System.Text;

namespace ConsoleSnakeCompetition.Utilities
{
    internal class TextRow : List<string>, ITextRow
    {
        protected TableBuilder owner = null;
        public TextRow(TableBuilder Owner)
        {
            owner = Owner;
            if (owner == null) throw new ArgumentException("Owner");
        }
        public string Output()
        {
            var sb = new StringBuilder();
            Output(sb);
            return sb.ToString();
        }
        public void Output(StringBuilder sb)
        {
            sb.AppendFormat(owner.FormatString, ToArray());
        }
        public object Tag
        {
            get; set;
        }
    }
}


