using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class xy2yShowFY : Form
    {
        DataTable dt_yzxx = new DataTable();
        public xy2yShowFY(DataTable  dt)
        {
            dt_yzxx = dt;
            InitializeComponent();

            dataGridView1.DataSource = dt_yzxx;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
          
            dataGridView1.Columns[0].HeaderText = "医嘱ID";
            dataGridView1.Columns[1].HeaderText = "医嘱名称";
            dataGridView1.Columns[2].HeaderText = "数量";
            dataGridView1.Columns[3].HeaderText = "单价";
            dataGridView1.Columns[4].HeaderText = "收费状态";
            dataGridView1.Columns[5].HeaderText = "医嘱日期";
            dataGridView1.Columns[6].HeaderText = "医嘱时间";
            dataGridView1.Columns[7].HeaderText = "医嘱状态";
            dataGridView1.Columns[8].HeaderText = "医嘱科室";
            dataGridView1.Columns[9].HeaderText = "检查部位";
            dataGridView1.Columns[10].HeaderText = "操作员";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                this.Close();

            }
           
        }

        private void xy2yShowFY_Load(object sender, EventArgs e)
        {

        }
    }
}