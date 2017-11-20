using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dbbase;

namespace LGHISJKZGQ
{
    public partial class stzxyy_select : Form
    {
        DataSet ds = new DataSet();
        public String _xx="";
        public String _xxx="";
        public String _xxxx="";
        string Sslbx = "";
        public String A
        {
            get { return _xx; }
        }
        public String B
        {
            get { return _xxx; }
        }
        public String C
        {
            get { return _xxxx; }
        }
        public stzxyy_select(DataSet ds1,string x)
        {
            ds = ds1;
            Sslbx = x;
            InitializeComponent();

        }

        private void stzxyy_select_Load(object sender, EventArgs e)
        {
            if (Sslbx == "")
                return;

            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-7);
            //dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = ds.Tables[0];

            if (Sslbx == "住院号")
            {
                button1.Enabled = false;

                dataGridView1.Columns[0].Width = 80;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[0].HeaderText = "住院号";
                dataGridView1.Columns[0].DataPropertyName = "zyh";
                dataGridView1.Columns[1].Width = 30;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[1].HeaderText = "jzh";
                dataGridView1.Columns[1].DataPropertyName = "jzh";
                dataGridView1.Columns[2].Width =80 ;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[2].HeaderText = "姓名";
                dataGridView1.Columns[2].DataPropertyName = "xm";
                dataGridView1.Columns[3].Width = 50;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[3].HeaderText = "年龄";
                dataGridView1.Columns[3].DataPropertyName = "nl";
                dataGridView1.Columns[4].Width = 100;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[4].HeaderText = "送检科室";
                dataGridView1.Columns[4].DataPropertyName = "dept_name";
                dataGridView1.Columns[5].Width = 80;
                dataGridView1.Columns[5].ReadOnly = true;
                dataGridView1.Columns[5].HeaderText = "送检医生";
                dataGridView1.Columns[5].DataPropertyName = "ysxm";
                dataGridView1.Columns[6].Width = 100;
                dataGridView1.Columns[6].ReadOnly = true;
                dataGridView1.Columns[6].HeaderText = "收费金额";
                dataGridView1.Columns[6].DataPropertyName = "sfje";
                dataGridView1.Columns[7].Width = 120;
                dataGridView1.Columns[7].ReadOnly = true;
                dataGridView1.Columns[7].HeaderText = "入院时间";
                dataGridView1.Columns[7].DataPropertyName = "ryrq";
                dataGridView1.Columns[8].Width = 80;
                dataGridView1.Columns[8].ReadOnly = true;
                dataGridView1.Columns[8].HeaderText = "床位号";
                dataGridView1.Columns[8].DataPropertyName = "cwdm";
                dataGridView1.Columns[9].Width = 80;
                dataGridView1.Columns[9].ReadOnly = true;
                dataGridView1.Columns[9].HeaderText = "编码";
                dataGridView1.Columns[9].DataPropertyName = "order_sn";
                dataGridView1.Columns[10].ReadOnly = true;
                dataGridView1.Columns[10].HeaderText = "项目名称";
                dataGridView1.Columns[10].DataPropertyName = "order_name";
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
                dataGridView1.Columns[14].Visible = false;
                dataGridView1.Columns[15].Visible = false;
            }
            else
            {
                dataGridView1.Columns[0].Width = 80;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[0].HeaderText = "病人编号";
                dataGridView1.Columns[0].DataPropertyName = "BRBH";
                dataGridView1.Columns[1].Width = 80;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[1].HeaderText = "姓名";
                dataGridView1.Columns[1].DataPropertyName = "DNAME";
                dataGridView1.Columns[2].Width = 30;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[2].HeaderText = "性别";
                dataGridView1.Columns[2].DataPropertyName = "SEX";
                dataGridView1.Columns[3].Width = 50;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[3].HeaderText = "年龄";
                dataGridView1.Columns[3].DataPropertyName = "NL";
                dataGridView1.Columns[4].Width = 80;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[4].HeaderText = "送检科室";
                dataGridView1.Columns[4].DataPropertyName = "KSMC";
                dataGridView1.Columns[5].Width = 50;
                dataGridView1.Columns[5].ReadOnly = true;
                dataGridView1.Columns[5].HeaderText = "送检医生";
                dataGridView1.Columns[5].DataPropertyName = "YSMC";
                dataGridView1.Columns[6].Width = 50;
                dataGridView1.Columns[6].ReadOnly = true;
                dataGridView1.Columns[6].HeaderText = "收费金额";
                dataGridView1.Columns[6].DataPropertyName = "SFJE";
                dataGridView1.Columns[7].Width = 120;
                dataGridView1.Columns[7].ReadOnly = true;
                dataGridView1.Columns[7].HeaderText = "收费时间";
                dataGridView1.Columns[7].DataPropertyName = "GHRQ";
                dataGridView1.Columns[8].Width = 140;
                dataGridView1.Columns[8].ReadOnly = true;
                dataGridView1.Columns[8].HeaderText = "检查项目";
                dataGridView1.Columns[8].DataPropertyName = "JYXM";
                dataGridView1.Columns[9].Width = 60;
                dataGridView1.Columns[9].ReadOnly = true;
                dataGridView1.Columns[9].HeaderText = "检查编码";
                dataGridView1.Columns[9].DataPropertyName = "JYSH";
            }
             //if (Sslbx == "住院号")
             //{
             //    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
             //   for (int x = 0; x < dataGridView1.RowCount; x++)
             //   {
                   
             //       DataTable bljc = new DataTable();
             //       bljc = aa.GetDataTable("select F_blh,F_SQXH,F_BRBH from T_jcxx where F_SQXH='" + dataGridView1.Rows[x].Cells[9].Value.ToString() + "' and F_BRBH='" + dataGridView1.Rows[x].Cells[0].Value.ToString() + "'", "blxx");
             //       if (bljc.Rows.Count > 0)
             //       {
             //           dataGridView1.Rows[x].DefaultCellStyle.BackColor=Color.Red;
             //       }
             //   }
             //}
            
        }



  
      

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (Sslbx == "住院号")
                {
                    
                    _xx = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                    _xxx = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    _xxxx = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
                }
                else
                {
                   
                    _xx = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
                    _xxx = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    _xxxx = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                }
                this.Close();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (Sslbx == "住院号")
                {
                    
                    _xx = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                    _xxx = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    _xxxx = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
                }
                else
                {

                    _xx = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
                    _xxx = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    _xxxx = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                }
                this.Close();
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (Sslbx == "住院号")
                {

                    _xx = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                    _xxx = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    _xxxx = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
                }
                else
                {

                    _xx = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
                    _xxx = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    _xxxx = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                }
                this.Close();
            }

          
        }

        private void button1_Click(object sender, EventArgs e)
        {


            DataTable dtNew = new DataTable();
            DataView view = new DataView();
            view.Table = ds.Tables[0];
            if (textBox1.Text.Trim()=="")
                view.RowFilter = "GHRQ>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and GHRQ<='" + dateTimePicker2.Value.AddDays(1).ToString("yyyy-MM-dd") + "'";
            else
            view.RowFilter = "DNAME like '%" + textBox1.Text.Trim()+ "%' and GHRQ>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and GHRQ<='" + dateTimePicker2.Value.AddDays(1).ToString("yyyy-MM-dd") + "'";
            view.Sort = "GHRQ DESC";                      
            dtNew = view.ToTable();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = dtNew;

            //for (int i = 0; i < dtNew.Rows.Count; i++)
            //    {
            //        dataGridView1.Rows[i].Cells[0].Value = dtNew.Rows[i]["BRBH"];
            //        dataGridView1.Rows[i].Cells[1].Value = dtNew.Rows[i]["DNAME"];
            //        dataGridView1.Rows[i].Cells[2].Value = dtNew.Rows[i]["SEX"];
            //        dataGridView1.Rows[i].Cells[3].Value = dtNew.Rows[i]["NL"];
            //        dataGridView1.Rows[i].Cells[4].Value = dtNew.Rows[i]["KSMC"];
            //        dataGridView1.Rows[i].Cells[5].Value = dtNew.Rows[i]["YSMC"];
            //        dataGridView1.Rows[i].Cells[6].Value = dtNew.Rows[i]["SFJE"];
            //        dataGridView1.Rows[i].Cells[7].Value = dtNew.Rows[i]["GHRQ"];
            //        dataGridView1.Rows[i].Cells[8].Value = ds.Tables[0].Rows[i]["JYXM"];
            //        dataGridView1.Rows[i].Cells[9].Value = ds.Tables[0].Rows[i]["JYSH"];
            //    }
        
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
           
        }

    }
}