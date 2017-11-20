namespace BatchPdf2Jpeg
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
            this.btnSelectPdfs = new System.Windows.Forms.Button();
            this.txtOutPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelectOutPath = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbQuality = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSelectDir = new System.Windows.Forms.Button();
            this.lblWorkStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSelectPdfs
            // 
            this.btnSelectPdfs.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectPdfs.Location = new System.Drawing.Point(705, 434);
            this.btnSelectPdfs.Name = "btnSelectPdfs";
            this.btnSelectPdfs.Size = new System.Drawing.Size(360, 82);
            this.btnSelectPdfs.TabIndex = 0;
            this.btnSelectPdfs.Text = "选择文件并转换";
            this.btnSelectPdfs.UseVisualStyleBackColor = true;
            this.btnSelectPdfs.Click += new System.EventHandler(this.btnSelectPdfs_Click);
            // 
            // txtOutPath
            // 
            this.txtOutPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOutPath.Location = new System.Drawing.Point(186, 20);
            this.txtOutPath.Name = "txtOutPath";
            this.txtOutPath.Size = new System.Drawing.Size(687, 47);
            this.txtOutPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 39);
            this.label1.TabIndex = 2;
            this.label1.Text = "输出位置:";
            // 
            // btnSelectOutPath
            // 
            this.btnSelectOutPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectOutPath.Location = new System.Drawing.Point(893, 10);
            this.btnSelectOutPath.Name = "btnSelectOutPath";
            this.btnSelectOutPath.Size = new System.Drawing.Size(157, 66);
            this.btnSelectOutPath.TabIndex = 3;
            this.btnSelectOutPath.Text = "选择...";
            this.btnSelectOutPath.UseVisualStyleBackColor = true;
            this.btnSelectOutPath.Click += new System.EventHandler(this.btnSelectOutPath_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(180, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(640, 36);
            this.label2.TabIndex = 4;
            this.label2.Text = "输出位置为空时,JPEG生成在PDF源文件目录下";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(13, 140);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 39);
            this.label3.TabIndex = 5;
            this.label3.Text = "输出质量:";
            // 
            // cmbQuality
            // 
            this.cmbQuality.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbQuality.FormattingEnabled = true;
            this.cmbQuality.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cmbQuality.Location = new System.Drawing.Point(186, 136);
            this.cmbQuality.Name = "cmbQuality";
            this.cmbQuality.Size = new System.Drawing.Size(149, 47);
            this.cmbQuality.TabIndex = 6;
            this.cmbQuality.Text = "6";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(180, 197);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(841, 36);
            this.label4.TabIndex = 7;
            this.label4.Text = "数值越大质量越好,如果图片不模糊请不要改大,否则图片会很大";
            // 
            // btnSelectDir
            // 
            this.btnSelectDir.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelectDir.Location = new System.Drawing.Point(28, 434);
            this.btnSelectDir.Name = "btnSelectDir";
            this.btnSelectDir.Size = new System.Drawing.Size(360, 82);
            this.btnSelectDir.TabIndex = 0;
            this.btnSelectDir.Text = "选择目录并转换";
            this.btnSelectDir.UseVisualStyleBackColor = true;
            this.btnSelectDir.Click += new System.EventHandler(this.btnSelectDir_Click);
            // 
            // lblWorkStatus
            // 
            this.lblWorkStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblWorkStatus.ForeColor = System.Drawing.Color.Red;
            this.lblWorkStatus.Location = new System.Drawing.Point(12, 281);
            this.lblWorkStatus.Name = "lblWorkStatus";
            this.lblWorkStatus.Size = new System.Drawing.Size(1079, 103);
            this.lblWorkStatus.TabIndex = 8;
            this.lblWorkStatus.Text = "等待任务";
            this.lblWorkStatus.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1093, 528);
            this.Controls.Add(this.lblWorkStatus);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbQuality);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelectOutPath);
            this.Controls.Add(this.txtOutPath);
            this.Controls.Add(this.btnSelectDir);
            this.Controls.Add(this.btnSelectPdfs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "PDF批量转JPG";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectPdfs;
        private System.Windows.Forms.TextBox txtOutPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectOutPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbQuality;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSelectDir;
        private System.Windows.Forms.Label lblWorkStatus;
    }
}

