using System.Text;

namespace ConsoleSnakeCompetition.Utilities
{
    public partial class TableBuilder
    {
        public string Separator
        {
            get; set;
        }

        protected List<ITextRow> rows = new List<ITextRow>();
        protected List<int> colLength = new List<int>();

        public TableBuilder()
        {
            Separator = "  ";
        }

        public ITextRow AddRow(params object[] cols)
        {
            var row = new TextRow(this);
            foreach (var o in cols)
            {
                var str = o.ToString().Trim();
                row.Add(str);
                if (colLength.Count >= row.Count)
                {
                    var curLength = colLength[row.Count - 1];
                    if (str.Length > curLength) colLength[row.Count - 1] = str.Length;
                }
                else
                {
                    colLength.Add(str.Length);
                }
            }
            rows.Add(row);
            return row;
        }

        protected string _fmtString = null;
        public string FormatString
        {
            get
            {
                if (_fmtString == null)
                {
                    var format = "";
                    var i = 0;
                    foreach (var len in colLength)
                    {
                        format += string.Format("{{{0},-{1}}}{2}", i++, len, Separator);
                    }
                    format += "\r\n";
                    _fmtString = format;
                }
                return _fmtString;
            }
        }

        public string Output()
        {
            var sb = new StringBuilder();
            foreach (TextRow row in rows)
            {
                row.Output(sb);
            }
            return sb.ToString();
        }
    }
}


