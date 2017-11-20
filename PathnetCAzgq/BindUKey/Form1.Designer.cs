namespace BindUKey
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
            this.txtUserCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCaId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtImgPath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCaName = new System.Windows.Forms.TextBox();
            this.txtReadUKey = new System.Windows.Forms.Button();
            this.btnBindUser = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtUserCode
            // 
            this.txtUserCode.Location = new System.Drawing.Point(221, 29);
            this.txtUserCode.Name = "txtUserCode";
            this.txtUserCode.Size = new System.Drawing.Size(316, 38);
            this.txtUserCode.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(89, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 32);
            this.label1.TabIndex = 1;
            this.label1.Text = "用户名:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "加密串:";
            // 
            // txtCaId
            // 
            this.txtCaId.Location = new System.Drawing.Point(221, 172);
            this.txtCaId.Name = "txtCaId";
            this.txtCaId.ReadOnly = true;
            this.txtCaId.Size = new System.Drawing.Size(799, 38);
            this.txtCaId.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 245);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(191, 32);
            this.label3.TabIndex = 5;
            this.label3.Text = "签名图片地址:";
            // 
            // txtImgPath
            // 
            this.txtImgPath.Location = new System.Drawing.Point(221, 242);
            this.txtImgPath.Name = "txtImgPath";
            this.txtImgPath.Size = new System.Drawing.Size(797, 38);
            this.txtImgPath.TabIndex = 4;
            this.txtImgPath.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 32);
            this.label4.TabIndex = 7;
            this.label4.Text = "UKey姓名:";
            // 
            // txtCaName
            // 
            this.txtCaName.Location = new System.Drawing.Point(221, 103);
            this.txtCaName.Name = "txtCaName";
            this.txtCaName.ReadOnly = true;
            this.txtCaName.Size = new System.Drawing.Size(316, 38);
            this.txtCaName.TabIndex = 6;
            // 
            // txtReadUKey
            // 
            this.txtReadUKey.Location = new System.Drawing.Point(609, 91);
            this.txtReadUKey.Name = "txtReadUKey";
            this.txtReadUKey.Size = new System.Drawing.Size(177, 61);
            this.txtReadUKey.TabIndex = 8;
            this.txtReadUKey.Text = "读取";
            this.txtReadUKey.UseVisualStyleBackColor = true;
            this.txtReadUKey.Click += new System.EventHandler(this.txtReadUKey_Click);
            // 
            // btnBindUser
            // 
            this.btnBindUser.Location = new System.Drawing.Point(843, 91);
            this.btnBindUser.Name = "btnBindUser";
            this.btnBindUser.Size = new System.Drawing.Size(177, 61);
            this.btnBindUser.TabIndex = 8;
            this.btnBindUser.Text = "绑定";
            this.btnBindUser.UseVisualStyleBackColor = true;
            this.btnBindUser.Click += new System.EventHandler(this.btnBindUser_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 325);
            this.Controls.Add(this.btnBindUser);
            this.Controls.Add(this.txtReadUKey);
            this.Controls.Add(this.txtCaName);
            this.Controls.Add(this.txtImgPath);
            this.Controls.Add(this.txtCaId);
            this.Controls.Add(this.txtUserCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "朗珈CA用户绑定程序";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUserCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCaId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtImgPath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCaName;
        private System.Windows.Forms.Button txtReadUKey;
        private System.Windows.Forms.Button btnBindUser;
    }
}

