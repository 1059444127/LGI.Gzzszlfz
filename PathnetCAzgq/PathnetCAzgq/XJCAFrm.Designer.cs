namespace PathnetCAzgq
{
    partial class XJCAFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XJCAFrm));
            this.axXJFormSealX1 = new AxXJFormSeal.AxXJFormSealX();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.axXJFormSealX1)).BeginInit();
            this.SuspendLayout();
            // 
            // axXJFormSealX1
            // 
            this.axXJFormSealX1.Location = new System.Drawing.Point(3, 2);
            this.axXJFormSealX1.Name = "axXJFormSealX1";
            this.axXJFormSealX1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axXJFormSealX1.OcxState")));
            this.axXJFormSealX1.Size = new System.Drawing.Size(10, 10);
            this.axXJFormSealX1.TabIndex = 2;
            this.axXJFormSealX1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label1.Font = new System.Drawing.Font("宋体", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(344, 22);
            this.label1.TabIndex = 3;
            this.label1.Text = "正在进行数字签名 请稍等。。。";
            // 
            // XJCAFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(370, 45);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.axXJFormSealX1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XJCAFrm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "CA签章";
            this.Load += new System.EventHandler(this.XJCAFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.axXJFormSealX1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxXJFormSeal.AxXJFormSealX axXJFormSealX1;
        private System.Windows.Forms.Label label1;
    }
}

