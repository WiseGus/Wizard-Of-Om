using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WizardOfOm {
    internal class CsDrawer {

        private enum DrawColorType { Strings, Chars }

        internal static void DrawKnownColors(RichTextBox ctrl) {
            int currentCarret = ctrl.SelectionStart;
            AntiFlicker.BeginUpdate(ctrl.Handle);
            try {
                ctrl.SelectAll();
                ctrl.SelectionColor = Color.Black;
                ctrl.DeselectAll();
                DrawKnownColors(ctrl, Color.Blue, true, "new", "string", "int", "short", "long", "bool", "var");
                DrawKnownColors(ctrl, Color.Teal, true, "SelectQuery", "UpdateQuery", "WherePhrase", "WhereTerm", "WhereClause", "Int16", "Int32", "Int64", "Guid", "FromTerm", "CaseClause", "SqlExpression", "CaseTerm", "SelectColumn", "GroupByTerm", "slQueryParameters");
                DrawKnownColors(ctrl, Color.Green, true, "CompareOperator", "WhereClauseRelationship", "JoinType", "OrderByDirection", "SqlAggregationFunction");
                DrawKnownColors(ctrl, Color.DarkRed, DrawColorType.Strings);
                DrawKnownColors(ctrl, Color.Brown, DrawColorType.Chars);
                DrawComments(ctrl, Color.DarkGreen);
            }
            finally {
                ctrl.Select(currentCarret, 0);
                AntiFlicker.EndUpdate(ctrl.Handle);
                ctrl.Invalidate();
            }
        }

        private static void DrawKnownColors(RichTextBox ctrl, Color color, DrawColorType drawType) {
            string expr = string.Empty;
            switch (drawType) {
                case DrawColorType.Strings:
                    expr = "\".*?\"";
                    break;
                case DrawColorType.Chars:
                    expr = "'.*?'";
                    break;
            }
            DrawKnownColors(ctrl, color, false, expr);
        }

        private static void DrawKnownColors(RichTextBox ctrl, Color color, bool useWholeWord, params string[] values) {
            foreach (string value in values) {
                string finalValue = useWholeWord ? "\\b" + value + "\\b" : value;
                MatchCollection matches = Regex.Matches(ctrl.Text, finalValue);
                foreach (Match match in matches) {
                    ctrl.Select(match.Index, match.Length);
                    ctrl.SelectionColor = color;
                }
            }
        }

        private static void DrawComments(RichTextBox ctrl, Color color) {
            string finalValue = "//.*?\n";
            MatchCollection matches = Regex.Matches(ctrl.Text, finalValue);
            foreach (Match match in matches) {
                ctrl.Select(match.Index, match.Length);
                ctrl.SelectionColor = Color.DarkGreen;
            }
        }
    }
}
