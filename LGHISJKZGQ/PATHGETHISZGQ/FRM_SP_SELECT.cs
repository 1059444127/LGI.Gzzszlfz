using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class FRM_SP_SELECT : Form
    {
        DataTable dt = new DataTable();
        string[] string1 = new string[4] { "", "", "", "" };

       
        int y = 0;

        string Columns="";
        string ColumnsNames = "";
        string colss = ""; string xsys1 = "";
        public FRM_SP_SELECT(DataTable  dt1,int x)
        {
            dt = dt1; y = x;
            InitializeComponent();
        }
        public FRM_SP_SELECT(DataTable dt1, int x, string Column)
        {
            dt = dt1; y = x; Columns = Column; 
            InitializeComponent();
        }
        public FRM_SP_SELECT(DataTable dt1, int x, string Column,string  cols)
        {
            dt = dt1; y = x; Columns = Column; colss = cols;
            InitializeComponent();
        }
        public FRM_SP_SELECT(DataTable dt1, int x, string Column,string  ColumnName, string cols,string xsys)
        {
            dt = dt1; y = x; Columns = Column; colss = cols; ColumnsNames = ColumnName; xsys1 = xsys;
            InitializeComponent();
        }
        private void FRM_SP_SELECT_Load(object sender, EventArgs e)
        {
            switch (xsys1)
            {
                case "1": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; break;
                case "2": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader; break;
                case "3": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader; break;
                case "4": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells; break;
                case "5": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader; break;
                case "6": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; break;
                case "7": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None; break;
                //case "8": dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader; break;
                default: dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells; break;
                
            }
       
       
            ///////////////////////////////////////////////////////
           // dataGridView1.AllowUserToResizeColumns = true;
            
        
            if (y == -1)
                y = dt.Columns.Count; 
            
            string[] Columnss = Columns.Split(',');
        
            
            if (Columns != "")
            {   
                



                //dataGridView1.Rows[0].Cells[0].Value = "11111";
              //  foreach (string cc in Columnss)
                    DataTable dt2 = new DataTable();
                    for (int i = 0; i < Columnss.Length; i++)
                    {
                        DataColumn dc1 = new DataColumn(Columnss[i].Trim());
                        dt2.Columns.Add(dc1);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                         DataRow  dr1=dt2.NewRow();
                        for (int s = 0; s < Columnss.Length; s++)
                        {
                            dr1[s] = dt.Rows[i][Columnss[s].Trim()].ToString();
                        }
                        dt2.Rows.Add(dr1);
                    }
                 //   dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.DataSource = dt2;

                /////////////////////////////////////////////////////
                    //for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    //{
                    //    if (!Columns.ToLower().Contains(dataGridView1.Columns[i].HeaderText.ToLower()))
                    //        MessageBox.Show(dataGridView1.Columns[i].HeaderText.Trim());
                    //    dataGridView1.Columns.Remove(dataGridView1.Columns[i]);
                    //    //dataGridView1.Columns[i].Visible = false;
                    //}
            }
            else
            {
                dataGridView1.AutoGenerateColumns= true;
                dataGridView1.DataSource = dt;
                int x = dt.Columns.Count;
                for (int i = y - 1; i < x; i++)
                {
                    dataGridView1.Columns[i].Visible = false;
                   // dataGridView1.Columns.Remove(dataGridView1.Columns[i].HeaderText.ToLower());
                }
            }

            ///////////////////////////////////////////////
            if (ColumnsNames.Trim() != "")
            {
                string[] ColumnsName = ColumnsNames.Split(',');
                if (ColumnsName.Length > 0)
                {
                    for (int a = 0; a < ColumnsName.Length; a++)
                    {
                        dataGridView1.Columns[a].HeaderText = ColumnsName[a].Trim();
                    }

                }
            }


        }
        public String [] F_STRING
        {
            get { return string1; }
        }
        //public String F_STRING_2
        //{
        //    get { return string2; }
        //}
        //public String F_STRING_3
        //{
        //    get { return string3; }
        //}
        //public String F_STRING_4
        //{
        //    get { return string4; }
        //}
        //public String F_STRING_5
        //{
        //    get { return string5; }
        //}

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            int y=dataGridView1.ColumnCount;
            if (e.RowIndex != -1)
            {
                if (colss.Trim()=="")
                {
                    string1[0] = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string1[1]= dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string1[2] = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string1[3] = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    string1[4] = dataGridView1.Rows[e.RowIndex].Cells[y-1].Value.ToString();
                }
                else
                {
                   
                    string[] col = colss.Split(',');
                    for (int i = 0; i < col.Length; i++)
                    {
                        
                        if(i<4)
                        {
                           
                          string name = col[i].Trim();
                            string1[i] = dataGridView1.CurrentRow.Cells[name].Value.ToString();
                         
                        }
                    }
                }
            }
            this.Close();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyData == Keys.Enter)
             {
                e.Handled = true;


                    if (colss.Trim() == "")
                    {
                        string1[0] = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                        string1[1] = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                        string1[2] = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                        string1[3] = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                    }
                    else
                    {
                        string[] col = colss.Split(',');
                        for (int i = 0; i < col.Length; i++)
                        {
                            if (i < 4)
                            {
                               string name = col[i].Trim();
                               // string ColumnsName1 = ColumnsNames.Split(',')[i].Trim();
                               string1[i] = dataGridView1.SelectedRows[0].Cells[name].Value.ToString();

                            }
                        }
                    }
                
                this.Close();

            }
           
        
        }

       

    
    }
}