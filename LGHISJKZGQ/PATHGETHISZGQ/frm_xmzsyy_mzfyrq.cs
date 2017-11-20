using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class frm_xmzsyy_mzfyrq : Form
    {
        DataTable dtfy = new DataTable();
        public frm_xmzsyy_mzfyrq(DataTable  dt)
        {
            dtfy = dt;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frm_xmzsyy_mzfyrq_Load(object sender, EventArgs e)
        {

            for (int x = 0; x < dtfy.Rows.Count; x++)
            {
                dataGridView1.Rows.Add();
                int y=dataGridView1.RowCount - 1;
                dataGridView1.Rows[y].Cells[0].Value = dtfy.Rows[x]["ADM_NO"].ToString();
                dataGridView1.Rows[y].Cells[1].Value = dtfy.Rows[x]["PatientID"].ToString();
                dataGridView1.Rows[y].Cells[2].Value = dtfy.Rows[x]["CHARGES_CODE"].ToString();
                dataGridView1.Rows[y].Cells[3].Value = dtfy.Rows[x]["DRUG_CODE"].ToString();
                dataGridView1.Rows[y].Cells[4].Value = dtfy.Rows[x]["DRUG_NAME"].ToString();
                dataGridView1.Rows[y].Cells[5].Value = dtfy.Rows[x]["DRUG_UNIT_PRICE"].ToString();
                dataGridView1.Rows[y].Cells[6].Value = dtfy.Rows[x]["DRUG_QTY"].ToString();
                dataGridView1.Rows[y].Cells[7].Value = dtfy.Rows[x]["CHARGES_AMT"].ToString();
                dataGridView1.Rows[y].Cells[8].Value = dtfy.Rows[x]["PRESC_SPEC_NAME"].ToString();
                dataGridView1.Rows[y].Cells[9].Value = dtfy.Rows[x]["PRESC_DOC_NAME"].ToString();
              
                dataGridView1.Rows[y].Cells[10].Value = dtfy.Rows[x]["confirm_flag"].ToString();
                dataGridView1.Rows[y].Cells[11].Value = dtfy.Rows[x]["EXEC_DEPT"].ToString();

                if (dtfy.Rows[x]["confirm_flag"].ToString() != "1")
                    dataGridView1.Rows[y].DefaultCellStyle.BackColor = Color.Yellow;
            }
        
            button1.Focus();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                this.DialogResult = DialogResult.OK;
                this.Close();

            }
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}