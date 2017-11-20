using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace LGHISJKZGQ
{
    public partial class fzs2y_mzlb : Form
    {
        public fzs2y_mzlb()
        {
            InitializeComponent();
        }
        private  IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        DataSet ds1 = new DataSet();
        private void button1_Click(object sender, EventArgs e)
        {
            getmzlb();
        }
        //获取门诊列表
        private  string  getmzlb()
        {
            string debug = f.ReadString("门诊列表", "debug", "");
            int ljfs = f.ReadInteger("门诊列表", "ljfs", 1);
            string weburl = f.ReadString("门诊列表", "wsurl", "");

           
            string fid = "BS10022";
       
            string getcmsg = "";

            string  cxtj="";
            if (txtxm.Text.Trim() != "")
            cxtj = cxtj + "<query item=\"NAME\" compy=\" like \" value=\"'%" + txtxm.Text.Trim() + "%'\" splice=\"and\"/>";
            if (txtkh.Text.Trim() != "")
            cxtj = cxtj + "<query item=\"IC_CARD_ID\" compy=\" = \" value=\"'" + txtkh.Text.Trim() + "'\" splice=\"and\"/>";
            if (cmbks.Text.Trim() != "")
                cxtj = cxtj + "<query item=\"DEPT_NAME\" compy=\" like \" value=\"'%" + cmbks.Text.Trim() + "%'\" splice=\"and\"/>";

            string putcmsg = "<ESBEntry><AccessControl><UserName/><Password/><Fid>BS10022</Fid></AccessControl><MessageHeader><Fid>BS10022</Fid><SourceSysCode>S45</SourceSysCode><TargetSysCode>S01</TargetSysCode><MsgDate>" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "</MsgDate></MessageHeader><MsgInfo><startNum>1</startNum><endNum>20000</endNum><Msg></Msg><query item=\"OPERATION_TIME\" compy=\" &gt;\" value=\"to_date('" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD HH24:MI:SS')\" splice=\"and\"/>"
                + "<query item=\"OPERATION_TIME\" compy=\" &lt;\" value=\"to_date('" + dateTimePicker1.Value.AddDays(1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD HH24:MI:SS')\" splice=\"and\"/>" + cxtj + "</MsgInfo></ESBEntry>";
           
            if (debug == "1")
                log.WriteMyLog("入参：" + putcmsg);

            string err_msg = "";
            bool rtn = false;

            if (ljfs == 0)
            {
                try
                {
                    rtn = MQ(fid, putcmsg, debug, ref  getcmsg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show("获取门诊列表异常："+e1.Message);
                    return "0";
                }
            }
            else
            {
                EHSBMQWeb.Service ehsb = new LGHISJKZGQ.EHSBMQWeb.Service();
                if (weburl.Trim() != "")
                    ehsb.Url = weburl;
                try
                {
                    rtn = ehsb.GETMQ(fid, putcmsg, ref  getcmsg, ref err_msg);
                }
                catch (Exception e1)
                {
                    MessageBox.Show("获取门诊列表异常：" + e1.Message);
                    return "0";
                }
            }


            if (!rtn)
            {
                MessageBox.Show(err_msg);
                return "0";
            }
            if (debug == "1")
                log.WriteMyLog("返回：" + getcmsg);

            if (getcmsg.Trim() == "")
            {
                MessageBox.Show("提取失败，返回为空");
                return "0";
            }
            string RetCon = "";
           
            string RetCode = "";

            string bobys = "";


            XmlNode xmlok = null;
            XmlNodeList xmlNL = null;
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml(getcmsg);
                xmlok = xd.SelectSingleNode("/ESBEntry/RetInfo");

                RetCon = xmlok["RetCon"].InnerText;
                RetCode = xmlok["RetCode"].InnerText;

                if (RetCode != "1")
                {
                    MessageBox.Show(RetCon);
                    return "0";
                }
                xmlNL = xd.SelectNodes("/ESBEntry/MsgInfo/Msg");
                foreach (XmlNode xn in xmlNL)
                {
                    XmlNode xmlok_DATA = null;

                    try
                    {
                        xd.LoadXml(xn.InnerText);
                        xmlok_DATA = xd.SelectSingleNode("/msg/body");
                    }
                    catch
                    {
                    }

                    bobys = bobys + xmlok_DATA.InnerXml;
                }
            }
            catch (Exception e1)
            {
                MessageBox.Show("提取信息异常,解析返回值异常：" + e1.Message);
                return "0";
            }


            //转成dataset
            XmlNode xmlok_DATA2 = null;

        
            try
            {
                xd.LoadXml("<body>" + bobys + "</body>");
                xmlok_DATA2 = xd.SelectSingleNode("/body");
            }
            catch (Exception xmlok_e)
            {
                MessageBox.Show("解析DATA异常：" + xmlok_e.Message);
                return "0";
            }
            if (xmlok_DATA2.InnerXml.Trim() == "")
            {
                MessageBox.Show("未找到对应的记录！");
                return "0";
            }



            try
            {
                StringReader sr = new StringReader(xmlok_DATA2.OuterXml);
                XmlReader xr = new XmlTextReader(sr);
                ds1.Clear();
                ds1.ReadXml(xr);

            }
            catch (Exception eee)
            {
                MessageBox.Show("XML转dataset异常：" + eee.Message);
                return "0";
            }



          

            if (ds1.Tables[0].Rows.Count > 1)
            {
                dataGridView1.Rows.Clear();
                dataGridView1.Rows.Add(ds1.Tables[0].Rows.Count);
                groupBox3.Text = "数据列表   " + ds1.Tables[0].Rows.Count + "条数据";
                for (int x = 0; x < ds1.Tables[0].Rows.Count; x++)
                {
                    dataGridView1.Rows[x].Cells[0].Value = ds1.Tables[0].Rows[x]["OPERATION_TIME"].ToString();
                    dataGridView1.Rows[x].Cells[1].Value = ds1.Tables[0].Rows[x]["IC_CARD_ID"].ToString();
                    dataGridView1.Rows[x].Cells[2].Value = ds1.Tables[0].Rows[x]["NAME"].ToString();
                    dataGridView1.Rows[x].Cells[3].Value = ds1.Tables[0].Rows[x]["SEX"].ToString();
                    dataGridView1.Rows[x].Cells[4].Value = ds1.Tables[0].Rows[x]["AGE"].ToString();
                    dataGridView1.Rows[x].Cells[5].Value = ds1.Tables[0].Rows[x]["DEPT_NAME"].ToString();
                    dataGridView1.Rows[x].Cells[6].Value = ds1.Tables[0].Rows[x]["APPLY_DOCTOR"].ToString();
                    dataGridView1.Rows[x].Cells[7].Value = ds1.Tables[0].Rows[x]["ITEM_NAME"].ToString();
                }
               
            }
            else
            {
               
                    MessageBox.Show("未查询到相关数据");
                    return "0";
  
            }

            return "0";
            //string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            //xml = xml + "<LOGENE>";
            //xml = xml + "<row ";
            //xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[xh]["sick_id"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[xh]["insur_no"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "申请序号=" + (char)34 + ds1.Tables[0].Rows[xh]["apply_no"].ToString().Trim() + (char)34 + " ";

            //if (fid == "BS25016")
            //{
            //    xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
            //    xml = xml + "住院号=" + (char)34 + ds1.Tables[0].Rows[xh]["residence_no"].ToString().Trim() + (char)34 + " ";
            //    xml = xml + "病区=" + (char)34 + ds1.Tables[0].Rows[xh]["dept_name"].ToString().Trim() + (char)34 + " ";
            //    xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[xh]["bed_no"].ToString().Trim() + (char)34 + " ";
            //}
            //else
            //{
            //    string mzh = ds1.Tables[0].Rows[xh]["insur_no"].ToString().Trim();
            //    if (mzh == "")
            //        mzh = Ssbz;
            //    xml = xml + "门诊号=" + (char)34 + mzh + (char)34 + " ";
            //    xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
            //    xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
            //    xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
            //}
            //xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[xh]["name"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "性别=" + (char)34 + ds1.Tables[0].Rows[xh]["sex"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[xh]["age"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "婚姻=" + (char)34 + ds1.Tables[0].Rows[xh]["marital_status"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[xh]["communicate_address"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[xh]["family_phone"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "身份证号=" + (char)34 + ds1.Tables[0].Rows[xh]["id_card_no"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "民族=" + (char)34 + ds1.Tables[0].Rows[xh]["nation"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "职业=" + (char)34 + ds1.Tables[0].Rows[xh]["profession"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "送检科室=" + (char)34 + ds1.Tables[0].Rows[xh]["apply_dept"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "送检医生=" + (char)34 + ds1.Tables[0].Rows[xh]["apply_doctor"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "收费=" + (char)34 + (char)34 + " ";
            //xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
            //xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
            //xml = xml + "医嘱项目=" + (char)34 + ds1.Tables[0].Rows[xh]["item_name"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "备用1=" + (char)34 + (char)34 + " ";
            //xml = xml + "备用2=" + (char)34 + (char)34 + " ";
            //xml = xml + "费别=" + (char)34 + ds1.Tables[0].Rows[xh]["rate_type"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "病人类别=" + (char)34 + ds1.Tables[0].Rows[xh]["table_type"].ToString().Trim() + (char)34 + " ";
            //xml = xml + "/>";
            //xml = xml + "<临床病史><![CDATA[" + ds1.Tables[0].Rows[xh]["clinical_history"].ToString().Trim() + "]]></临床病史>";
            //xml = xml + "<临床诊断><![CDATA[" + ds1.Tables[0].Rows[xh]["now_diagnosis"].ToString().Trim() + "]]></临床诊断>";
            //xml = xml + "</LOGENE>";


            //return xml;

        }


        //连接MQ客户端
        private static bool MQ(string fid, string putcmsg, string debug, ref string getcmsg)
        {
            try
            {
                MQDLL.MQFunction MQManagment = new MQDLL.MQFunction();
                long ret = 0;
                //连接
                ret = MQManagment.connectMQ();

                if (ret != 1)
                {
                    MessageBox.Show("连接MQ服务失败!");
                    return false;
                }
                if (debug == "1")
                    MessageBox.Show("连接MQ服务成功");

                string cmsgid = "";
                getcmsg = "";
                ret = 0;
                ret = MQManagment.putMsg(fid, putcmsg, ref cmsgid);
                ret = MQManagment.getMsgById(fid, cmsgid, 10000, ref getcmsg);



                if (getcmsg.Trim() == "")
                {
                    MessageBox.Show("提取信息失败，返回空");
                    return false;
                }
                //断开
                MQManagment.disconnectMQ();
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return false;
            }

            return true;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

            DataView dv = ds1.Tables[0].DefaultView;
            if (cmbks2.Text.Trim() == "")
            dv.RowFilter = "  NAME like '%" + textBox6.Text.Trim() + "%' ";
            else
            dv.RowFilter = "  NAME like '%" + textBox6.Text.Trim() + "%'  and DEPT_NAME like '%" + cmbks2.Text.Trim() + "%'";   
           
            dataGridView1.Rows.Clear();
           
            DataTable dt = new DataTable();
            dt = dv.ToTable();
           
            groupBox3.Text = "数据列表   " + dt.Rows.Count + "条数据";
            for (int x = 0; x < dt.Rows.Count; x++)
            { dataGridView1.Rows.Add(1);
                dataGridView1.Rows[x].Cells[0].Value = dt.Rows[x]["OPERATION_TIME"].ToString();
                dataGridView1.Rows[x].Cells[1].Value = dt.Rows[x]["IC_CARD_ID"].ToString();
                dataGridView1.Rows[x].Cells[2].Value = dt.Rows[x]["NAME"].ToString();
                dataGridView1.Rows[x].Cells[3].Value = dt.Rows[x]["SEX"].ToString();
                dataGridView1.Rows[x].Cells[4].Value = dt.Rows[x]["AGE"].ToString();
                dataGridView1.Rows[x].Cells[5].Value = dt.Rows[x]["DEPT_NAME"].ToString();
                dataGridView1.Rows[x].Cells[6].Value = dt.Rows[x]["APPLY_DOCTOR"].ToString();
                dataGridView1.Rows[x].Cells[7].Value = dt.Rows[x]["ITEM_NAME"].ToString();
            }
        }

        //private void textBox4_TextChanged(object sender, EventArgs e)
        //{



        //    DataView dv = ds1.Tables[0].DefaultView;
        //    if (textBox6.Text.Trim() == "")
        //    dv.RowFilter = "  DEPT_NAME like '%" + textBox4.Text.Trim() + "%' ";
        //    else
        //    dv.RowFilter = "  NAME like '%" + textBox6.Text.Trim() + "%'  and DEPT_NAME like '%" + textBox4.Text.Trim() + "%'";   
           
        //    dataGridView1.Rows.Clear();

        //    DataTable dt = new DataTable();
        //    dt = dv.ToTable();
         
        //    groupBox3.Text = "数据列表   " + dt.Rows.Count + "条数据";
        //    for (int x = 0; x < dt.Rows.Count; x++)
        //    {   dataGridView1.Rows.Add(1);
        //        dataGridView1.Rows[x].Cells[0].Value = dt.Rows[x]["OPERATION_TIME"].ToString();
        //        dataGridView1.Rows[x].Cells[1].Value = dt.Rows[x]["IC_CARD_ID"].ToString();
        //        dataGridView1.Rows[x].Cells[2].Value = dt.Rows[x]["NAME"].ToString();
        //        dataGridView1.Rows[x].Cells[3].Value = dt.Rows[x]["SEX"].ToString();
        //        dataGridView1.Rows[x].Cells[4].Value = dt.Rows[x]["AGE"].ToString();
        //        dataGridView1.Rows[x].Cells[5].Value = dt.Rows[x]["DEPT_NAME"].ToString();
        //        dataGridView1.Rows[x].Cells[6].Value = dt.Rows[x]["APPLY_DOCTOR"].ToString();
        //        dataGridView1.Rows[x].Cells[7].Value = dt.Rows[x]["ITEM_NAME"].ToString();
        //    }
        //}



        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.Handled = true;
                CARD_ID = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                this.DialogResult = DialogResult.Yes;
              //  lastXH = dataGridView1.SelectedRows[0].Cells[dataGridView1.ColumnCount - 1].Value.ToString();
                this.Close();

            }
        }
        string CARD_ID = "";
        public String IC_CARD_ID
        {
            get { return CARD_ID; }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                CARD_ID = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                //   lastXH = dataGridView1.SelectedRows[0].Cells[dataGridView1.ColumnCount - 1].Value.ToString();
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
        }

        private void fzs2y_mzlb_Load(object sender, EventArgs e)
        {
            string ks = f.ReadString("门诊列表", "ks", ",消化内科门诊,妇产科门诊1,妇产科门诊2,皮肤保健科,心血管内科门诊");

            string[] ksxx = ks.Split(',');
            string[] ksxx2 = ks.Split(',');
            cmbks.DataSource = ksxx;
          cmbks2.DataSource = ksxx2;
            getmzlb();
        }

        private void cmbks2_TextUpdate(object sender, EventArgs e)
        {
            DataView dv = ds1.Tables[0].DefaultView;
            if (cmbks2.Text.Trim() == "")
                dv.RowFilter = "  DEPT_NAME like '%" + cmbks2.Text.Trim() + "%' ";
            else
                dv.RowFilter = "  NAME like '%" + textBox6.Text.Trim() + "%'  and DEPT_NAME like '%" + cmbks2.Text.Trim() + "%'";

            dataGridView1.Rows.Clear();

            DataTable dt = new DataTable();
            dt = dv.ToTable();

            groupBox3.Text = "数据列表   " + dt.Rows.Count + "条数据";
            for (int x = 0; x < dt.Rows.Count; x++)
            {
                dataGridView1.Rows.Add(1);
                dataGridView1.Rows[x].Cells[0].Value = dt.Rows[x]["OPERATION_TIME"].ToString();
                dataGridView1.Rows[x].Cells[1].Value = dt.Rows[x]["IC_CARD_ID"].ToString();
                dataGridView1.Rows[x].Cells[2].Value = dt.Rows[x]["NAME"].ToString();
                dataGridView1.Rows[x].Cells[3].Value = dt.Rows[x]["SEX"].ToString();
                dataGridView1.Rows[x].Cells[4].Value = dt.Rows[x]["AGE"].ToString();
                dataGridView1.Rows[x].Cells[5].Value = dt.Rows[x]["DEPT_NAME"].ToString();
                dataGridView1.Rows[x].Cells[6].Value = dt.Rows[x]["APPLY_DOCTOR"].ToString();
                dataGridView1.Rows[x].Cells[7].Value = dt.Rows[x]["ITEM_NAME"].ToString();
            }
        }

        private void cmbks2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataView dv = ds1.Tables[0].DefaultView;
            if (cmbks2.Text.Trim() == "")
                dv.RowFilter = "  DEPT_NAME like '%" + cmbks2.Text.Trim() + "%' ";
            else
                dv.RowFilter = "  NAME like '%" + textBox6.Text.Trim() + "%'  and DEPT_NAME like '%" + cmbks2.Text.Trim() + "%'";

            dataGridView1.Rows.Clear();

            DataTable dt = new DataTable();
            dt = dv.ToTable();

            groupBox3.Text = "数据列表   " + dt.Rows.Count + "条数据";
            for (int x = 0; x < dt.Rows.Count; x++)
            {
                dataGridView1.Rows.Add(1);
                dataGridView1.Rows[x].Cells[0].Value = dt.Rows[x]["OPERATION_TIME"].ToString();
                dataGridView1.Rows[x].Cells[1].Value = dt.Rows[x]["IC_CARD_ID"].ToString();
                dataGridView1.Rows[x].Cells[2].Value = dt.Rows[x]["NAME"].ToString();
                dataGridView1.Rows[x].Cells[3].Value = dt.Rows[x]["SEX"].ToString();
                dataGridView1.Rows[x].Cells[4].Value = dt.Rows[x]["AGE"].ToString();
                dataGridView1.Rows[x].Cells[5].Value = dt.Rows[x]["DEPT_NAME"].ToString();
                dataGridView1.Rows[x].Cells[6].Value = dt.Rows[x]["APPLY_DOCTOR"].ToString();
                dataGridView1.Rows[x].Cells[7].Value = dt.Rows[x]["ITEM_NAME"].ToString();
            }
        }
    }
}