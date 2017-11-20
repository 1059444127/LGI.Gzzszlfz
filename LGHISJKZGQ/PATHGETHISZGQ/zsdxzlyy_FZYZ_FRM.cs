using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LGHISJKZGQ
{
    public partial class zsdxzlyy_FZYZ_FRM : Form
    {
        DataTable dt = new DataTable();
         public zsdxzlyy_FZYZ_FRM()
        {
            InitializeComponent();
        }
        private void FRM_ZSZLYY_FZYZ_SELECT_Load(object sender, EventArgs e)
        {
            dt.Rows.Clear();
          
            comboBox1.Text = "δִ��";
            dateTimePicker1.Text = DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd");
            dateTimePicker2.Text = DateTime.Today.ToString("yyyy-MM-dd");
          
            button1_Click(null, null);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {

                getLogeneXml = LogeneXml(dataGridView1.SelectedRows[0].Cells["ҽ����"].Value.ToString());
               
              this.DialogResult = DialogResult.Yes;
                this.Close();
            }
         
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyData == Keys.Enter)
             {
                 e.Handled = true;
                 getLogeneXml = LogeneXml(dataGridView1.SelectedRows[0].Cells["ҽ����"].Value.ToString());
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
          private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private void button1_Click(object sender, EventArgs e)
        {
            string conStr = f.ReadString("����ҽ���б�", "BLODBC", "Data Source=172.16.95.230;Initial Catalog=pathnet;User Id=pathnet;Password=4s3c2a1p");
            string sqlstr = f.ReadString("����ҽ���б�", "sqlstr", "select top 10000 F_ZYH as סԺ��,F_BRLB as �������, F_XM as ����, F_XB as �Ա�, F_NL as ����,F_BLH as �����,F_LKH as �����,F_BJW as ҽ����Ŀ,F_BZ as ��ע,F_YZZT as ҽ��״̬,F_SQYS as ����ҽ��,F_SQSJ as ��������,F_ZXR  as ִ����,F_ZXSJ as ִ��ʱ��,F_TJYZH as ҽ����,F_BRBH as  ���˱��,F_LCZD as �ٴ����,F_LXXX as ��ϵ��Ϣ,F_SFZH as ���֤��,F_BQ as ����,F_CH as ����,F_HY as ����,F_SJDW as �ͼ쵥λ FROM  V_TJYZ_TO_FZ ");
            SqlDB_ZGQ sql = new SqlDB_ZGQ();
            sqlstr = sqlstr + " where F_SQSJ>='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'  and F_SQSJ<'" + dateTimePicker2.Value.AddDays(1).ToString("yyyy-MM-dd") + "' ";

            if (txtxm.Text.Trim() != "")
                sqlstr = sqlstr + " and F_XM='" + txtxm.Text.Trim() + "'";
            if (txtblh.Text.Trim() != "")
                sqlstr = sqlstr + " and F_BLH='" + txtblh.Text.Trim() + "'";

            if (txtzyh.Text.Trim() != "")
                sqlstr = sqlstr + " and F_ZYH='" + txtzyh.Text.Trim() + "'";
            if (txtmzh.Text.Trim() != "")
                sqlstr = sqlstr + " and F_MZH='" + txtmzh.Text.Trim() + "'";
            if (txtsqys.Text.Trim() != "")
                sqlstr = sqlstr + " and F_SQYS='" + txtsqys.Text.Trim() + "'";
            if (txtzxr.Text.Trim() != "")
                sqlstr = sqlstr + " and F_ZXR='" + txtzxr.Text.Trim() + "'";

            if (comboBox1.Text.Trim() != "ȫ��")
            {
                if (comboBox1.Text.Trim() == "δִ��")
                    sqlstr = sqlstr + " and F_YZZT=' '";
                else
                    sqlstr = sqlstr + " and F_YZZT='" + comboBox1.Text.Trim() + "'";
            }

            string errMsg = "";
            sqlstr = sqlstr + "  order by F_SQSJ desc";


             dt = sql.Sql_DataAdapter(conStr, sqlstr, ref errMsg);
             if (errMsg != "")
             {
                 MessageBox.Show(errMsg); return;
             }

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            groupBox1.Text = "��ѯ���:" + dt.Rows.Count.ToString();

            if (dt.Rows.Count <= 0)
            {
                MessageBox.Show("δ��ѯ����ؼ�¼");

            }
            else
            {
        
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Width = 100;   
                dataGridView1.Columns[1].Width = 50;
                dataGridView1.Columns[2].Width = 70;
                dataGridView1.Columns[3].Width = 30;
                dataGridView1.Columns[4].Width = 45;
                dataGridView1.Columns[5].Width = 80;
                dataGridView1.Columns[6].Width = 85;
                dataGridView1.Columns[7].Width = 160;
                dataGridView1.Columns[8].Width = 90;
                dataGridView1.Columns[9].Width = 60;
                dataGridView1.Columns[10].Width = 65;
                dataGridView1.Columns[11].Width = 120;
                dataGridView1.Columns[12].Width = 65;
                dataGridView1.Columns[13].Width = 120;
                dataGridView1.Columns[14].Width = 120;
                dataGridView1.Columns[15].Width = 80;
                dataGridView1.Columns[16].Width = 150;
                dataGridView1.Columns[17].Width = 100;
                dataGridView1.Columns[18].Width = 160;
                dataGridView1.Columns[19].Width = 90;
                dataGridView1.Columns[20].Width = 80;
                dataGridView1.Columns[21].Width = 50;
                dataGridView1.Columns[22].Width = 140;

            }
        }
        string getLogeneXml = "";
        public String GetLogeneXml
        {
            get { return getLogeneXml; }
        }
        private string LogeneXml(string  tjyzh)
        {
            DataView dv = dt.DefaultView;
            dv.RowFilter = "ҽ����='" + tjyzh + "'";

            DataTable dt_sqd = dv.ToTable();
            int count = 0;
            if (dt_sqd.Rows.Count > 0)
            {

                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                xml = xml + "���˱��=" + (char)34 + dt_sqd.Rows[count]["���˱��"].ToString() + (char)34 + " ";
                xml = xml + "����ID=" + (char)34 + dt_sqd.Rows[count]["�����"].ToString().Trim()   + (char)34 + " ";
                xml = xml + "�������=" + (char)34 + dt_sqd.Rows[count]["ҽ����"].ToString().Trim() + (char)34 + " ";
                if (dt_sqd.Rows[count]["�������"].ToString().Trim() == "����")
                {
                    xml = xml + "�����=" + (char)34 + dt_sqd.Rows[count]["סԺ��"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "סԺ��=" + (char)34 +"" + (char)34 + " ";
                }
                else
                {
                    xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "סԺ��=" + (char)34 + dt_sqd.Rows[count]["סԺ��"].ToString().Trim() + (char)34 + " ";
                }
                xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                string xb = dt_sqd.Rows[count]["�Ա�"].ToString().Trim();
                xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";
                xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                xml = xml + "��ַ=" + (char)34 + "" + (char)34 + "   ";
                xml = xml + "�绰=" + (char)34 + dt_sqd.Rows[count]["��ϵ��Ϣ"].ToString().Trim() + (char)34 + " ";
                xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                xml = xml + "����=" + (char)34 + dt_sqd.Rows[count]["����"].ToString().Trim() + (char)34 + " ";
                xml = xml + "���֤��=" + (char)34 + dt_sqd.Rows[count]["���֤��"].ToString().Trim() + (char)34 + " ";
                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "�ͼ����=" + (char)34 + "�����" + (char)34 + " ";
                xml = xml + "�ͼ�ҽ��=" + (char)34 + dt_sqd.Rows[count]["����ҽ��"].ToString().Trim() + (char)34 + " ";
                xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                //  xml = xml + "ҽ����Ŀ=" + (char)34 + dt_sqd.Rows[count]["ҽ����Ŀ"].ToString().Trim() + (char)34 + " ";
                xml = xml + "ҽ����Ŀ=" + (char)34 + "--yz--" + (char)34 + " ";

                xml = xml + "����1=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "����2=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "ԭ�����=" + (char)34 + dt_sqd.Rows[count]["�����"].ToString().Trim() + (char)34 + " ";
                xml = xml + "�������=" + (char)34 + dt_sqd.Rows[count]["�������"].ToString().Trim() + (char)34 + " ";
                xml = xml + "/>";
                xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                xml = xml + "</LOGENE>";
                log.WriteMyLog(xml);
               // MessageBox.Show(xml);
                return xml;
            }
            else
            {
                return "0";
            }
        }
         
    }
}