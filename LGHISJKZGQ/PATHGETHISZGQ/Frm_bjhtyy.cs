using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class Frm_bjhtyy : Form
    {

        DataSet ds;
        string F_SQXH = "";

        public Frm_bjhtyy(DataSet ds1)
        {
            ds = ds1;
            InitializeComponent(); 
        }

        private void Frm_bjhtyy_Load(object sender, EventArgs e)
        {
           // dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = ds.Tables[0];
          
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[0].HeaderText = "体检号";
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[1].HeaderText = "申请序号";
            dataGridView1.Columns[2].Width = 80;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[2].HeaderText = "姓名";
            dataGridView1.Columns[3].Width = 50;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[3].HeaderText = "性别";
            dataGridView1.Columns[4].Width = 50;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[4].HeaderText = "年龄";
            dataGridView1.Columns[5].Width = 100;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[5].HeaderText = "医嘱项目";
            dataGridView1.Columns[6].Width = 100;
            dataGridView1.Columns[6].ReadOnly = true;
            dataGridView1.Columns[6].HeaderText = "送检科室";
            dataGridView1.Columns[7].Width = 100;
            dataGridView1.Columns[7].ReadOnly = true;
            dataGridView1.Columns[7].HeaderText = "送检医生";
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns[11].Visible = false;
            dataGridView1.Columns[12].Visible = false;
            dataGridView1.Columns[13].Visible = false;
            dataGridView1.Columns[14].Visible = false;
            dataGridView1.Columns[15].Visible = false;
            dataGridView1.Columns[16].Visible = false;
            dataGridView1.Columns[17].Visible = false;
            dataGridView1.Columns[18].Visible = false;
            dataGridView1.Columns[19].Visible = false;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                dataGridView1.Rows[i].Cells[0].Value = ds.Tables[0].Rows[i]["checkupcode"];
                dataGridView1.Rows[i].Cells[1].Value = ds.Tables[0].Rows[i]["EXAMCODE"];
                dataGridView1.Rows[i].Cells[2].Value = ds.Tables[0].Rows[i]["NAME"];
                dataGridView1.Rows[i].Cells[3].Value = ds.Tables[0].Rows[i]["GENDER"];

                dataGridView1.Rows[i].Cells[4].Value = ds.Tables[0].Rows[i]["age"];
                dataGridView1.Rows[i].Cells[5].Value = ds.Tables[0].Rows[i]["ORDERNAME"];
                dataGridView1.Rows[i].Cells[6].Value = ds.Tables[0].Rows[i]["APPLYDEPARTMENT"];
                dataGridView1.Rows[i].Cells[7].Value = ds.Tables[0].Rows[i]["APPLYDOCTOR"];

            }
        }

        public String SQXH
        {
            get { return F_SQXH; }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
             F_SQXH = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            
            this.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
             F_SQXH = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
 
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }


    }
}