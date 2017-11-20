using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJK
{
    public partial class yzxz_yfy : Form
    {
        public yzxz_yfy(DataTable dt1)
        {
            InitializeComponent();
            dt = dt1;
        }

        public DataTable dt = new DataTable();
        private void yzxz_yfy_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        private string F_xh = "";        
        public string xh
        {
            get { return F_xh; }
            set { this.F_xh = value; }
        }
       

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            F_xh = dataGridView1.CurrentRow.Cells["序号"].Value.ToString();
            //F_yzlb = dataGridView1.CurrentRow.Cells["医嘱类别"].ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
            /*
            if (MessageBox.Show("你将先择医嘱号为：" + dataGridView1.CurrentRow.Cells["医嘱ID"].ToString() + "，请确认！", "PATHGETHIS", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
               
 
            }
            else
            {
                return;
            }
            */
        }

        private void yzxz_yfy_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            F_xh = dataGridView1.CurrentRow.Cells["序号"].Value.ToString();            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}