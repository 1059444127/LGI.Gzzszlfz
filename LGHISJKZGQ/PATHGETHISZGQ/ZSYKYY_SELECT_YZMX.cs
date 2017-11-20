using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace LGHISJKZGQ
{
    public partial class ZSYKYY_SELECT_YZMX: Form
    {
        DataTable dt = new DataTable();
        //返回值
        string  XH ="";
        string lastXH = "";
        string odbc = "";
        string Columns = "";
        string ColumnsNames = "";
        string xsys1 = "";
        public ZSYKYY_SELECT_YZMX(DataTable dt1, string xsys, string odbcstr)
        {
            dt = dt1;
            odbc = odbcstr;
            InitializeComponent();
        }
        public ZSYKYY_SELECT_YZMX(DataTable dt1, string Column, string xsys, string odbcstr)
        {
            dt = dt1; Columns = Column; odbc = odbcstr;
            InitializeComponent();
        }

        public ZSYKYY_SELECT_YZMX(DataTable dt1, string Column, string ColumnName, string xsys,string odbcstr)
        {
            dt = dt1; Columns = Column; ColumnsNames = ColumnName; xsys1 = xsys; odbc = odbcstr;
            InitializeComponent();
        }
        private void FRM_YZ_SELECT_Load(object sender, EventArgs e)
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
         int       y = dt.Columns.Count;
             
            string[] Columnss = Columns.Split(',');
        
            
            if (Columns != "")
            {   
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

                    DataView cv = dt2.DefaultView;
                    cv.Sort = "RequestDateTime desc";

                    dataGridView1.DataSource = cv.ToTable();
                  
            }
            else
            {
                dataGridView1.AutoGenerateColumns= true;
                dataGridView1.DataSource = dt;
                int x = dt.Columns.Count;
                for (int i = y - 1; i < x; i++)
                {
                    dataGridView1.Columns[i].Visible = false;
                }
            }
          
            if (ColumnsNames.Trim() != "")
            {
                string[] ColumnsName = ColumnsNames.Split(',');
               
                if (ColumnsName.Length > 0)
                {
                
                    for (int a = 0; a < ColumnsName.Length; a++)
                    {
                        dataGridView1.Columns[a].HeaderText = ColumnsName[a].Trim();
                        dataGridView1.Columns[a].SortMode=DataGridViewColumnSortMode.Programmatic;
                    }

                }
            }

            if (dataGridView1.Rows.Count>0)
            GetYZMX(dataGridView1.Rows[0].Cells[2].Value.ToString());

        }
        public String F_XH
        {
            get { return XH; }
        }
        public String F_LAST_XH
        {
            get { return lastXH; }
        }
        string yzxm = "";
        public String F_yzxm
        {
            get { return yzxm; }
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
              XH= dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
              lastXH = dataGridView1.SelectedRows[0].Cells[dataGridView1.ColumnCount-1].Value.ToString();

              for (int x = 0; x < dataGridView2.RowCount; x++)
              {
                  if (dataGridView2.Rows[x].Cells[2].Value.ToString().Trim() == "")
                      continue;
                  if (yzxm == "")
                      yzxm = dataGridView2.Rows[x].Cells[1].Value.ToString().Trim()+"^" + dataGridView2.Rows[x].Cells[2].Value.ToString().Trim();
                  else
                      yzxm =yzxm+"|"+dataGridView2.Rows[x].Cells[1].Value.ToString().Trim() + "^" + dataGridView2.Rows[x].Cells[2].Value.ToString().Trim();
              }

                  this.DialogResult = DialogResult.Yes;
                this.Close();
            }
         
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyData == Keys.Enter)
             {
                 e.Handled = true;
              
                 XH = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                 lastXH = dataGridView1.SelectedRows[0].Cells[dataGridView1.ColumnCount - 1].Value.ToString();

                 for (int x = 0; x < dataGridView2.RowCount; x++)
                 {
                     if (dataGridView2.Rows[x].Cells[2].Value.ToString().Trim() == "")
                         continue;
                     if (yzxm == "")
                         yzxm = dataGridView2.Rows[x].Cells[1].Value.ToString().Trim() + "^" + dataGridView2.Rows[x].Cells[2].Value.ToString().Trim();
                     else
                         yzxm = yzxm + "|" + dataGridView2.Rows[x].Cells[1].Value.ToString().Trim() + "^" + dataGridView2.Rows[x].Cells[2].Value.ToString().Trim();
                 }
                 this.DialogResult = DialogResult.Yes;
                this.Close();

            }
           
        
        }

        private void FRM_YZ_SELECT_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.Yes && this.DialogResult != DialogResult.No)
            {
                this.DialogResult = DialogResult.No;
            }
        }


        public void GetYZMX(string sqdh)
        {

            SqlParameter[] sqlPt = new SqlParameter[1];
            for (int j = 0; j < sqlPt.Length; j++)
            {
                sqlPt[j] = new SqlParameter();
            }
            //申请单号
            sqlPt[0].ParameterName = "In_FunctionRequestIDs";
            sqlPt[0].SqlDbType = SqlDbType.NVarChar;
            sqlPt[0].Size=100;
            sqlPt[0].Direction = ParameterDirection.Input;
            sqlPt[0].Value = sqdh;

            DataTable dt_yzmx = new DataTable();
            SqlDB_ZGQ db = new SqlDB_ZGQ();
            string err_msg = "";
            dt_yzmx = db.Sql_DataAdapter(odbc, "pGetFunctionRequestListForPathNet", ref sqlPt, CommandType.StoredProcedure, ref err_msg);
            if (dt_yzmx == null)
            {
                return ;
            }
            dataGridView2.DataSource = dt_yzmx;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.Columns[0].HeaderText = "申请单号";
            dataGridView2.Columns[1].HeaderText = "项目ID";
            dataGridView2.Columns[2].HeaderText = "项目名称";
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GetYZMX(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
        }


        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Enter)
            {
                GetYZMX(dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
            }
        }
       

    
    }
}