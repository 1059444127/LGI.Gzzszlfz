using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.Odbc;


namespace LGHISJKZGQ
{
    public partial class ZSDXYKYYSQForm : Form
    {
        public ZSDXYKYYSQForm()
        {
            InitializeComponent();
        }

        private Hashtable ht;

        private DataTable dt;

        public string rexml = "";

        private static IniFiles f = new IniFiles("sz.ini");

        private void GDSYSQForm_Load(object sender, EventArgs e)
        {
            ht = new Hashtable();
            dbbase.odbcdb aa = new dbbase.odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p;", "", "");
            DataTable cyc = aa.GetDataTable("select * from T_CYC where F_CYC_FL='SQKS' order by F_PXID", "T_CYC");
            if (cyc != null && cyc.Rows.Count > 0)
            {
                for (int i = 0; i < cyc.Rows.Count; i++)
                {
                    ht.Add(cyc.Rows[i]["F_CYC_MC"].ToString(), cyc.Rows[i]["F_ZJC1"].ToString());
                    this.comboBox1.Items.Add(cyc.Rows[i]["F_CYC_MC"].ToString());
                }
            }

            this.dateTimePicker1.Value = DateTime.Now.AddDays(-5);
            this.dateTimePicker2.Value = DateTime.Now;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string constring1 = f.ReadString("GDSY", "hisstring", "DSN=pathnet-his;UID=pathnetuser;PWD=userpathnet");
            ODBCHelper bb = new ODBCHelper(constring1);
            OdbcCommand cmd = bb.GetStoredProcCommond("{call pGetFunctionRequestForPathNet(?,?,?,?,?,?,?)}");
            bb.AddInParameter(cmd, "@In_Flag", DbType.Int16, 3);
            bb.AddInParameter(cmd, "@In_StartDate", DbType.DateTime, this.dateTimePicker1.Value);
            bb.AddInParameter(cmd, "@In_EndDate", DbType.DateTime, this.dateTimePicker2.Value);
            bb.AddInParameter(cmd, "@In_RequestDepartmentNos", DbType.String, ht[this.comboBox1.Text] == null ? "" : ht[this.comboBox1.Text] as string);
            bb.AddInParameter(cmd, "@In_IPSeqNoText", DbType.String, "");
            bb.AddInParameter(cmd, "@In_RegisterDate", DbType.DateTime, "");
            bb.AddInParameter(cmd, "@In_SeqNo", DbType.Int32, "");
            OdbcDataAdapter sqlda = new OdbcDataAdapter(cmd);
            DataSet ds = new DataSet();
            sqlda.Fill(ds);
            if (ds.Tables == null || ds.Tables.Count < 1)
                return;
            dt = ds.Tables[0];

            if (dt == null || dt.Rows.Count < 1)
                return;

            DataTable dtbind = new DataTable();
            dtbind.Columns.Add("PatientID");
            dtbind.Columns.Add("PatientName");
            dtbind.Columns.Add("PatientSex");
            dtbind.Columns.Add("PatientAge");
            dtbind.Columns.Add("RequestDepartmentName");
            dtbind.Columns.Add("RequestDateTime");
            dtbind.Columns.Add("FunctionRequestID");

            foreach (DataRow dr in dt.Rows)
            {
                string brbh = dr["PatientID"] == null ? "" : dr["PatientID"].ToString();
                string xm = dr["PatientName"] == null ? "" : dr["PatientName"].ToString();
                string xb = dr["PatientSex"] == null ? "" : dr["PatientSex"].ToString();
                string birthday = dt.Rows[0]["patientbirthday"] == null ? "" : dt.Rows[0]["patientbirthday"].ToString().Trim();
                string nl = "";
                if (birthday != "")
                {
                    try
                    {
                        DateTime birth = DateTime.Parse(birthday);
                        nl = GDSY.CalculateAgeCorrect(birth, DateTime.Now).ToString();
                    }
                    catch
                    {
                        nl = "";
                    }
                }
                string sjks = dr["RequestDepartmentName"] == null ? "" : dr["RequestDepartmentName"].ToString();
                string sqsj = dr["RequestDateTime"] == null ? "" : dr["RequestDateTime"].ToString();
                string sqxh = dr["FunctionRequestID"] == null ? "" : dr["FunctionRequestID"].ToString();
                dtbind.Rows.Add(brbh, xm, xb, nl, sjks, sqsj, sqxh);
            }

            this.dataGridView1.DataSource = dtbind;
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataRow dr = dt.Rows[e.RowIndex];
            if (dr == null) return;

            LogeneXmlHelper lxp = new LogeneXmlHelper();
            lxp.F_ZYH = dr["IPSeqNoText"] == null ? "" : dr["IPSeqNoText"].ToString().Trim();
            lxp.F_BRBH = dr["PatientID"] == null ? "" : dr["PatientID"].ToString().Trim();
            lxp.F_CH = dr["SickBedNo"] == null ? "" : dr["SickBedNo"].ToString().Trim();
            lxp.F_XM = dr["PatientName"] == null ? "" : dr["PatientName"].ToString().Trim();
            string birthday = dr["PatientBirthDay"] == null ? "" : dr["PatientBirthDay"].ToString().Trim();
            if (birthday != "")
            {
                try
                {
                    DateTime birth = DateTime.Parse(birthday);
                    int nl = GDSY.CalculateAgeCorrect(birth, DateTime.Now);
                    lxp.F_NL = nl.ToString();
                }
                catch
                {
                    lxp.F_NL = "";
                }
            }

            lxp.F_XB = dr["PatientSex"] == null ? "" : dr["PatientSex"].ToString().Trim();
            lxp.F_SJKS = dr["RequestDepartmentName"] == null ? "" : dr["RequestDepartmentName"].ToString().Trim();
            lxp.F_JZID = dr["InPatientID"] == null ? "" : dr["InPatientID"].ToString().Trim();
            lxp.F_SQXH = dr["FunctionRequestID"] == null ? "" : dr["FunctionRequestID"].ToString().Trim();

            GDSYForm gdsyform = new GDSYForm();
            gdsyform.In_FunctionRequestIDs = lxp.F_SQXH;
            if (gdsyform.ShowDialog() == DialogResult.OK)
            {
                lxp.F_YZXM = gdsyform.ItemName;
            }
            this.rexml = lxp.ReturnLogeneXML();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}