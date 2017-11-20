namespace LGHISJKZGQ
{
    partial class ZSDXYKYYSQForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colpatientid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPatientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPatientSex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRequestDepartmentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRequestDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFunctionRequestID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-mm-dd";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dateTimePicker1.Location = new System.Drawing.Point(83, 5);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(118, 21);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "起始时间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(229, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "结束时间：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(445, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "提交科室：";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy-mm-dd";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(300, 5);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(120, 21);
            this.dateTimePicker2.TabIndex = 4;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(516, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(663, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "查询";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colpatientid,
            this.colPatientName,
            this.colPatientSex,
            this.colage,
            this.colRequestDepartmentName,
            this.colRequestDateTime,
            this.colFunctionRequestID});
            this.dataGridView1.Location = new System.Drawing.Point(12, 32);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(742, 231);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            // 
            // colpatientid
            // 
            this.colpatientid.DataPropertyName = "PatientID";
            this.colpatientid.HeaderText = "患者ID";
            this.colpatientid.Name = "colpatientid";
            this.colpatientid.ReadOnly = true;
            // 
            // colPatientName
            // 
            this.colPatientName.DataPropertyName = "PatientName";
            this.colPatientName.HeaderText = "患者姓名";
            this.colPatientName.Name = "colPatientName";
            this.colPatientName.ReadOnly = true;
            this.colPatientName.Width = 80;
            // 
            // colPatientSex
            // 
            this.colPatientSex.DataPropertyName = "PatientSex";
            this.colPatientSex.HeaderText = "性别";
            this.colPatientSex.Name = "colPatientSex";
            this.colPatientSex.ReadOnly = true;
            this.colPatientSex.Width = 55;
            // 
            // colage
            // 
            this.colage.DataPropertyName = "PatientAge";
            this.colage.HeaderText = "年龄";
            this.colage.Name = "colage";
            this.colage.ReadOnly = true;
            this.colage.Width = 55;
            // 
            // colRequestDepartmentName
            // 
            this.colRequestDepartmentName.DataPropertyName = "RequestDepartmentName";
            this.colRequestDepartmentName.HeaderText = "申请科室";
            this.colRequestDepartmentName.Name = "colRequestDepartmentName";
            this.colRequestDepartmentName.ReadOnly = true;
            // 
            // colRequestDateTime
            // 
            this.colRequestDateTime.DataPropertyName = "RequestDateTime";
            this.colRequestDateTime.HeaderText = "申请时间";
            this.colRequestDateTime.Name = "colRequestDateTime";
            this.colRequestDateTime.ReadOnly = true;
            // 
            // colFunctionRequestID
            // 
            this.colFunctionRequestID.DataPropertyName = "FunctionRequestID";
            this.colFunctionRequestID.HeaderText = "技诊申请单ID";
            this.colFunctionRequestID.Name = "colFunctionRequestID";
            this.colFunctionRequestID.ReadOnly = true;
            // 
            // ZSDXYKYYSQForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 275);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ZSDXYKYYSQForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查询申请";
            this.Load += new System.EventHandler(this.GDSYSQForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colpatientid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPatientSex;
        private System.Windows.Forms.DataGridViewTextBoxColumn colage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRequestDepartmentName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRequestDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFunctionRequestID;
    }
}