namespace ResendReport
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSqlWhere = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnResend = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dteShsj2 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBrlb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dteShsj1 = new System.Windows.Forms.DateTimePicker();
            this.txtBlk = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.fBLKDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fBRLBDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fBLHDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fXMDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fBRBHDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fSPARE5DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fSQXHDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fXBDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fNLDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fZYHDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fMZHDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tJCXXBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.lblStates = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tJCXXBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblStates);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtSqlWhere);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.btnResend);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.dteShsj2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtBrlb);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dteShsj1);
            this.panel1.Controls.Add(this.txtBlk);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1862, 266);
            this.panel1.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(171, 209);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(617, 32);
            this.label6.TabIndex = 11;
            this.label6.Text = "SQL条件必须以and或者or开头,查询的表是JCXX";
            // 
            // txtSqlWhere
            // 
            this.txtSqlWhere.Location = new System.Drawing.Point(177, 157);
            this.txtSqlWhere.Name = "txtSqlWhere";
            this.txtSqlWhere.Size = new System.Drawing.Size(1421, 38);
            this.txtSqlWhere.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(136, 32);
            this.label5.TabIndex = 9;
            this.label5.Text = "SQL条件:";
            // 
            // btnResend
            // 
            this.btnResend.Location = new System.Drawing.Point(1404, 44);
            this.btnResend.Name = "btnResend";
            this.btnResend.Size = new System.Drawing.Size(194, 68);
            this.btnResend.TabIndex = 8;
            this.btnResend.Text = "重传";
            this.btnResend.UseVisualStyleBackColor = true;
            this.btnResend.Click += new System.EventHandler(this.btnResend_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(1141, 44);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(194, 68);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(478, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 32);
            this.label4.TabIndex = 7;
            this.label4.Text = "审核时间2:";
            // 
            // dteShsj2
            // 
            this.dteShsj2.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dteShsj2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dteShsj2.Location = new System.Drawing.Point(659, 89);
            this.dteShsj2.Name = "dteShsj2";
            this.dteShsj2.Size = new System.Drawing.Size(356, 38);
            this.dteShsj2.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(478, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(151, 32);
            this.label3.TabIndex = 5;
            this.label3.Text = "审核时间1:";
            // 
            // txtBrlb
            // 
            this.txtBrlb.Location = new System.Drawing.Point(177, 92);
            this.txtBrlb.Name = "txtBrlb";
            this.txtBrlb.Size = new System.Drawing.Size(254, 38);
            this.txtBrlb.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "病人类别:";
            // 
            // dteShsj1
            // 
            this.dteShsj1.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dteShsj1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dteShsj1.Location = new System.Drawing.Point(659, 35);
            this.dteShsj1.Name = "dteShsj1";
            this.dteShsj1.Size = new System.Drawing.Size(356, 38);
            this.dteShsj1.TabIndex = 2;
            // 
            // txtBlk
            // 
            this.txtBlk.Location = new System.Drawing.Point(177, 35);
            this.txtBlk.Name = "txtBlk";
            this.txtBlk.Size = new System.Drawing.Size(254, 38);
            this.txtBlk.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(51, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "病例库:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 266);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1862, 810);
            this.panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fBLKDataGridViewTextBoxColumn,
            this.fBRLBDataGridViewTextBoxColumn,
            this.fBLHDataGridViewTextBoxColumn,
            this.fXMDataGridViewTextBoxColumn,
            this.fBRBHDataGridViewTextBoxColumn,
            this.fSPARE5DataGridViewTextBoxColumn,
            this.fSQXHDataGridViewTextBoxColumn,
            this.fXBDataGridViewTextBoxColumn,
            this.fNLDataGridViewTextBoxColumn,
            this.fZYHDataGridViewTextBoxColumn,
            this.fMZHDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.tJCXXBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 40;
            this.dataGridView1.Size = new System.Drawing.Size(1862, 810);
            this.dataGridView1.TabIndex = 0;
            // 
            // fBLKDataGridViewTextBoxColumn
            // 
            this.fBLKDataGridViewTextBoxColumn.DataPropertyName = "F_BLK";
            this.fBLKDataGridViewTextBoxColumn.HeaderText = "病例库";
            this.fBLKDataGridViewTextBoxColumn.Name = "fBLKDataGridViewTextBoxColumn";
            this.fBLKDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fBRLBDataGridViewTextBoxColumn
            // 
            this.fBRLBDataGridViewTextBoxColumn.DataPropertyName = "F_BRLB";
            this.fBRLBDataGridViewTextBoxColumn.HeaderText = "病人类别";
            this.fBRLBDataGridViewTextBoxColumn.Name = "fBRLBDataGridViewTextBoxColumn";
            this.fBRLBDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fBLHDataGridViewTextBoxColumn
            // 
            this.fBLHDataGridViewTextBoxColumn.DataPropertyName = "F_BLH";
            this.fBLHDataGridViewTextBoxColumn.HeaderText = "病理号";
            this.fBLHDataGridViewTextBoxColumn.Name = "fBLHDataGridViewTextBoxColumn";
            this.fBLHDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fXMDataGridViewTextBoxColumn
            // 
            this.fXMDataGridViewTextBoxColumn.DataPropertyName = "F_XM";
            this.fXMDataGridViewTextBoxColumn.HeaderText = "姓名";
            this.fXMDataGridViewTextBoxColumn.Name = "fXMDataGridViewTextBoxColumn";
            this.fXMDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fBRBHDataGridViewTextBoxColumn
            // 
            this.fBRBHDataGridViewTextBoxColumn.DataPropertyName = "F_BRBH";
            this.fBRBHDataGridViewTextBoxColumn.HeaderText = "编号";
            this.fBRBHDataGridViewTextBoxColumn.Name = "fBRBHDataGridViewTextBoxColumn";
            this.fBRBHDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fSPARE5DataGridViewTextBoxColumn
            // 
            this.fSPARE5DataGridViewTextBoxColumn.DataPropertyName = "F_SPARE5";
            this.fSPARE5DataGridViewTextBoxColumn.HeaderText = "审核时间";
            this.fSPARE5DataGridViewTextBoxColumn.Name = "fSPARE5DataGridViewTextBoxColumn";
            this.fSPARE5DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fSQXHDataGridViewTextBoxColumn
            // 
            this.fSQXHDataGridViewTextBoxColumn.DataPropertyName = "F_SQXH";
            this.fSQXHDataGridViewTextBoxColumn.HeaderText = "申请序号";
            this.fSQXHDataGridViewTextBoxColumn.Name = "fSQXHDataGridViewTextBoxColumn";
            this.fSQXHDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fXBDataGridViewTextBoxColumn
            // 
            this.fXBDataGridViewTextBoxColumn.DataPropertyName = "F_XB";
            this.fXBDataGridViewTextBoxColumn.HeaderText = "性别";
            this.fXBDataGridViewTextBoxColumn.Name = "fXBDataGridViewTextBoxColumn";
            this.fXBDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fNLDataGridViewTextBoxColumn
            // 
            this.fNLDataGridViewTextBoxColumn.DataPropertyName = "F_NL";
            this.fNLDataGridViewTextBoxColumn.HeaderText = "年龄";
            this.fNLDataGridViewTextBoxColumn.Name = "fNLDataGridViewTextBoxColumn";
            this.fNLDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fZYHDataGridViewTextBoxColumn
            // 
            this.fZYHDataGridViewTextBoxColumn.DataPropertyName = "F_ZYH";
            this.fZYHDataGridViewTextBoxColumn.HeaderText = "住院号";
            this.fZYHDataGridViewTextBoxColumn.Name = "fZYHDataGridViewTextBoxColumn";
            this.fZYHDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // fMZHDataGridViewTextBoxColumn
            // 
            this.fMZHDataGridViewTextBoxColumn.DataPropertyName = "F_MZH";
            this.fMZHDataGridViewTextBoxColumn.HeaderText = "门诊号";
            this.fMZHDataGridViewTextBoxColumn.Name = "fMZHDataGridViewTextBoxColumn";
            this.fMZHDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tJCXXBindingSource
            // 
            this.tJCXXBindingSource.DataSource = typeof(SendPisResult.Models.T_JCXX);
            // 
            // lblStates
            // 
            this.lblStates.AutoSize = true;
            this.lblStates.ForeColor = System.Drawing.Color.Blue;
            this.lblStates.Location = new System.Drawing.Point(981, 209);
            this.lblStates.Name = "lblStates";
            this.lblStates.Size = new System.Drawing.Size(0, 32);
            this.lblStates.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1862, 1076);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "结果重传";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tJCXXBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dteShsj1;
        private System.Windows.Forms.TextBox txtBlk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dteShsj2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtBrlb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnResend;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn fBLKDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fBRLBDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fBLHDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fXMDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fBRBHDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fSPARE5DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fSQXHDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fXBDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fNLDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fZYHDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn fMZHDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource tJCXXBindingSource;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSqlWhere;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblStates;
    }
}

