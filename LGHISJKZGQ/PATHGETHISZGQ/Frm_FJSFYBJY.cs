using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class Frm_FJSFYBJY : Form
    {
        DataTable dt = new DataTable();
        public Frm_FJSFYBJY(DataTable  dt1)
        {
            dt = dt1;
            InitializeComponent();
        }
        public Frm_FJSFYBJY(DataTable dt1,string withs)
        {
            dt = dt1; with= withs;
            InitializeComponent();
        }
        string F_XH = "";
        string with = "";
        public String xh
        {
            get { return F_XH; }
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
               
                F_XH = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                this.Close();

            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
              
                    F_XH = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                this.Close();

            }
            
        }

        private void Frm_FJSFYBJY_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = dt;
            if (with.Trim() != "")
            {
                string[] with_s = with.Split('^');
                int x = 0;
                foreach (string w in with_s)
                {
                    dataGridView1.Columns[x].Width = int.Parse(w);
                    x++;
                }
            }
        }
    }
}