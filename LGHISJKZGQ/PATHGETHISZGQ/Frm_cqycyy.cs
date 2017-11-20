using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LGHISJKZGQ;

namespace LGHISJKZGQ
{
    public partial class Frm_cqycyy : Form
    {

         private static IniFiles w = new IniFiles("pathgethis.ini");
        DataSet ds = new DataSet();
        public   String _xx;

        public String A
        {set {  _xx= value; }

            get { return _xx; }
        }

        public Frm_cqycyy(DataSet ds1)
        {
           
            ds = ds1;
           
            InitializeComponent();

            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[0].HeaderText = "申请序号";

            dataGridView1.Columns[1].Width = 120;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[1].HeaderText = "申请时间";
            dataGridView1.Columns[2].Width = 50;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[2].HeaderText = "姓名";
            dataGridView1.Columns[3].Width = 30;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[3].HeaderText = "性别";
            dataGridView1.Columns[4].Width = 40;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[4].HeaderText = "年龄";
            dataGridView1.Columns[5].Width = 80;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[5].HeaderText = "住院号";
            dataGridView1.Columns[6].Width = 60;
            dataGridView1.Columns[6].ReadOnly = true;
            dataGridView1.Columns[6].HeaderText = "门诊号";
            dataGridView1.Columns[7].Width = 80;
            dataGridView1.Columns[7].ReadOnly = true;
            dataGridView1.Columns[7].HeaderText = "检查项目";



            dataGridView1.Columns[8].Width = 80;
            dataGridView1.Columns[8].ReadOnly = true;
            dataGridView1.Columns[8].HeaderText = "送检科室";
            dataGridView1.Columns[9].Width = 40;
            dataGridView1.Columns[9].ReadOnly = true;
            dataGridView1.Columns[9].HeaderText = "床号";
            dataGridView1.Columns[10].Width = 100;
            dataGridView1.Columns[10].ReadOnly = true;
            dataGridView1.Columns[10].HeaderText = "标本名称";
            dataGridView1.Columns[11].Width = 150;
            dataGridView1.Columns[11].ReadOnly = true;
            dataGridView1.Columns[11].HeaderText = "临床诊断";
            dataGridView1.Columns[12].Width = 50;
            dataGridView1.Columns[12].ReadOnly = true;
            dataGridView1.Columns[12].HeaderText = "病人类别";
        }

        private void cqycyy_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = ds.Tables[0];

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                dataGridView1.Rows[i].Cells[0].Value = ds.Tables[0].Rows[i]["申请序号"];

                dataGridView1.Rows[i].Cells[1].Value = ds.Tables[0].Rows[i]["申请时间"];

                dataGridView1.Rows[i].Cells[2].Value = ds.Tables[0].Rows[i]["姓名"];

                dataGridView1.Rows[i].Cells[3].Value = ds.Tables[0].Rows[i]["性别"];

                dataGridView1.Rows[i].Cells[4].Value = ds.Tables[0].Rows[i]["年龄"];

                dataGridView1.Rows[i].Cells[5].Value = ds.Tables[0].Rows[i]["住院号"];

                dataGridView1.Rows[i].Cells[6].Value = ds.Tables[0].Rows[i]["门诊号"];
                dataGridView1.Rows[i].Cells[7].Value = ds.Tables[0].Rows[i]["医嘱项目"];

                dataGridView1.Rows[i].Cells[8].Value = ds.Tables[0].Rows[i]["送检科室"];

                dataGridView1.Rows[i].Cells[9].Value = ds.Tables[0].Rows[i]["床号"];

                dataGridView1.Rows[i].Cells[10].Value = ds.Tables[0].Rows[i]["标本名称"];


                dataGridView1.Rows[i].Cells[11].Value = ds.Tables[0].Rows[i]["临床诊断"];
                dataGridView1.Rows[i].Cells[12].Value = ds.Tables[0].Rows[i]["病人类别"];


            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }



        private void dataGridView1_CellDoubleClick1(object sender, DataGridViewCellEventArgs e)
        {
            string sqxh = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

            _xx = sqxh;

            this.Close();

        }


        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string sqxh = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

            _xx = sqxh;

            this.Close();

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string sqxh = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();

            _xx = sqxh;

            this.Close();
        }

        private void cqycyy_FormClosing(object sender, FormClosingEventArgs e)
        {


        }

 

    }
}