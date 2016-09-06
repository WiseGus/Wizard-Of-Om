using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WizardOfOm {
    internal class SQLDrawer {

        internal static void DrawKnownColors(RichTextBox ctrl) {
            int currentCarret = ctrl.SelectionStart;
            AntiFlicker.BeginUpdate(ctrl.Handle);
            try {
                ctrl.SelectAll();
                ctrl.SelectionColor = Color.Black;
                ctrl.DeselectAll();
                DrawKnownColors(ctrl, Color.Gray, true, "inner", "left", "right", "join", "and", "or", "is null", "in", "exists", "not", "null");
                DrawKnownColors(ctrl, Color.Blue, true, "select", "top", "distinct", "from", "where", "order by", "group by", "having", "asc", "desc", "case", "when", "else", "end", "then");
                DrawKnownColors(ctrl, Color.FromArgb(255, 64, 255), true, "avg", "sum", "min", "count", "max", "coalesce");
                DrawKnownColors(ctrl, Color.Brown, false, "'.*?'");
            }
            finally {
                ctrl.Select(currentCarret, 0);
                AntiFlicker.EndUpdate(ctrl.Handle);
                ctrl.Invalidate();
            }
        }

        private static void DrawKnownColors(RichTextBox ctrl, Color color, bool useWholeWord, params string[] values) {
            foreach (string value in values) {
                string finalValue = useWholeWord ? "\\b" + value + "\\b" : value;
                MatchCollection matches = Regex.Matches(ctrl.Text, finalValue, RegexOptions.IgnoreCase);
                foreach (Match match in matches) {
                    ctrl.Select(match.Index, match.Length);
                    ctrl.SelectionColor = color;
                }
            }
        }
    }
}
