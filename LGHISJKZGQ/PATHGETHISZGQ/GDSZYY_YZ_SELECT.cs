using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class GDSZYY_YZ_SELECT : Form
    {
        DataTable dt = new DataTable();
        //返回值
        string  XH ="";
        string sqxh = "";
        string yzxm = "";
        int y = 0;
        public GDSZYY_YZ_SELECT(DataTable dt1)
        {
            dt = dt1;
            InitializeComponent();
        }
        private void FRM_YZ_SELECT_Load(object sender, EventArgs e)
        {
             dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; 
             
            dataGridView1.Rows.Add(dt.Rows.Count);
            for(int x=0;x<dt.Rows.Count;x++)
            {
                dataGridView1.Rows[x].Cells[0].Value=true;
                dataGridView1.Rows[x].Cells[1].Value = dt.Rows[x]["序号"].ToString();
                dataGridView1.Rows[x].Cells[2].Value = dt.Rows[x]["MedicalNo"].ToString();
                dataGridView1.Rows[x].Cells[3].Value = dt.Rows[x]["CheckNumber"].ToString();
                dataGridView1.Rows[x].Cells[4].Value = dt.Rows[x]["PatientName"].ToString();
                dataGridView1.Rows[x].Cells[5].Value = dt.Rows[x]["Sex"].ToString();
                dataGridView1.Rows[x].Cells[6].Value = dt.Rows[x]["Money"].ToString();
                dataGridView1.Rows[x].Cells[7].Value = dt.Rows[x]["CheckItemName"].ToString();
                dataGridView1.Rows[x].Cells[8].Value = dt.Rows[x]["AppDeptName"].ToString();
                dataGridView1.Rows[x].Cells[9].Value = dt.Rows[x]["DoctorName"].ToString();
                dataGridView1.Rows[x].Cells[10].Value = dt.Rows[x]["ExecDeptName"].ToString();
            }
        }
        public String F_XH
        {
            get { return XH; }
        }
        public string F_sqxh
        {
            get { return sqxh; }
        }
        public string F_yzxm
        {
            get { return yzxm; }
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    if ((bool)dataGridView1.SelectedRows[0].Cells[0].Value == false)
                        dataGridView1.SelectedRows[0].Cells[0].Value = true;
                    else
                        dataGridView1.SelectedRows[0].Cells[0].Value = false;
                }
              //XH= dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
              //lastXH = dataGridView1.SelectedRows[0].Cells[dataGridView1.ColumnCount-1].Value.ToString();
            
              //this.DialogResult = DialogResult.Yes;
              //  this.Close();

            }
         
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            sqxh = "";
            yzxm = "";
            XH = "";

             if (e.KeyData == Keys.Enter)
             {
                     e.Handled = true;
                   
            float sum=0;
            for (int x = 0; x < dataGridView1.RowCount; x++)
            {
                if((bool)dataGridView1.Rows[x].Cells[0].Value==true)
                {
                    if(sqxh=="")
                    { 
                        sqxh=dataGridView1.Rows[x].Cells[3].Value.ToString().Trim();
                        yzxm=dataGridView1.Rows[x].Cells[7].Value.ToString().Trim();
                        XH = dataGridView1.Rows[x].Cells[1].Value.ToString().Trim();
                        try
                        {
                        sum=float.Parse(dataGridView1.Rows[x].Cells[6].Value.ToString().Trim());
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        
                        sqxh=sqxh+"||"+dataGridView1.Rows[x].Cells[3].Value.ToString().Trim();
                         yzxm=yzxm+"||"+dataGridView1.Rows[x].Cells[7].Value.ToString().Trim();
                          try
                        {
                        sum=sum+float.Parse(dataGridView1.Rows[x].Cells[6].Value.ToString().Trim());
                        }
                        catch
                        {
                        }
                    }
                }
            }

            if(sqxh=="")
            {
                MessageBox.Show("未选择医嘱项目");
            }
            else
            {
                if(MessageBox.Show("你选择的医嘱项目总费用为："+sum.ToString()+"\r\n"+yzxm+"\r\n是否确定？","提示",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1)==DialogResult.Yes)
                {
                this.DialogResult = DialogResult.Yes;
                this.Close();
                }
                else
                {
                    return;
                }

            }
        
                 
                // e.Handled = true;
                // XH = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                // lastXH = dataGridView1.SelectedRows[0].Cells[dataGridView1.ColumnCount - 1].Value.ToString();
                // this.DialogResult = DialogResult.Yes;
                //this.Close();
                 //if (dataGridView1.SelectedRows.Count > 0)
                 //{
                 //    if ((bool)dataGridView1.SelectedRows[0].Cells[0].Value == false)
                 //        dataGridView1.SelectedRows[0].Cells[0].Value = true;
                 //    else
                 //        dataGridView1.SelectedRows[0].Cells[0].Value = false;
                 //}
            }
           
        
        }

        private void FRM_YZ_SELECT_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.Yes && this.DialogResult != DialogResult.No)
            {
                this.DialogResult = DialogResult.No;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            for (int x = 0; x < dataGridView1.RowCount; x++)
            {
                dataGridView1.Rows[x].Cells[0].Value = checkBox1.Checked;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sqxh = "";
            yzxm = "";
            XH = "";
            float sum=0;
            for (int x = 0; x < dataGridView1.RowCount; x++)
            {
                if((bool)dataGridView1.Rows[x].Cells[0].Value==true)
                {
                    if(sqxh=="")
                    {  sqxh=dataGridView1.Rows[x].Cells[3].Value.ToString().Trim();
                        yzxm=dataGridView1.Rows[x].Cells[7].Value.ToString().Trim();
                        XH = dataGridView1.Rows[x].Cells[1].Value.ToString().Trim();
                        try
                        {
                        sum=float.Parse(dataGridView1.Rows[x].Cells[6].Value.ToString().Trim());
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        
                        sqxh=sqxh+"||"+dataGridView1.Rows[x].Cells[3].Value.ToString().Trim();
                         yzxm=yzxm+"||"+dataGridView1.Rows[x].Cells[7].Value.ToString().Trim();
                          try
                        {
                        sum=sum+float.Parse(dataGridView1.Rows[x].Cells[6].Value.ToString().Trim());
                        }
                        catch
                        {
                        }
                    }
                }
            }

            if(sqxh=="")
            {
                MessageBox.Show("未选择医嘱项目");
            }
            else
            {
                if(MessageBox.Show("你选择的医嘱项目总费用为："+sum.ToString()+"\r\n"+yzxm+"\r\n是否确定？","提示",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Information,MessageBoxDefaultButton.Button1)==DialogResult.Yes)
                {
                 //e.Handled = true;
                 
                this.DialogResult = DialogResult.Yes;
                this.Close();
                }
                else
                {
                    return;
                }

            }
        }
    }
}