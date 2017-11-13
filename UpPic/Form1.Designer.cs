namespace UpPic
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.btnOpenPicFolder = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.reportImageBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIsUpload = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFileFullName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colXm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBlh = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTxxh = new DevExpress.XtraGrid.Columns.GridColumn();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportImageBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.simpleButton3);
            this.panelControl1.Controls.Add(this.simpleButton2);
            this.panelControl1.Controls.Add(this.btnOpenPicFolder);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1915, 99);
            this.panelControl1.TabIndex = 0;
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(240, 28);
            this.simpleButton3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(152, 51);
            this.simpleButton3.TabIndex = 1;
            this.simpleButton3.Text = "导入";
            this.simpleButton3.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(50, 28);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(152, 51);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "刷新";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // btnOpenPicFolder
            // 
            this.btnOpenPicFolder.Location = new System.Drawing.Point(431, 28);
            this.btnOpenPicFolder.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnOpenPicFolder.Name = "btnOpenPicFolder";
            this.btnOpenPicFolder.Size = new System.Drawing.Size(235, 51);
            this.btnOpenPicFolder.TabIndex = 0;
            this.btnOpenPicFolder.Text = "打开图片文件夹";
            this.btnOpenPicFolder.Click += new System.EventHandler(this.btnOpenPicFolder_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.splitContainerControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 99);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1915, 954);
            this.panelControl2.TabIndex = 1;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gridControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.pictureEdit1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1915, 954);
            this.splitContainerControl1.SplitterPosition = 736;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.reportImageBindingSource;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(736, 954);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // reportImageBindingSource
            // 
            this.reportImageBindingSource.DataSource = typeof(UpPic.ReportImage);
            this.reportImageBindingSource.CurrentItemChanged += new System.EventHandler(this.reportImageBindingSource_CurrentItemChanged);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIsUpload,
            this.colFileFullName,
            this.colXm,
            this.colBlh,
            this.colTxxh});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colIsUpload
            // 
            this.colIsUpload.FieldName = "IsUpload";
            this.colIsUpload.Name = "colIsUpload";
            this.colIsUpload.Visible = true;
            this.colIsUpload.VisibleIndex = 0;
            // 
            // colFileFullName
            // 
            this.colFileFullName.FieldName = "FileSafeName";
            this.colFileFullName.Name = "colFileFullName";
            this.colFileFullName.OptionsColumn.AllowEdit = false;
            this.colFileFullName.Visible = true;
            this.colFileFullName.VisibleIndex = 1;
            // 
            // colXm
            // 
            this.colXm.FieldName = "Xm";
            this.colXm.Name = "colXm";
            this.colXm.OptionsColumn.AllowEdit = false;
            this.colXm.Visible = true;
            this.colXm.VisibleIndex = 2;
            // 
            // colBlh
            // 
            this.colBlh.FieldName = "Blh";
            this.colBlh.Name = "colBlh";
            this.colBlh.OptionsColumn.AllowEdit = false;
            this.colBlh.Visible = true;
            this.colBlh.VisibleIndex = 3;
            // 
            // colTxxh
            // 
            this.colTxxh.FieldName = "Txxh";
            this.colTxxh.Name = "colTxxh";
            this.colTxxh.OptionsColumn.AllowEdit = false;
            this.colTxxh.Visible = true;
            this.colTxxh.VisibleIndex = 4;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Cursor = System.Windows.Forms.Cursors.Default;
            this.pictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEdit1.Location = new System.Drawing.Point(0, 0);
            this.pictureEdit1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.pictureEdit1.Properties.ZoomAccelerationFactor = 1D;
            this.pictureEdit1.Size = new System.Drawing.Size(1149, 954);
            this.pictureEdit1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 33F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1915, 1053);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "Form1";
            this.Text = "上传报告图片";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportImageBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.BindingSource reportImageBindingSource;
        private DevExpress.XtraGrid.Columns.GridColumn colFileFullName;
        private DevExpress.XtraGrid.Columns.GridColumn colXm;
        private DevExpress.XtraGrid.Columns.GridColumn colBlh;
        private DevExpress.XtraGrid.Columns.GridColumn colTxxh;
        private DevExpress.XtraGrid.Columns.GridColumn colIsUpload;
        private DevExpress.XtraEditors.SimpleButton btnOpenPicFolder;
    }
}

