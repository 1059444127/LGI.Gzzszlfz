using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class AHSZLYY_Frm : Form
    {
        DataSet ds = new DataSet();
        string[] string1 = new string[4] { "", "", "", "" };
        public AHSZLYY_Frm(DataSet ds1)
        {
           
            InitializeComponent();
            ds = ds1;
        }

        private void AHSZLYY_Frm_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ds.Tables[0];

             
                dataGridView1.Columns[0].Width = 70;
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[0].HeaderText = "π“∫≈–Ú∫≈";

                dataGridView1.Columns[1].Visible = false;

                dataGridView1.Columns[2].Width = 80;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[2].HeaderText = "ø®∫≈";

                dataGridView1.Columns[3].Width = 80;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[3].HeaderText = "∑¢∆±∫≈";

                dataGridView1.Columns[4].Width = 60;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[4].HeaderText = "–’√˚";

                dataGridView1.Columns[5].Width = 30;
                dataGridView1.Columns[5].ReadOnly = true;
                dataGridView1.Columns[5].HeaderText = "–‘±";

                dataGridView1.Columns[6].Width = 50;
                dataGridView1.Columns[6].ReadOnly = true;
                dataGridView1.Columns[6].HeaderText = "ƒÍ¡‰";


                dataGridView1.Columns[7].ReadOnly = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;

                dataGridView1.Columns[13].Width = 100;
                dataGridView1.Columns[13].ReadOnly = true;
                dataGridView1.Columns[13].HeaderText = "¡Ÿ¥≤’Ô∂œ";

                dataGridView1.Columns[14].Width = 80;
                dataGridView1.Columns[14].ReadOnly = true;
                dataGridView1.Columns[14].HeaderText = "ÀÕºÏø∆ “";

                dataGridView1.Columns[15].Width = 60;
                dataGridView1.Columns[15].ReadOnly = true;
                dataGridView1.Columns[15].HeaderText = "ÀÕºÏ“Ω…˙";

                dataGridView1.Columns[16].Width = 120;
                dataGridView1.Columns[16].ReadOnly = true;
                dataGridView1.Columns[16].HeaderText = "π“∫≈»’∆⁄";
           
        }

        public String[] F_STRING
        {
            get { return string1; }
        }
    }
}