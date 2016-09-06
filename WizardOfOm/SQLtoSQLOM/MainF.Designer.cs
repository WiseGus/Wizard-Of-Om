namespace WizardOfOm {
    partial class MainF {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainF));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ctrlTransform = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.engineVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.gusWiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ctrlSqlOmToSql = new System.Windows.Forms.RichTextBox();
            this.ctrlSql = new System.Windows.Forms.RichTextBox();
            this.ctrlSqlOm = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ctrlOperationResult = new System.Windows.Forms.Label();
            this.msSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.oracleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctrlTransform,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1264, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ctrlTransform
            // 
            this.ctrlTransform.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.msSQLToolStripMenuItem,
            this.oracleToolStripMenuItem});
            this.ctrlTransform.Name = "ctrlTransform";
            this.ctrlTransform.Size = new System.Drawing.Size(73, 20);
            this.ctrlTransform.Text = "Transform";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.engineVersionToolStripMenuItem,
            this.toolStripSeparator2,
            this.gusWiseToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // engineVersionToolStripMenuItem
            // 
            this.engineVersionToolStripMenuItem.Enabled = false;
            this.engineVersionToolStripMenuItem.Name = "engineVersionToolStripMenuItem";
            this.engineVersionToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.engineVersionToolStripMenuItem.Text = "Engine version: ";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(154, 6);
            // 
            // gusWiseToolStripMenuItem
            // 
            this.gusWiseToolStripMenuItem.Enabled = false;
            this.gusWiseToolStripMenuItem.Name = "gusWiseToolStripMenuItem";
            this.gusWiseToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.gusWiseToolStripMenuItem.Text = "Gus Wise";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.ctrlSqlOmToSql, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ctrlSql, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ctrlSqlOm, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ctrlOperationResult, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1264, 961);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // ctrlSqlOmToSql
            // 
            this.ctrlSqlOmToSql.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.SetColumnSpan(this.ctrlSqlOmToSql, 2);
            this.ctrlSqlOmToSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlSqlOmToSql.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.ctrlSqlOmToSql.Location = new System.Drawing.Point(3, 655);
            this.ctrlSqlOmToSql.Name = "ctrlSqlOmToSql";
            this.ctrlSqlOmToSql.ReadOnly = true;
            this.ctrlSqlOmToSql.Size = new System.Drawing.Size(373, 303);
            this.ctrlSqlOmToSql.TabIndex = 5;
            this.ctrlSqlOmToSql.Text = "";
            // 
            // ctrlSql
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ctrlSql, 2);
            this.ctrlSql.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlSql.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.ctrlSql.Location = new System.Drawing.Point(3, 43);
            this.ctrlSql.Name = "ctrlSql";
            this.ctrlSql.Size = new System.Drawing.Size(373, 566);
            this.ctrlSql.TabIndex = 2;
            this.ctrlSql.Text = "";
            // 
            // ctrlSqlOm
            // 
            this.ctrlSqlOm.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.SetColumnSpan(this.ctrlSqlOm, 2);
            this.ctrlSqlOm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlSqlOm.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(161)));
            this.ctrlSqlOm.Location = new System.Drawing.Point(382, 43);
            this.ctrlSqlOm.Name = "ctrlSqlOm";
            this.ctrlSqlOm.ReadOnly = true;
            this.tableLayoutPanel1.SetRowSpan(this.ctrlSqlOm, 3);
            this.ctrlSqlOm.Size = new System.Drawing.Size(879, 915);
            this.ctrlSqlOm.TabIndex = 1;
            this.ctrlSqlOm.Text = "";
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Poor Richard", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(310, 40);
            this.label1.TabIndex = 3;
            this.label1.Text = "Sql";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Poor Richard", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(634, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(627, 40);
            this.label2.TabIndex = 4;
            this.label2.Text = "SqlOm";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 2);
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Poor Richard", 20.25F);
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(3, 612);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(373, 40);
            this.label4.TabIndex = 7;
            this.label4.Text = "SqlOm to Sql";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ctrlOperationResult
            // 
            this.ctrlOperationResult.AutoSize = true;
            this.ctrlOperationResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlOperationResult.Font = new System.Drawing.Font("Poor Richard", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctrlOperationResult.Location = new System.Drawing.Point(382, 0);
            this.ctrlOperationResult.Name = "ctrlOperationResult";
            this.ctrlOperationResult.Size = new System.Drawing.Size(246, 40);
            this.ctrlOperationResult.TabIndex = 9;
            this.ctrlOperationResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // msSQLToolStripMenuItem
            // 
            this.msSQLToolStripMenuItem.Name = "msSQLToolStripMenuItem";
            this.msSQLToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.msSQLToolStripMenuItem.Text = "Ms SQL";
            // 
            // oracleToolStripMenuItem
            // 
            this.oracleToolStripMenuItem.Name = "oracleToolStripMenuItem";
            this.oracleToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.oracleToolStripMenuItem.Text = "Oracle";
            // 
            // MainF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 985);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainF";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "The wizard of Om™";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox ctrlSqlOm;
        private System.Windows.Forms.RichTextBox ctrlSql;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem ctrlTransform;
        private System.Windows.Forms.RichTextBox ctrlSqlOmToSql;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gusWiseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem engineVersionToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label ctrlOperationResult;
        private System.Windows.Forms.ToolStripMenuItem msSQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem oracleToolStripMenuItem;
    }
}

