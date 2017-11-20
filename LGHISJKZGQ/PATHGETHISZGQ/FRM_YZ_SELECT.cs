using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class FRM_YZ_SELECT : Form
    {
        DataTable dt = new DataTable();
        //返回值
        string  XH ="";
        string lastXH = "";

        //
       // int z = -1;
        int y = 0;

        
        //显示的列名
        string Columns="";
        //显示的标题名
        string ColumnsNames = "";
        //dataGridView1显示样式
        string xsys1 = "";

        public FRM_YZ_SELECT(DataTable dt1, string xsys)
        {
            dt = dt1;
            InitializeComponent();
        }
        public FRM_YZ_SELECT(DataTable dt1, string Column, string xsys)
        {
            dt = dt1;Columns = Column; 
            InitializeComponent();
        }

        public FRM_YZ_SELECT(DataTable dt1, string Column,string  ColumnName,string xsys)
        {
            dt = dt1; Columns = Column; ColumnsNames = ColumnName; xsys1 = xsys;
            InitializeComponent();
        }
        private void FRM_YZ_SELECT_Load(object sender, EventArgs e)
        {
            switch (xsys1)
            {
                case "1": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; break;
                case "2": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader; break;
                case "3": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader; break;
                case "4": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells; break;
                case "5": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader; break;
                case "6": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; break;
                case "7": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; break;
                //case "8": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader; break;
                default: dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; break;
                
            }
                y = dt.Columns.Count;
             
            string[] Columnss = Columns.Split(',');
        
            
            if (Columns != "")
            {   
                    DataTable dt2 = new DataTable();
                    for (int i = 0; i < Columnss.Length; i++)
                    {
                        DataColumn dc1 = new DataColumn(Columnss[i].Trim());
                        dt2.Columns.Add(dc1);
                    }
                 
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                         DataRow  dr1=dt2.NewRow();
                        for (int s = 0; s < Columnss.Length; s++)
                        {
                            dr1[s] = dt.Rows[i][Columnss[s].Trim()].ToString();
                        }
                        dt2.Rows.Add(dr1);
                    }
                
                    dataGridView1.DataSource = dt2;
            }
            else
            {
                dataGridView1.AutoGenerateColumns= true;
                dataGridView1.DataSource = dt;
                int x = dt.Columns.Count;
                for (int i = y - 1; i < x; i++)
                {
                    dataGridView1.Columns[i].Visible = false;
                }
            }
          
            if (ColumnsNames.Trim() != "")
            {
                string[] ColumnsName = ColumnsNames.Split(',');
               
                if (ColumnsName.Length > 0)
                {
                
                    for (int a = 0; a < ColumnsName.Length; a++)
                    {
                        dataGridView1.Columns[a].HeaderText = ColumnsName[a].Trim();
                    }

                }
            }

            dataGridView1.Columns[0].Visible = false;
        }
        public String F_XH
        {
            get { return XH; }
        }
        public String F_LAST_XH
        {
            get { return lastXH; }
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                XH = e.RowIndex.ToString();
              lastXH = dataGridView1.SelectedRows[0].Cells[dataGridView1.ColumnCount-1].Value.ToString();
            
              this.DialogResult = DialogResult.Yes;
                this.Close();
            }
         
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyData == Keys.Enter)
             {
                 e.Handled = true;
                 if (dataGridView1.SelectedRows.Count>0)
                 {
                     XH = dataGridView1.SelectedRows[0].Index.ToString();
                 }
                 else
                 {
                     MessageBox.Show("请选择一条医嘱!");
                    return;
                 }
                 lastXH = dataGridView1.SelectedRows[0].Cells[dataGridView1.ColumnCount - 1].Value.ToString();
                 this.DialogResult = DialogResult.Yes;
                this.Close();

            }
           
        
        }

        private void FRM_YZ_SELECT_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.Yes && this.DialogResult != DialogResult.No)
            {
                this.DialogResult = DialogResult.No;
            }
        }
    }
}