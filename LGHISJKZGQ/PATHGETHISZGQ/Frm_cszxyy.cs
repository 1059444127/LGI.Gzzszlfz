using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class Frm_cszxyy : Form
    {
        DataSet ds = new DataSet();
        public String _xx;
        int index = 0;
        public String A
        {
            set { _xx = value; }

            get { return _xx; }
        }

        public Frm_cszxyy(DataSet  ds1,int x)
        {
            ds = ds1;
            index = x;
  
           
            InitializeComponent(); dataGridView1.AutoGenerateColumns = false;
        }

        private void cszxyy_Load(object sender, EventArgs e)
        {
            //dataGridView1.AutoGenerateColumns = false;
            
            if (index == 3)
            {
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.Columns[0].Width = 50;
                dataGridView1.Columns[0].HeaderText = "编号";
                dataGridView1.Columns[1].Width = 170;
                dataGridView1.Columns[1].HeaderText = "申请号";
                dataGridView1.Columns[2].Width = 80;
                dataGridView1.Columns[2].HeaderText = "姓名";
                dataGridView1.Columns[3].Width = 50;
                dataGridView1.Columns[3].HeaderText = "年龄";
                dataGridView1.Columns[4].Width = 100;
                dataGridView1.Columns[4].HeaderText = "门诊号";
                dataGridView1.Columns[5].Width = 150;
                dataGridView1.Columns[5].HeaderText = "医嘱项目";
                dataGridView1.Columns[6].Width = 150;
                dataGridView1.Columns[6].HeaderText = "单位";
                dataGridView1.Columns[7].Width = 100;
                dataGridView1.Columns[7].HeaderText = "电话";
                dataGridView1.Columns[8].Width = 150;
                dataGridView1.Columns[8].HeaderText = "地址";

                this.Text = this.Text + "   体检号：" + ds.Tables[0].Rows[0]["MZBAH"].ToString();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = ds.Tables[0].Rows[i]["userid"];
                    dataGridView1.Rows[i].Cells[1].Value = ds.Tables[0].Rows[i]["ID"];
                    dataGridView1.Rows[i].Cells[2].Value = ds.Tables[0].Rows[i]["XM"];
                    dataGridView1.Rows[i].Cells[3].Value = ds.Tables[0].Rows[i]["NLMC"];

                    dataGridView1.Rows[i].Cells[4].Value = ds.Tables[0].Rows[i]["GHLSH"];
                    dataGridView1.Rows[i].Cells[5].Value = ds.Tables[0].Rows[i]["XMMC"];
                    dataGridView1.Rows[i].Cells[6].Value = ds.Tables[0].Rows[i]["DWMC"];
                    dataGridView1.Rows[i].Cells[7].Value = ds.Tables[0].Rows[i]["LXDH"];
                    dataGridView1.Rows[i].Cells[8].Value = ds.Tables[0].Rows[i]["ZZ"];
                }



              
            }
            if (index == 1)
            {
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.Columns[0].Width = 30;
                dataGridView1.Columns[0].HeaderText = "序号";
                dataGridView1.Columns[1].Width = 100;
                dataGridView1.Columns[1].HeaderText = "申请单号";
                dataGridView1.Columns[2].Width = 80;
                dataGridView1.Columns[2].HeaderText = "姓名";
                dataGridView1.Columns[3].Width = 100;
                dataGridView1.Columns[3].HeaderText = "送检科室";
                dataGridView1.Columns[4].Width = 80;
                dataGridView1.Columns[4].HeaderText = "送检医生";
                dataGridView1.Columns[5].Width = 50;
                dataGridView1.Columns[5].HeaderText = "床号";
                dataGridView1.Columns[6].Width = 200;
                dataGridView1.Columns[6].HeaderText = "标本名称";
                dataGridView1.Columns[7].Width = 250;
                dataGridView1.Columns[7].HeaderText = "临床诊断";
                
                this.Text = this.Text + "   住院号/门诊号：" + ds.Tables[0].Rows[0]["ZYH"].ToString();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value =(i+1);
                    dataGridView1.Rows[i].Cells[1].Value = ds.Tables[0].Rows[i]["ID"];
                    dataGridView1.Rows[i].Cells[2].Value = ds.Tables[0].Rows[i]["XM"];
                    dataGridView1.Rows[i].Cells[3].Value = ds.Tables[0].Rows[i]["SQKSMC"];
                    dataGridView1.Rows[i].Cells[4].Value = ds.Tables[0].Rows[i]["YSDM"];
                    dataGridView1.Rows[i].Cells[5].Value = ds.Tables[0].Rows[i]["CWH"];
                    string bbmc = "";
                    if (ds.Tables[0].Rows[i]["bw1"].ToString() != "")
                        bbmc = bbmc + ds.Tables[0].Rows[0]["bw1"].ToString();
                    if (ds.Tables[0].Rows[i]["bb1"].ToString() != "")
                        bbmc = bbmc + "(" + ds.Tables[0].Rows[i]["bb1"].ToString() + ")";

                    if (ds.Tables[0].Rows[i]["bw2"].ToString() != "")
                        bbmc = bbmc + "," + ds.Tables[0].Rows[i]["bw2"].ToString();
                    if (ds.Tables[0].Rows[i]["bb2"].ToString() != "")
                        bbmc = bbmc + "(" + ds.Tables[0].Rows[i]["bb2"].ToString() + ")";

                    if (ds.Tables[0].Rows[i]["bw3"].ToString() != "")
                        bbmc = bbmc + "," + ds.Tables[0].Rows[i]["bw3"].ToString();
                    if (ds.Tables[0].Rows[i]["bb3"].ToString() != "")
                        bbmc = bbmc + "(" + ds.Tables[0].Rows[i]["bb3"].ToString() + ")";

                    if (ds.Tables[0].Rows[i]["bw4"].ToString() != "")
                        bbmc = bbmc + "," + ds.Tables[0].Rows[i]["bw4"].ToString();
                    if (ds.Tables[0].Rows[i]["bb4"].ToString() != "")
                        bbmc = bbmc + "(" + ds.Tables[0].Rows[i]["bb4"].ToString() + ")";

                    dataGridView1.Rows[i].Cells[6].Value = bbmc.Trim();
                    dataGridView1.Rows[i].Cells[7].Value = ds.Tables[0].Rows[i]["LCZD"];
                }
            }

            }




        private void dataGridView1_CellDoubleClick1(object sender, DataGridViewCellEventArgs e)
        {
            string sqxh = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            _xx = sqxh;

            this.Close();

        }


        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string sqxh = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            _xx = sqxh;

            this.Close();

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string sqxh = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            _xx = sqxh;
              this.Close();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string sqxh = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();

            _xx = sqxh;
  this.Close();
        
        }
    }
}