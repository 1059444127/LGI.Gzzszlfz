using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class Frm_hnsrmyy : Form
    {
        DataSet ds;
      
       
        string lx = "";
        string XH= "";
        string lastXH = "";
        public Frm_hnsrmyy(DataSet ds1,string lx1)
        {
            lx=lx1;
            InitializeComponent();
            ds = ds1;
        }

        private void Frm_hnsrmyy_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ds.Tables[0];

            if(lx=="ÃÂºÏ")
            {

            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = true;
            dataGridView1.Columns[6].ReadOnly = true;
            dataGridView1.Columns[7].Visible = true;
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
            dataGridView1.Columns[20].Visible = false;
            dataGridView1.Columns[21].Visible = false;
            dataGridView1.Columns[22].Visible = false;
            dataGridView1.Columns[23].Visible = false;
            dataGridView1.Columns[24].Visible = false;
            dataGridView1.Columns[25].Visible = true;
            dataGridView1.Columns[25].Width = 200;
            }
            else
            {
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

         

            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{

            //    dataGridView1.Rows[i].Cells[0].Value = ds.Tables[0].Rows[i]["checkupcode"];
            //    dataGridView1.Rows[i].Cells[1].Value = ds.Tables[0].Rows[i]["EXAMCODE"];
            //    dataGridView1.Rows[i].Cells[2].Value = ds.Tables[0].Rows[i]["NAME"];
            //    dataGridView1.Rows[i].Cells[3].Value = ds.Tables[0].Rows[i]["GENDER"];

            //    dataGridView1.Rows[i].Cells[4].Value = ds.Tables[0].Rows[i]["age"];
            //    dataGridView1.Rows[i].Cells[5].Value = ds.Tables[0].Rows[i]["ORDERNAME"];
            //    dataGridView1.Rows[i].Cells[6].Value = ds.Tables[0].Rows[i]["APPLYDEPARTMENT"];
            //    dataGridView1.Rows[i].Cells[7].Value = ds.Tables[0].Rows[i]["APPLYDOCTOR"];

            //}
           
           
        }
        //public String GHRQ
        //{
        //    get { return ghrq; }
        //}
        //public String FPH
        //{
        //    get { return fph; }
        //}
        //public String CARDNO
        //{
        //    get { return cardno; }
        //}
        //public String GHXH
        //{
        //    get { return ghxh; }
        //}

        public String F_XH
        {
            get { return XH; }
        }
        public String F_LAST_XH
        {
            get { return lastXH; }
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                XH = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                lastXH = dataGridView1.SelectedRows[0].Cells[dataGridView1.ColumnCount - 1].Value.ToString();
                //if(lx=="ÃÂºÏ")
                //{
                
                //fph = dataGridView1.Rows[e.RowIndex].Cells[21].Value.ToString();
                //cardno = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                //}
                //else
                //{
                //ghrq = dataGridView1.Rows[e.RowIndex].Cells[16].Value.ToString();
                //fph = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                //cardno=dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                //ghxh = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                //}
            }
            this.Close();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                //  if(lx=="ÃÂºÏ")
                //{
                 
                //fph = dataGridView1.Rows[e.RowIndex].Cells[21].Value.ToString();
                //cardno = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                //}
                //else
                //{
                //ghrq = dataGridView1.Rows[e.RowIndex].Cells[16].Value.ToString();
                //fph = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                //cardno = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                //ghxh = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                //  }
                XH = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                lastXH = dataGridView1.SelectedRows[0].Cells[dataGridView1.ColumnCount - 1].Value.ToString();
            }
            this.Close();
        }
    }
}