using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using LGHISJKZGQ;
namespace LGHISJKZGQ
{
    public partial class Frm_nbyzrmyytj : Form
    {

        private static IniFiles w = new IniFiles("pathgethis.ini");
        DataSet ds = new DataSet();
        
        public Frm_nbyzrmyytj(DataSet ds1)
        {
            ds= ds1;
         
            InitializeComponent();
        
            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[0].ReadOnly = true;
            
            dataGridView1.Columns[1].Width = 100;
            dataGridView1.Columns[1].ReadOnly = true;
     
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[2].ReadOnly = true;
          
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[3].ReadOnly = true;
      
            dataGridView1.Columns[4].Width = 150;
            dataGridView1.Columns[4].ReadOnly = true;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = ds.Tables[0];
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                dataGridView1.Columns[0].HeaderText = "体检编号";
                dataGridView1.Rows[i].Cells[0].Value = ds.Tables[0].Rows[i]["brbh"];
                dataGridView1.Columns[1].HeaderText = "项目编号";
                dataGridView1.Rows[i].Cells[1].Value = ds.Tables[0].Rows[i]["xmbh"];
                dataGridView1.Columns[2].HeaderText = "病人姓名";
                dataGridView1.Rows[i].Cells[2].Value = ds.Tables[0].Rows[i]["brxm"];
                dataGridView1.Columns[3].HeaderText = "病人性别";
                dataGridView1.Rows[i].Cells[3].Value = ds.Tables[0].Rows[i]["brxb"];
                dataGridView1.Columns[4].HeaderText = "项目名称";
                dataGridView1.Rows[i].Cells[4].Value = ds.Tables[0].Rows[i]["xmmc"];

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
         
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string xmbh = dataGridView1.Rows[e.RowIndex].Cells["xmbh"].Value.ToString();

            w.WriteString("tijian", "xmbh", xmbh);
            this.Close();
        }

    }
}