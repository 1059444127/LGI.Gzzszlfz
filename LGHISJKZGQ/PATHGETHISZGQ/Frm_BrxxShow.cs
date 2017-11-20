using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class Frm_BrxxShow : Form
    {
        public Frm_BrxxShow()
        {
            InitializeComponent();
        }
        int y = 0;
        DataTable dt = new DataTable();
       
        string[] Column_1;
        string[] ColumnName_1;
        int xsys1 = 1;
        int[] widtrs;
        public Frm_BrxxShow(DataTable dt1)
        {
            dt = dt1;
            InitializeComponent();
        }
        public Frm_BrxxShow(DataTable dt1, string[] Column, string[] ColumnName)
        {
            dt = dt1;
            Column_1 = Column;
            ColumnName_1 = ColumnName;
            InitializeComponent();
        }
        public Frm_BrxxShow(DataTable dt1, string[] Column, string[] ColumnName, int AutoSizeColumnsMode)
        {
            dt = dt1;
            Column_1 = Column;
            ColumnName_1 = ColumnName;
            xsys1 = AutoSizeColumnsMode;
            InitializeComponent();
        }
        public Frm_BrxxShow(DataTable dt1, string[] Column, string[] ColumnName, int[] Width)
        {
            dt = dt1;
            Column_1 = Column;
            ColumnName_1 = ColumnName;
            widtrs = Width;
            InitializeComponent();
        }
        public Frm_BrxxShow(DataTable dt1, string[] Column, string[] ColumnName, int AutoSizeColumnsMode, int[] Width)
        {
            dt = dt1;
            Column_1 = Column;
            ColumnName_1 = ColumnName;
            xsys1 = AutoSizeColumnsMode;
            widtrs = Width;
            InitializeComponent();
        }
        private void Frm_BrxxShow_Load(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();

            //switch (xsys1)
            //{
            //    case "1": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; break;
            //    case "2": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader; break;
            //    case "3": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader; break;
            //    case "4": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells; break;
            //    case "5": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader; break;
            //    case "6": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; break;
            //    case "7": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; break;
            //    default: dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; break;

            //}

            //------------------------------------------------
            int count = dt.Rows.Count;
            if (count <= 0)
            {
                y = -1;
                return;
            }
          //  dataGridView1.Rows.Add(dt.Rows.Count);
            dataGridView1.DataSource=dt;
            for (int x1 = 0; x1 < count; x1++)
            {
              // dataGridView1.Rows[x1].Cells[]
            }





        }
        public int ID
        {
            get { return y; }
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                y =(int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
            }
            this.Close();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyData == Keys.Enter)
             {
                e.Handled = true;
                y = (int)dataGridView1.SelectedRows[0].Cells[0].Value;
                this.Close();

            }
           
        
        }
    
    }
}