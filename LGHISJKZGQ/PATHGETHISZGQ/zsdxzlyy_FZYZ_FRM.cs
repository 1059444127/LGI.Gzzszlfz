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
          
            comboBox1.Text = "未执行";
            dateTimePicker1.Text = DateTime.Today.AddDays(-10).ToString("yyyy-MM-dd");
            dateTimePicker2.Text = DateTime.Today.ToString("yyyy-MM-dd");
          
            button1_Click(null, null);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {

                getLogeneXml = LogeneXml(dataGridView1.SelectedRows[0].Cells["医嘱号"].Value.ToString());
               
              this.DialogResult = DialogResult.Yes;
                this.Close();
            }
         
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
             if (e.KeyData == Keys.Enter)
             {
                 e.Handled = true;
                 getLogeneXml = LogeneXml(dataGridView1.SelectedRows[0].Cells["医嘱号"].Value.ToString());
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
            string conStr = f.ReadString("病理医嘱列表", "BLODBC", "Data Source=172.16.95.230;Initial Catalog=pathnet;User Id=pathnet;Password=4s3c2a1p");
            string sqlstr = f.ReadString("病理医嘱列表", "sqlstr", "select top 10000 F_ZYH as 住院号,F_BRLB as 病人类别, F_XM as 姓名, F_XB as 性别, F_NL as 年龄,F_BLH as 病理号,F_LKH as 蜡块号,F_BJW as 医嘱项目,F_BZ as 备注,F_YZZT as 医嘱状态,F_SQYS as 申请医生,F_SQSJ as 申请日期,F_ZXR  as 执行人,F_ZXSJ as 执行时间,F_TJYZH as 医嘱号,F_BRBH as  病人编号,F_LCZD as 临床诊断,F_LXXX as 联系信息,F_SFZH as 身份证号,F_BQ as 病区,F_CH as 床号,F_HY as 婚姻,F_SJDW as 送检单位 FROM  V_TJYZ_TO_FZ ");
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

            if (comboBox1.Text.Trim() != "全部")
            {
                if (comboBox1.Text.Trim() == "未执行")
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
            groupBox1.Text = "查询结果:" + dt.Rows.Count.ToString();

            if (dt.Rows.Count <= 0)
            {
                MessageBox.Show("未查询到相关记录");

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
            dv.RowFilter = "医嘱号='" + tjyzh + "'";

            DataTable dt_sqd = dv.ToTable();
            int count = 0;
            if (dt_sqd.Rows.Count > 0)
            {

                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                xml = xml + "病人编号=" + (char)34 + dt_sqd.Rows[count]["病人编号"].ToString() + (char)34 + " ";
                xml = xml + "就诊ID=" + (char)34 + dt_sqd.Rows[count]["病理号"].ToString().Trim()   + (char)34 + " ";
                xml = xml + "申请序号=" + (char)34 + dt_sqd.Rows[count]["医嘱号"].ToString().Trim() + (char)34 + " ";
                if (dt_sqd.Rows[count]["病人类别"].ToString().Trim() == "门诊")
                {
                    xml = xml + "门诊号=" + (char)34 + dt_sqd.Rows[count]["住院号"].ToString().Trim() + (char)34 + " ";
                    xml = xml + "住院号=" + (char)34 +"" + (char)34 + " ";
                }
                else
                {
                    xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                    xml = xml + "住院号=" + (char)34 + dt_sqd.Rows[count]["住院号"].ToString().Trim() + (char)34 + " ";
                }
                xml = xml + "姓名=" + (char)34 + dt_sqd.Rows[count]["姓名"].ToString().Trim() + (char)34 + " ";
                string xb = dt_sqd.Rows[count]["性别"].ToString().Trim();
                xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";
                xml = xml + "年龄=" + (char)34 + dt_sqd.Rows[count]["年龄"].ToString().Trim() + (char)34 + " ";
                xml = xml + "婚姻=" + (char)34 + dt_sqd.Rows[count]["婚姻"].ToString().Trim() + (char)34 + " ";
                xml = xml + "地址=" + (char)34 + "" + (char)34 + "   ";
                xml = xml + "电话=" + (char)34 + dt_sqd.Rows[count]["联系信息"].ToString().Trim() + (char)34 + " ";
                xml = xml + "病区=" + (char)34 + dt_sqd.Rows[count]["病区"].ToString().Trim() + (char)34 + " ";
                xml = xml + "床号=" + (char)34 + dt_sqd.Rows[count]["床号"].ToString().Trim() + (char)34 + " ";
                xml = xml + "身份证号=" + (char)34 + dt_sqd.Rows[count]["身份证号"].ToString().Trim() + (char)34 + " ";
                xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "送检科室=" + (char)34 + "病理科" + (char)34 + " ";
                xml = xml + "送检医生=" + (char)34 + dt_sqd.Rows[count]["申请医生"].ToString().Trim() + (char)34 + " ";
                xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                //  xml = xml + "医嘱项目=" + (char)34 + dt_sqd.Rows[count]["医嘱项目"].ToString().Trim() + (char)34 + " ";
                xml = xml + "医嘱项目=" + (char)34 + "--yz--" + (char)34 + " ";

                xml = xml + "备用1=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "备用2=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                xml = xml + "原病理号=" + (char)34 + dt_sqd.Rows[count]["病理号"].ToString().Trim() + (char)34 + " ";
                xml = xml + "病人类别=" + (char)34 + dt_sqd.Rows[count]["病人类别"].ToString().Trim() + (char)34 + " ";
                xml = xml + "/>";
                xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
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