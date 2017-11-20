using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class Frm_CDRMYY : Form
    {
        string F_SQXH = "";
        string F_tmh="";
        DataTable T_SQD_YZXX = new DataTable();
        DataTable T_SQD_BBXX = new DataTable();
        DataTable T_SQD = new DataTable();
        bool F_yzbbxx = false;
        public Frm_CDRMYY(string SQXH, DataTable SQD_YZXX, DataTable SQD_BBXX,DataTable  sqdxx,string  tmh,bool yzbbxx)
        {
            InitializeComponent();
            F_SQXH = SQXH;
            T_SQD_BBXX = SQD_BBXX;
            T_SQD_YZXX = SQD_YZXX;
            T_SQD = sqdxx;
            F_tmh = tmh;
            F_yzbbxx = yzbbxx;
        }

        private void Frm_CDRMYY_Load(object sender, EventArgs e)
        {
            if (T_SQD_YZXX.Rows.Count == 0)
            {
                MessageBox.Show("未查询到此申请单的医嘱信息");
            }

     
         
          
            for (int x = 0; x < T_SQD_YZXX.Rows.Count; x++)
            {
                dataGridView1.Rows.Add(1);
                dataGridView1.Rows[x].Cells[0].Value = (x + 1).ToString();
                dataGridView1.Rows[x].Cells[1].Value = T_SQD_YZXX.Rows[x]["F_SQXH"].ToString();
                dataGridView1.Rows[x].Cells[2].Value = T_SQD_YZXX.Rows[x]["F_YZH"].ToString();
                dataGridView1.Rows[x].Cells[3].Value = T_SQD_YZXX.Rows[x]["F_JCXMBM"].ToString();
                dataGridView1.Rows[x].Cells[4].Value = T_SQD_YZXX.Rows[x]["F_JCXMMC"].ToString();
                dataGridView1.Rows[x].Cells[5].Value = T_SQD_YZXX.Rows[x]["F_YZFY"].ToString();
                dataGridView1.Rows[x].Cells[6].Value = T_SQD.Rows[0]["F_sqKS"].ToString();
                dataGridView1.Rows[x].Cells[7].Value = T_SQD.Rows[0]["F_sqYS"].ToString();
                dataGridView1.Rows[x].Cells[8].Value = T_SQD.Rows[0]["F_sqrq"].ToString();
            }
            if (T_SQD_BBXX.Rows.Count == 0)
            {
                MessageBox.Show("未查询到此申请单的标本信息");
            }
         

            for (int x = 0; x < T_SQD_BBXX.Rows.Count; x++)
            {
                dataGridView2.Rows.Add(1);
                dataGridView2.Rows[x].Cells[0].Value = x + 1;
                dataGridView2.Rows[x].Cells[1].Value = T_SQD_BBXX.Rows[x]["F_SQXH"].ToString();
                dataGridView2.Rows[x].Cells[2].Value = T_SQD_BBXX.Rows[x]["F_YZH"].ToString();
                dataGridView2.Rows[x].Cells[3].Value = T_SQD_BBXX.Rows[x]["F_TMH"].ToString();
                dataGridView2.Rows[x].Cells[4].Value = T_SQD_BBXX.Rows[x]["F_BBMC"].ToString();
                dataGridView2.Rows[x].Cells[5].Value = T_SQD_BBXX.Rows[x]["F_CQBW"].ToString();
                dataGridView2.Rows[x].Cells[6].Value = T_SQD_BBXX.Rows[x]["F_SL"].ToString();
                dataGridView2.Rows[x].Cells[7].Value = T_SQD_BBXX.Rows[x]["F_ltsj"].ToString();

                if (F_tmh.Trim() == T_SQD_BBXX.Rows[x]["F_TMH"].ToString().Trim())
                {
                    dataGridView2.Rows[x].Cells[8].Value = true;
                    dataGridView2.Rows[x].DefaultCellStyle.BackColor = Color.Yellow;
                }
                else
                    dataGridView2.Rows[x].Cells[8].Value = false;
            }
            textBox1.SelectAll();
            textBox1.Focus();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("未输入条码号"); textBox1.Focus(); return;
            }


            if (dataGridView2.RowCount <= 0)
            {
                MessageBox.Show("此申请单中未查询到此标本条码信息"); textBox1.Focus(); return;
            }

            int y = 0;
            for (int x = 0; x < T_SQD_BBXX.Rows.Count; x++)
            {
                if (textBox1.Text.Trim() == T_SQD_BBXX.Rows[x]["F_TMH"].ToString().Trim())
                {
                    dataGridView2.Rows[x].Cells[8].Value = true;
                   
                    y = 1;
                }

            }
            if (y == 0)
            {
                MessageBox.Show("此申请单中未查询到此标本袋条码信息！\r\n请标本袋信息是否正确！");

                textBox1.SelectAll(); textBox1.Focus();
            }
            else
                textBox1.Text = "";

            bool bbxxqr = true;
            for (int x = 0; x < dataGridView2.RowCount; x++)
            {



                if ((bool)dataGridView2.Rows[x].Cells[8].Value)
                    dataGridView2.Rows[x].DefaultCellStyle.BackColor = Color.Yellow;
                else
                {
                    dataGridView2.Rows[x].DefaultCellStyle.BackColor = Color.Transparent;
                    bbxxqr = false;
                }

            }



            //if (F_yzbbxx)
            //{
                if (bbxxqr)
                {
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }
            //}
            //else
            //{
            //    if (bbxxqr)
            //    {
            //        this.DialogResult = DialogResult.Yes;
            //        this.Close();
            //    }
            //}

           

           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool   bbxxqr=true;
            if (F_yzbbxx)
            {
                for (int x = 0; x < dataGridView2.RowCount; x++)
                {
                    if (!((bool)dataGridView2.Rows[x].Cells[8].Value))
                        bbxxqr = false;

                }
            }

            if (bbxxqr)
            {
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else
            {
                MessageBox.Show("还有标本条码号未扫描,不能提取信息！"); return;
            }
        }

        private void Frm_CDRMYY_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.No || this.DialogResult == DialogResult.Yes)
            {
            }
            else
            {
                this.DialogResult = DialogResult.No;
            }
        }


    }
}