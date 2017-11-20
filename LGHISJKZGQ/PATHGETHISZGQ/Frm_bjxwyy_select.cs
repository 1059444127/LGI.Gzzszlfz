using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class Frm_bjxwyy_select : Form
    {
        DataSet ds;
        string F_SQXH = "";
        string F_ZYCS="";
        string F_RYRQ = "";
        string F_bbmc = "";
        string F_LCZD = "";
        string F_sjys = "";
        public Frm_bjxwyy_select(DataSet ds1)
        {
            ds = ds1;
            InitializeComponent(); 
        }

        private void Frm_bjxwyy_select_Load(object sender, EventArgs e)
        {
           // dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = ds.Tables[0];
          
            dataGridView1.Columns[0].Width = 70;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[0].HeaderText = "申请序号";
            dataGridView1.Columns[1].Width = 80;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[1].HeaderText = "医嘱项目";
            dataGridView1.Columns[2].Width = 30;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[2].HeaderText = "住院次数";
            dataGridView1.Columns[3].Width = 80;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[3].HeaderText = "姓名";
            dataGridView1.Columns[4].Width = 100;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[4].HeaderText = "入院日期";
            dataGridView1.Columns[5].Width = 80;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[5].HeaderText = "病区";
            dataGridView1.Columns[6].Width = 50;
            dataGridView1.Columns[6].ReadOnly = true;
            dataGridView1.Columns[6].HeaderText = "床号";
            dataGridView1.Columns[7].Width = 80;
            dataGridView1.Columns[7].ReadOnly = true;
            dataGridView1.Columns[7].HeaderText = "送检医生";
            dataGridView1.Columns[8].Width = 80;
            dataGridView1.Columns[8].ReadOnly = true;
            dataGridView1.Columns[8].HeaderText = "标本名称";
            dataGridView1.Columns[9].Width = 100;
            dataGridView1.Columns[9].ReadOnly = true;
            dataGridView1.Columns[9].HeaderText = "临床诊断";
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
            dataGridView1.Columns[20].Visible = false;
            dataGridView1.Columns[21].Visible = false;
            dataGridView1.Columns[22].Visible = false;
            dataGridView1.Columns[23].Visible = false;
            dataGridView1.Columns[24].Visible = false;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                dataGridView1.Rows[i].Cells[0].Value = ds.Tables[0].Rows[i]["F_SQXH"];
                dataGridView1.Rows[i].Cells[1].Value = ds.Tables[0].Rows[i]["F_YZXM"];
               dataGridView1.Rows[i].Cells[2].Value = ds.Tables[0].Rows[i]["F_ZYCS"];
                dataGridView1.Rows[i].Cells[3].Value = ds.Tables[0].Rows[i]["F_XM"];

                dataGridView1.Rows[i].Cells[4].Value = ds.Tables[0].Rows[i]["F_RYRQ"];
                dataGridView1.Rows[i].Cells[5].Value = ds.Tables[0].Rows[i]["F_BQ"];
                dataGridView1.Rows[i].Cells[6].Value = ds.Tables[0].Rows[i]["F_CH"];
                dataGridView1.Rows[i].Cells[7].Value = ds.Tables[0].Rows[i]["F_SJYS"];
                dataGridView1.Rows[i].Cells[8].Value = ds.Tables[0].Rows[i]["F_BBMC"];
                dataGridView1.Rows[i].Cells[9].Value = ds.Tables[0].Rows[i]["F_LCZD"];
            }
        }

        public String SQXH
        {
            get { return F_SQXH; }
        }
        public String ZYCS
        {
            get { return F_ZYCS; }
        }
        public String RYRQ
        {
            get { return F_RYRQ; }
        }
        public String BBMC
        {
            get { return F_bbmc; }
        }
        public String LCZD
        {
            get { return F_LCZD; }
        }
        public String SJYS
        {
            get { return F_sjys; }
        }



        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                F_SQXH = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                F_ZYCS = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                F_RYRQ = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                F_sjys = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                F_bbmc = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
                F_LCZD = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();

            }
            this.Close();
        }

        private void dataGridView1_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {

                F_SQXH = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                F_ZYCS = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                F_RYRQ = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                F_sjys = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                F_bbmc = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
                F_LCZD = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();

            }
            this.Close();
        }

    }
}