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
            dataGridView1.Columns[0].HeaderText = "�������";

            dataGridView1.Columns[1].Width = 120;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[1].HeaderText = "����ʱ��";
            dataGridView1.Columns[2].Width = 50;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[2].HeaderText = "����";
            dataGridView1.Columns[3].Width = 30;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[3].HeaderText = "�Ա�";
            dataGridView1.Columns[4].Width = 40;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[4].HeaderText = "����";
            dataGridView1.Columns[5].Width = 80;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[5].HeaderText = "סԺ��";
            dataGridView1.Columns[6].Width = 60;
            dataGridView1.Columns[6].ReadOnly = true;
            dataGridView1.Columns[6].HeaderText = "�����";
            dataGridView1.Columns[7].Width = 80;
            dataGridView1.Columns[7].ReadOnly = true;
            dataGridView1.Columns[7].HeaderText = "�����Ŀ";



            dataGridView1.Columns[8].Width = 80;
            dataGridView1.Columns[8].ReadOnly = true;
            dataGridView1.Columns[8].HeaderText = "�ͼ����";
            dataGridView1.Columns[9].Width = 40;
            dataGridView1.Columns[9].ReadOnly = true;
            dataGridView1.Columns[9].HeaderText = "����";
            dataGridView1.Columns[10].Width = 100;
            dataGridView1.Columns[10].ReadOnly = true;
            dataGridView1.Columns[10].HeaderText = "�걾����";
            dataGridView1.Columns[11].Width = 150;
            dataGridView1.Columns[11].ReadOnly = true;
            dataGridView1.Columns[11].HeaderText = "�ٴ����";
            dataGridView1.Columns[12].Width = 50;
            dataGridView1.Columns[12].ReadOnly = true;
            dataGridView1.Columns[12].HeaderText = "�������";
        }

        private void cqycyy_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = ds.Tables[0];

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {

                dataGridView1.Rows[i].Cells[0].Value = ds.Tables[0].Rows[i]["�������"];

                dataGridView1.Rows[i].Cells[1].Value = ds.Tables[0].Rows[i]["����ʱ��"];

                dataGridView1.Rows[i].Cells[2].Value = ds.Tables[0].Rows[i]["����"];

                dataGridView1.Rows[i].Cells[3].Value = ds.Tables[0].Rows[i]["�Ա�"];

                dataGridView1.Rows[i].Cells[4].Value = ds.Tables[0].Rows[i]["����"];

                dataGridView1.Rows[i].Cells[5].Value = ds.Tables[0].Rows[i]["סԺ��"];

                dataGridView1.Rows[i].Cells[6].Value = ds.Tables[0].Rows[i]["�����"];
                dataGridView1.Rows[i].Cells[7].Value = ds.Tables[0].Rows[i]["ҽ����Ŀ"];

                dataGridView1.Rows[i].Cells[8].Value = ds.Tables[0].Rows[i]["�ͼ����"];

                dataGridView1.Rows[i].Cells[9].Value = ds.Tables[0].Rows[i]["����"];

                dataGridView1.Rows[i].Cells[10].Value = ds.Tables[0].Rows[i]["�걾����"];


                dataGridView1.Rows[i].Cells[11].Value = ds.Tables[0].Rows[i]["�ٴ����"];
                dataGridView1.Rows[i].Cells[12].Value = ds.Tables[0].Rows[i]["�������"];


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