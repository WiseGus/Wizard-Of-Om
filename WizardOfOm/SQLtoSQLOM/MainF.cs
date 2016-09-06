using SQLtoOM.Engine;
using System;
using System.Drawing;
using System.Windows.Forms;
using WizardOfOm.Properties;

namespace WizardOfOm {
    public partial class MainF : Form {

        private SqlOMEngine _Engine;

        public MainF() {
            InitializeComponent();

            engineVersionToolStripMenuItem.Text += SqlOMEngine.GetVersion();
            msSQLToolStripMenuItem.Click += MsSQLToolStripMenuItem_Click;
            oracleToolStripMenuItem.Click += OracleToolStripMenuItem_Click;
            ctrlSql.TextChanged += CtrlSql_TextChanged;
        }

        protected override void OnShown(EventArgs e) {
            base.OnShown(e);
            SetCtrlData();
        }

        private string StripSql(string sql) {
            sql = sql
                .Replace(" as ", " ")
                .Replace(" AS ", " ")
                .Replace("[", "")
                .Replace("]", "")
                .Replace("(", " ")
                .Replace(")", "")
                .Replace("  ", " ")
                .Replace("\r", " ")
                .Replace("\n", " ")
                .Replace("\t", " ")
                .Trim()
                .ToUpper();

            sql = string.Join(" ", sql.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            return sql;
        }

        private void MsSQLToolStripMenuItem_Click(object sender, EventArgs e) {
            ConvertToSqlOm(RendererType.MsSQL);
        }

        private void OracleToolStripMenuItem_Click(object sender, EventArgs e) {
            ConvertToSqlOm(RendererType.Oracle);
        }

        private void SetCtrlData() {
            if (!Settings.Default.UseSampleSQL) return;

            string sql = @"SELECT TOP 10
REL.CMID RELATEDCONT_ID, 
((2 + COALESCE(CMCONTACTS.CMREVNUM, CMRESOURCES.CMREVNUM, 0)) / 2) CALC_VAL,
CMCONTACTS.CMID, 
CMRESOURCES.CMDESCRIPTION, 
1 A, 
'2' B,
GXUSER.GXLOGINNAME,
COUNT(1) COUNT_NO,
AVG(CMREVNUM) AVG_REVNUM,
SUM(CMREVNUM) TOTAL_REVNUM,
COALESCE(CMREFERENCENUM, 'UNKNOWN') CUSTOM_REFNUM,
CASE WHEN CMISACCOUNT = 1 AND CMISPERSON = 0 THEN 'ACCOUNT'
WHEN CMISACCOUNT = 0 AND CMISPERSON = 1 THEN 'PERSON'
WHEN CMISCOMPANY = 1 THEN 'COMPANY'
ELSE 'UNKNOWN' END CONTACT_TYPE,
(SELECT TOP 1 CMTEXT CONT_MESSAGE
FROM CMCONTACTMESSAGES WHERE CMCONTACTS.CMID = CMCONTACTMESSAGES.CMCONTACTID AND CMCONTACTMESSAGES.CMWHATEVER = @subQryVal ORDER BY CMPOSTDATE DESC) CONTACT_MESSAGE
FROM CMCONTACTS
INNER JOIN CMCONTACTRELATIONS REL ON (REL.CMRELCONTID = CMCONTACTS.CMID OR REL.CMRELCONTID IS NULL)
LEFT JOIN CMRESOURCES ON CMCONTACTS.CMRESOURCEID = CMRESOURCES.CMID
LEFT JOIN GXUSER ON CMRESOURCES.CMUSERID = GXUSER.GXID
WHERE 
(CMCONTACTS.CMID IN ('id1', 'id2', 'id3')
AND EXISTS (SELECT CMID FROM CMCONTACTMESSAGES WHERE CMCONTACTS.CMID = CMCONTACTMESSAGES.CMCONTACTID))
OR (CMCONTACTS.CMACTIVE = @contActive AND 
(CMRESOURCES.CMDESCRIPTION = 'test' AND GXUSER.GXACTIVE = 1)) 
GROUP BY 
CMRESOURCES.CMACTIVE, 
CMCONTACTS.CMNAME, 
REL.CMID, 
CMCONTACTS.CMID, 
CMRESOURCES.CMDESCRIPTION, 
GXUSER.GXLOGINNAME, 
CMISACCOUNT, 
CMISPERSON, 
CMISCOMPANY,
CMREFERENCENUM
HAVING SUM(CMCONTACTS.CMREVNUM) > 1
ORDER BY 
CMCONTACTS.CMNAME ASC";
            ctrlSql.AppendText(sql);

            ConvertToSqlOm(RendererType.MsSQL);
        }

        private void ConvertToSqlOm(RendererType rendererType) {
            ctrlSqlOm.Clear();
            ctrlSqlOmToSql.Clear();
            if (string.IsNullOrEmpty(ctrlSql.Text)) return;

            string sqlOmText = string.Empty;
            string sqlOmToSqlText = string.Empty;

            Cursor = Cursors.WaitCursor;

            try {
                _Engine = new SqlOMEngine();
                _Engine.ParseSQL(rendererType, ctrlSql.Text);

                sqlOmText = _Engine.GetGeneratedSqlOm();
                sqlOmToSqlText = _Engine.GetGeneratedSql();

            }
            catch (Exception ex) {
                sqlOmText = ex.ToString();
                ctrlSqlOmToSql.Clear();
            }
            finally {
                ctrlSqlOm.AppendText(sqlOmText);
                ctrlSqlOmToSql.AppendText(sqlOmToSqlText);

                CsDrawer.DrawKnownColors(ctrlSqlOm);
                SQLDrawer.DrawKnownColors(ctrlSqlOmToSql);

                ctrlSqlOm.Select(0, 0);
                ctrlSqlOm.ScrollToCaret();

                ValidateGeneratedSql(rendererType);

                Cursor = Cursors.Default;
            }
        }

        private void ValidateGeneratedSql(RendererType rendererType) {
            if (string.IsNullOrEmpty(ctrlSql.Text)) return;

            string strippedOriginalSql = StripSql(ctrlSql.Text);
            string strippedSqlFromSqlOm = StripSql(ctrlSqlOmToSql.Text);

            bool sqlFromSqlOmIsValid = strippedOriginalSql.Equals(strippedSqlFromSqlOm);

            if (sqlFromSqlOmIsValid) {
                ctrlOperationResult.Text = "Success";
                ctrlOperationResult.ForeColor = Color.Green;
            }
            else if (string.IsNullOrEmpty(ctrlSqlOmToSql.Text)) {
                ctrlOperationResult.Text = "Error";
                ctrlOperationResult.ForeColor = Color.Red;
            }
            else {
                ctrlOperationResult.Text = "Warning";
                ctrlOperationResult.ForeColor = Color.Orange;
            }

            ctrlOperationResult.Text += $" ({rendererType.ToString()})";
        }

        private void CtrlSql_TextChanged(object sender, EventArgs e) {
            SQLDrawer.DrawKnownColors(ctrlSql);
        }
    }
}
