using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection; // 使用 Assembly 类需用此 命名空间
using System.Reflection.Emit; // 使用 ILGenerator 需用此 命名空间
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.XPath;
using System.Data;
using System.IO;
using M1Card.Common;
using System.ComponentModel;
using LGHISJKZGQ;
namespace LGHISJK
{
    class njxkyy
    {

        [DllImport("HisInterface.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int Init(string AIniDir);
        [DllImport("HisInterface.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern void UnInit();
        [DllImport("HisInterface.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern int SendEmr(string AMsgCode, StringBuilder AsSendXml, IntPtr AsRetXml);

           //int i = -1;
           // i = Init(dir);
           // if (i == 0)
           // {
           //     MessageBox.Show("初始化成功！");
           // }
           // else
           // {
           //     MessageBox.Show("初始化失败！函数返回值为：" + i.ToString());
           // }

           // UnInit();


           // string MsgCode = "";
           // MsgCode = textBox4.Text.Trim();
           // if (String.IsNullOrEmpty(MsgCode))
           // {
           //     MessageBox.Show("请输入消息码！");
           // }
           // int i = -100;
           // string sendXml;
           // sendXml = textBox1.Text.Trim();

           // IntPtr rcvXml = Marshal.AllocHGlobal(800000);
           // i = SendEmr(MsgCode, sendXml, rcvXml);
           // MessageBox.Show(i.ToString());
           // string returnXML = Marshal.PtrToStringAnsi(rcvXml);
           // Marshal.FreeHGlobal(rcvXml);


            
           // //rcvXml = new StringBuilder();
           
           // if (i!=0)
           // {
           //     MessageBox.Show("调用SendEmr出错，函数返回值为："+i.ToString());
           // }
           // richTextBox1.Text = returnXML.ToString();








        private static DLLWrapper func = new DLLWrapper();
        private static IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");

        public static LoadDllapi loaddll = new LoadDllapi();



        public delegate int dllconn(StringBuilder inifile);
        public delegate int commit(string s1, StringBuilder s2, StringBuilder s3);
        public delegate void disdllconn();

        public static string njxkyyXML(string Sslbx, string Ssbz, string Debug)
        {
            string brlb = "";
            string codetype = "";
            string zxks = "";
            string sjks = "";
            string yzxm = "";
            string sqdh = "";
            string sjys = "";
            int sqd = 0;
            if (Sslbx == "住院号")
            {
                brlb = "1";
                codetype = "1";
                sqd = f.ReadInteger("住院号", "sqd", 0);
                zxks = f.ReadString("住院号", "zxks", "222").Replace("\0", "");
            }
            if (Sslbx == "发票号")
            {
                brlb = "0";
                codetype = "9";
                sqd = f.ReadInteger("发票号", "sqd", 0);
                zxks = f.ReadString("发票号", "zxks", "21AA").Replace("\0", "");
            }
            if (Sslbx == "卡号")
            {
                try
                {
                    Ssbz = Read(100);
                }
                catch
                {
                    MessageBox.Show("请讲卡放在读卡器上");
                    return "0";
                }
                //  MessageBox.Show(Ssbz.ToString());
                brlb = "0";
                codetype = "2";
                sqd = f.ReadInteger("卡号", "sqd", 0);
                zxks = f.ReadString("卡号", "zxks", "21AA").Replace("\0", "");
            }
            if (Sslbx == "门诊号")
            {
                brlb = "0";
                codetype = "1";
                sqd = f.ReadInteger("门诊号", "sqd", 0);
                zxks = f.ReadString("门诊号", "zxks", "21AA").Replace("\0", "");
            }
            if (Sslbx == "体检号")
            {

                return "0";
            }


            if (brlb == "")
            {
                MessageBox.Show("无此" + Sslbx);
                return "0";
            }

            string readxml = jb01zgq(brlb, codetype, Ssbz, Debug);
            MessageBox.Show("222");
            MessageBox.Show("HIS传出的字符串"+readxml);
            log.WriteMyLog(readxml);
            //string readxml = jb012(brlb, codetype, Ssbz, Debug);
            DataSet ds1 = new DataSet();
            try
            {
                StringReader xmlstr = null;
                XmlTextReader xmread = null;
                xmlstr = new StringReader(readxml);
                xmread = new XmlTextReader(xmlstr);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmread);

                XmlNode node = xmldoc.SelectSingleNode("//ROWDATA");
                string A = node.OuterXml;
                A = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>" + A;
                xmlstr = new StringReader(A);
                xmread = new XmlTextReader(xmlstr);

                ds1.ReadXml(xmread);
            }
            catch
            {
                if (Debug == "1")
                    log.WriteMyLog("xml转表失败！");
                return "0";

            }
            int xh = 0;
            try
            {
                sjks = ds1.Tables[0].Rows[xh]["DeptName"].ToString().Trim();
            }
            catch
            {
                sjks = "";
            }
            //申请单查询
            if (sqd == 1)
            {
                string his_blh = ds1.Tables[0].Rows[0]["hospno"].ToString().Trim();
                string his_brlb = brlb;
                string his_brxm = ds1.Tables[0].Rows[0]["Patname"].ToString().Trim();
                string his_patid = ds1.Tables[0].Rows[0]["Patientid"].ToString().Trim();

                string his_syxh = "";
                //2014-10-29后加发票号
             
                try
                {
                    his_syxh = ds1.Tables[0].Rows[0]["Syxh"].ToString().Trim();
                }
                catch
                {
                    his_syxh = "0";
                }


              //  his_fph = "0";


                string Tjrybh = "";
                try
                {
                    Tjrybh = ds1.Tables[0].Rows[0]["Tjrybh"].ToString().Trim();
                }
                catch
                {
                    Tjrybh = "0";
                }
               
                //string zxks = f.ReadString("msyy", "zxks", "21AA");
                readxml = jb032(his_brlb, his_blh, his_patid, his_syxh, "0", "", "", "", zxks, "0");
                int xh2 = 0;
                DataSet jb03_ds = new DataSet();
                try
                {
                    StringReader xmlstr = null;
                    XmlTextReader xmread = null;
                    xmlstr = new StringReader(readxml);
                    xmread = new XmlTextReader(xmlstr);
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(xmread);

                    XmlNode node = xmldoc.SelectSingleNode("//ROWDATA");
                    string A = node.OuterXml;
                    A = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>" + A;
                    xmlstr = new StringReader(A);
                    xmread = new XmlTextReader(xmlstr);

                    jb03_ds.ReadXml(xmread);

                    DataTable dtx = new DataTable();
                    dtx.Columns.Add("序号", Type.GetType("System.String"));
                    dtx.Columns.Add("病历号", Type.GetType("System.String"));
                    dtx.Columns.Add("病人ID", Type.GetType("System.String"));
                    dtx.Columns.Add("病人姓名", Type.GetType("System.String"));
                    dtx.Columns.Add("检查项目", Type.GetType("System.String"));
                    dtx.Columns.Add("单价", Type.GetType("System.String"));
                    dtx.Columns.Add("数量", Type.GetType("System.String"));
                    dtx.Columns.Add("单位", Type.GetType("System.String"));
                    dtx.Columns.Add("申请科室", Type.GetType("System.String"));
                    dtx.Columns.Add("申请医生", Type.GetType("System.String"));
                    dtx.Columns.Add("请求日期", Type.GetType("System.String"));


                    string[] dtxrow = new string[dtx.Columns.Count];
                    for (int i = 0; i < jb03_ds.Tables[0].Rows.Count; i++)
                    {
                        dtxrow[0] = i.ToString();
                        dtxrow[1] = jb03_ds.Tables[0].Rows[i]["blh"].ToString();
                        //dtxrow[2] = ds1.Tables[0].Rows[i]["yzlb"].ToString();
                        dtxrow[2] = jb03_ds.Tables[0].Rows[i]["patid"].ToString();
                        dtxrow[3] = his_brxm;
                        dtxrow[4] = jb03_ds.Tables[0].Rows[i]["Itemname"].ToString();
                        dtxrow[5] = jb03_ds.Tables[0].Rows[i]["Price"].ToString();
                        dtxrow[6] = jb03_ds.Tables[0].Rows[i]["Itemqty"].ToString();
                        dtxrow[7] = jb03_ds.Tables[0].Rows[i]["itemunit"].ToString();
                        dtxrow[8] = jb03_ds.Tables[0].Rows[i]["Qqksmc"].ToString();
                        dtxrow[9] = jb03_ds.Tables[0].Rows[i]["Ysmc"].ToString();
                        dtxrow[10] = jb03_ds.Tables[0].Rows[i]["qqrq"].ToString();

                        dtx.Rows.Add(dtxrow);
                    }
                    if (dtx.Rows.Count > 0)
                    {
                        yzxz_yfy from2 = new yzxz_yfy(dtx);

                        string xhb = "";

                        if (from2.ShowDialog() == DialogResult.OK)
                        {
                            xhb = from2.xh;
                            xh2 = Convert.ToInt16(xhb);
                            sqdh = jb03_ds.Tables[0].Rows[xh2]["qqxh"].ToString().Trim();
                            yzxm = jb03_ds.Tables[0].Rows[xh2]["itemname"].ToString().Trim();
                            sjks = jb03_ds.Tables[0].Rows[xh2]["qqksmc"].ToString().Trim();
                            sjys = jb03_ds.Tables[0].Rows[xh2]["ysmc"].ToString().Trim();
                        }
                        else
                        {
                            if (Debug == "1")
                                log.WriteMyLog("未选择医嘱！");
                            return "0";
                        }
                    }
                    //dataGridView1.DataSource = dtx;
                }
                catch (Exception ex)
                {
                    log.WriteMyLog(ex.Message);
                    // MessageBox.Show("未查找到检查记录！");

                }
            }
            //申请单查询结束

            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";

            if (brlb == "0")
            {
                xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[xh]["patientid"].ToString().Trim() + (char)34 + " ";
                xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[xh]["cardno"].ToString().Trim() + (char)34 + " ";
                xml = xml + "门诊号=" + (char)34 + ds1.Tables[0].Rows[xh]["HospNo"].ToString().Trim() + (char)34 + " ";
                xml = xml + "住院号=" + (char)34 + (char)34 + " ";
            }
            else if (brlb == "1")
            {

                xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[xh]["CureNO"].ToString().Trim() + (char)34 + " ";
                xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[xh]["PatientID"].ToString().Trim() + (char)34 + " ";
                xml = xml + "门诊号=" + (char)34 + (char)34 + " ";
                xml = xml + "住院号=" + (char)34 + ds1.Tables[0].Rows[xh]["HospNo"].ToString().Trim() + (char)34 + " ";
            }
            else
            {
                xml = xml + "病人编号=" + (char)34 + ds1.Tables[0].Rows[xh]["patientid"].ToString().Trim() + (char)34 + " ";
                xml = xml + "就诊ID=" + (char)34 + ds1.Tables[0].Rows[xh]["cardno"].ToString().Trim() + (char)34 + " ";
                xml = xml + "门诊号=" + (char)34 + ds1.Tables[0].Rows[xh]["HospNo"].ToString().Trim() + (char)34 + " ";
                xml = xml + "住院号=" + (char)34 + (char)34 + " ";

            }
            xml = xml + "申请序号=" + (char)34 + sqdh + (char)34 + " ";

            xml = xml + "姓名=" + (char)34 + ds1.Tables[0].Rows[xh]["PatName"].ToString().Trim() + (char)34 + " ";
            string xb = "";
            if (ds1.Tables[0].Rows[xh]["Sex"].ToString().Trim() == "1") xb = "男";
            if (ds1.Tables[0].Rows[xh]["Sex"].ToString().Trim() == "2") xb = "女";
            if (ds1.Tables[0].Rows[xh]["Sex"].ToString().Trim() == "3") xb = "其他";

            xml = xml + "性别=" + (char)34 + xb + (char)34 + " ";
            //string nl = datediff(DateTime.Now, Convert.ToDateTime(ds1.Tables[0].Rows[xh]["birthday"].ToString().Trim()));

            try
            {
                xml = xml + "年龄=" + (char)34 + ds1.Tables[0].Rows[xh]["Age"].ToString().Trim() + ds1.Tables[0].Rows[xh]["AgeUnit"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
            }
            xml = xml + "婚姻=" + (char)34 + (char)34 + " ";
            try
            {
                xml = xml + "地址=" + (char)34 + ds1.Tables[0].Rows[xh]["Address"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "地址=" + (char)34 + " " + (char)34 + " ";
            }
            try
            {
                xml = xml + "电话=" + (char)34 + ds1.Tables[0].Rows[xh]["Phone"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "电话=" + (char)34 + (char)34 + " ";
            }
            try
            {
                xml = xml + "病区=" + (char)34 + ds1.Tables[0].Rows[xh]["bqmc"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "病区=" + (char)34 + (char)34 + " ";
            }
            try
            {
                xml = xml + "床号=" + (char)34 + ds1.Tables[0].Rows[xh]["BedNo"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "床号=" + (char)34 + (char)34 + " ";
            }
            try
            {
                xml = xml + "身份证号=" + (char)34 + ds1.Tables[0].Rows[xh]["IDNum"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "身份证号=" + (char)34 + (char)34 + " ";
            }
            try
            {
                xml = xml + "民族=" + (char)34 + ds1.Tables[0].Rows[xh]["Nation"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "民族=" + (char)34 + (char)34 + " ";
            }
            xml = xml + "职业=" + (char)34 + (char)34 + " ";
            try
            {
                xml = xml + "送检科室=" + (char)34 + ds1.Tables[0].Rows[xh]["DeptName"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "送检科室=" + (char)34 + (char)34 + " ";
            }
            xml = xml + "送检医生=" + (char)34 + sjys + (char)34 + " ";
            //xml = xml + "临床诊断=" + (char)34 + (char)34 + " ";
            //xml = xml + "临床病史=" + (char)34 + (char)34 + " ";
            xml = xml + "收费=" + (char)34 + (char)34 + " ";
            xml = xml + "标本名称=" + (char)34 + (char)34 + " ";
            xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
            xml = xml + "医嘱项目=" + (char)34 + yzxm + (char)34 + " ";
            xml = xml + "备用1=" + (char)34 + (char)34 + " ";
            xml = xml + "备用2=" + (char)34 + (char)34 + " ";
            string fkfs = "";

            //if (ds1.Tables[0].Rows[xh]["ChargeType"].ToString().Trim() == "7") fkfs = "干保";

            xml = xml + "费别=" + (char)34 + fkfs + (char)34 + " ";
            if (brlb == "1")
            {
                xml = xml + "病人类别=" + (char)34 + "住院" + (char)34 + " ";
            }
            else
            {
                xml = xml + "病人类别=" + (char)34 + "门诊" + (char)34 + " ";
            }
            xml = xml + "/>";
            xml = xml + "<临床病史><![CDATA[" + " " + "]]></临床病史>";
            xml = xml + "<临床诊断><![CDATA[" + " " + "]]></临床诊断>";
            xml = xml + "</LOGENE>";
            if (Debug == "1")
                log.WriteMyLog("返回的xml字符串:" + xml);
            return xml;




        }
        public static string jb012(string brlb, string codetype, string code, string Debug)
        {
            loaddll.initPath("Ris_His_Interface.dll");
            MessageBox.Show("1");
  dllconn init = (dllconn)loaddll.InvokeMethod("Init", typeof(dllconn));
  commit sendemr = (commit)loaddll.InvokeMethod("SendEmr", typeof(commit));
  disdllconn uninit = (disdllconn)loaddll.InvokeMethod("UnInit", typeof(disdllconn));       
            StringBuilder dllconn33 = new StringBuilder("");
            string retstring = "";
            int yy = 0;
            MessageBox.Show("3");
            try
            {

                yy = init(dllconn33);
            }
            catch
            {
              
            }
            if (yy == 0)
            {
                StringBuilder dllconn = new StringBuilder("");
                StringBuilder S1 = new StringBuilder("JB01");
                string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "GB2312" + (char)34 + " standalong=" + (char)34 + "yes" + (char)34 + "?>";
                inxml = inxml + "<DATAPACKET Version=" + (char)34 + "2.0" + (char)34 + ">";
                inxml = inxml + "<METADATA>";
                inxml = inxml + "<FIELDS>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "brlb" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "codetype" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "code" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "20" + (char)34 + "/>";
                inxml = inxml + "</FIELDS>";
                inxml = inxml + "<PARAMS CHANGE_LOG=" + (char)34 + "1 0 4" + (char)34 + "/>";
                inxml = inxml + "</METADATA>";
                inxml = inxml + "<ROWDATA>";
                inxml = inxml + "<ROW RowState=" + (char)34 + "4" + (char)34 + " ";
                inxml = inxml + "brlb=" + (char)34 + brlb + (char)34 + " ";
                inxml = inxml + "codetype=" + (char)34 + codetype + (char)34 + " ";
                inxml = inxml + "code=" + (char)34 + code + (char)34 + "/>";
                inxml = inxml + "</ROWDATA>";
                inxml = inxml + "</DATAPACKET>";
                StringBuilder S2 = new StringBuilder(inxml);
                StringBuilder S3 = new StringBuilder(65536);

                    sendemr("JB01", S2, S3);
                    retstring = S3.ToString();
            }
            else
            {
                MessageBox.Show("连接His数据库失败！");
                if (Debug == "1")
                    log.WriteMyLog("连接His数据库失败！");
            }

                uninit();
            loaddll.freeLoadDll();
       
            return retstring;
          

            }

        public static string jb01zgq(string brlb, string codetype, string code, string Debug)
        {
         StringBuilder dllconn33 = new StringBuilder("");
            int i = -1;
            i = Init("");
            if (i == 0)
            {
                MessageBox.Show("初始化成功！");
            }
            else
            {
                MessageBox.Show("初始化失败！函数返回值为：" + i.ToString());
            }

         
            string retstring = "";

            if (i == 0)
            {
                StringBuilder dllconn = new StringBuilder("");
                StringBuilder S1 = new StringBuilder("JB01");
                string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "GB2312" + (char)34 + " standalong=" + (char)34 + "yes" + (char)34 + "?>";
                inxml = inxml + "<DATAPACKET Version=" + (char)34 + "2.0" + (char)34 + ">";
                inxml = inxml + "<METADATA>";
                inxml = inxml + "<FIELDS>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "brlb" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "codetype" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "code" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "20" + (char)34 + "/>";
                inxml = inxml + "</FIELDS>";
                inxml = inxml + "<PARAMS CHANGE_LOG=" + (char)34 + "1 0 4" + (char)34 + "/>";
                inxml = inxml + "</METADATA>";
                inxml = inxml + "<ROWDATA>";
                inxml = inxml + "<ROW RowState=" + (char)34 + "4" + (char)34 + " ";
                inxml = inxml + "brlb=" + (char)34 + brlb + (char)34 + " ";
                inxml = inxml + "codetype=" + (char)34 + codetype + (char)34 + " ";
                inxml = inxml + "code=" + (char)34 + code + (char)34 + "/>";
                inxml = inxml + "</ROWDATA>";
                inxml = inxml + "</DATAPACKET>";
                StringBuilder S2 = new StringBuilder(inxml);
               // StringBuilder S3 = new StringBuilder(65536);
                int x = -100;
                IntPtr rcvXml = Marshal.AllocHGlobal(800000);
                x = SendEmr("JB01", S2, rcvXml);
                MessageBox.Show(i.ToString());
                retstring = Marshal.PtrToStringAnsi(rcvXml);
                Marshal.FreeHGlobal(rcvXml);
                //rcvXml = new StringBuilder();
                if (x!= 0)
                {
                    MessageBox.Show("调用SendEmr出错，函数返回值为：" + i.ToString());
                }

            }
            else
            {
                MessageBox.Show("连接His数据库失败！");
                if (Debug == "1")
                    log.WriteMyLog("连接His数据库失败！");
            }
            UnInit();
           // uninit();
            //loaddll.freeLoadDll();

            return retstring;


        }


        public static string jb01(string brlb, string codetype, string code, string Debug)
        {

            try
            {
                func.LoadDll("Ris_His_Interface.dll");
              
                func.LoadFun("Init");
            }
            catch(Exception  dd)
            {
                MessageBox.Show("加载动态库出错！"+dd.ToString());
            }

            //disdllconn uninit = (disdllconn)loaddll.InvokeMethod("UnInit", typeof(disdllconn));

            StringBuilder dllconn = new StringBuilder("");
            StringBuilder S1 = new StringBuilder("JB01");
            string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "GB2312" + (char)34 + " standalong=" + (char)34 + "yes" + (char)34 + "?>";
            inxml = inxml + "<DATAPACKET Version=" + (char)34 + "2.0" + (char)34 + ">";
            inxml = inxml + "<METADATA>";
            inxml = inxml + "<FIELDS>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "brlb" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "codetype" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
            inxml = inxml + "<FIELD attrname=" + (char)34 + "code" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "20" + (char)34 + "/>";
            inxml = inxml + "</FIELDS>";
            inxml = inxml + "<PARAMS CHANGE_LOG=" + (char)34 + "1 0 4" + (char)34 + "/>";
            inxml = inxml + "</METADATA>";
            inxml = inxml + "<ROWDATA>";
            inxml = inxml + "<ROW RowState=" + (char)34 + "4" + (char)34 + "  ";
            inxml = inxml + "brlb=" + (char)34 + brlb + (char)34 + " ";
            inxml = inxml + "codetype=" + (char)34 + codetype + (char)34 + " ";
            inxml = inxml + "code=" + (char)34 + code + (char)34 + "/>";
            inxml = inxml + "</ROWDATA>";
            inxml = inxml + "</DATAPACKET>";
            //MessageBox.Show("传入HIS的字符串："+inxml);

            StringBuilder S2 = new StringBuilder(inxml);
            StringBuilder S3 = new StringBuilder(65536);

            object[] Parameters = new object[] { dllconn }; // 实参为 0 

            Type[] ParameterTypes = new Type[] { typeof(StringBuilder) }; // 实参类型为 int 

            DLLWrapper.ModePass[] themode = new DLLWrapper.ModePass[] { DLLWrapper.ModePass.ByValue }; // 传送方式为值传 

            Type Type_Return = typeof(int); // 返回类型为 int 
            int xx = 0;
            try
            {
                xx = (int)func.Invoke(Parameters, ParameterTypes, themode, Type_Return);

            }
            catch
            {
                MessageBox.Show("连接HIS数据库失败！");
                if (Debug == "1")
                    log.WriteMyLog("连接HIS数据库失败！");

                if (f.ReadInteger("savetohis", "unload", 1) != 0)
                {
                }
                else
                {

                    func.UnLoadDll();
                }
                return "0";

            }
            if (xx == 0)
            {

                try
                {
                    func.LoadFun("SendEmr");
                    Parameters = new object[] { "JB01", S2, S3 }; // 实参为 3 
                    ParameterTypes = new Type[] { typeof(String), typeof(StringBuilder), typeof(StringBuilder) }; // 实参类型为 pchar 
                    themode = new DLLWrapper.ModePass[] { DLLWrapper.ModePass.ByValue, DLLWrapper.ModePass.ByValue, DLLWrapper.ModePass.ByValue }; // 传送方式为值传 
                    xx = (int)func.Invoke(Parameters, ParameterTypes, themode, Type_Return);
                }
                catch (Exception e1)
                {
                    MessageBox.Show("传入字符串格式不对！" + e1);
                }

            }
            else
            {
                MessageBox.Show("连接HIS数据库失败！");
                if (Debug == "1")
                    log.WriteMyLog("连接HIS数据库失败！");

                if (f.ReadInteger("savetohis", "unload", 1) != 0)
                { }
                else
                {

                    func.UnLoadDll();
                }
                return "0";
            }
            if (xx == 0)
            {
                //func.LoadDll("UnInit");
                //Parameters = new object[] { }; // 实参为 0
                //ParameterTypes = new Type[] { }; // 实参类型为 pchar 
                //themode = new DLLWrapper.ModePass[] { }; // 传送方式为值传 
                //func.Invoke(Parameters, ParameterTypes, themode, Type_Return);

                //MessageBox.Show("1");
                // uninit();
                if (f.ReadInteger("savetohis", "unload", 1) != 0)
                {

                }
                else
                {

                    func.UnLoadDll();
                }
                //MessageBox.Show(S3.ToString());
                return S3.ToString();
            }
            else
            {

                MessageBox.Show("未查询到病人信息！");
                if (Debug == "1")
                    log.WriteMyLog(S3.ToString());
                //  uninit();
                if (f.ReadInteger("savetohis", "unload", 1) != 0)
                { }
                else
                {

                    func.UnLoadDll();
                }
                return "0";

            }
            // 弹出提示框，显示调用 myfun.Invoke 方法的结果，即调用 count 函数                        

        }

        public static string jb032(string brlb, string blh, string patid, string syxh, string qqxh, string tjrybh, string rq1, string rq2, string zxks, string Debug)
        {
            loaddll.initPath("Ris_His_Interface.dll");

            dllconn init = (dllconn)loaddll.InvokeMethod("Init", typeof(dllconn));
            disdllconn uninit = (disdllconn)loaddll.InvokeMethod("UnInit", typeof(disdllconn));
            commit sendemr = (commit)loaddll.InvokeMethod("SendEmr", typeof(commit));
            StringBuilder dllconn33 = new StringBuilder("");
            string retstring = "";
            int yy = 0;
            try
            {
                yy = init(dllconn33);
            }
            catch
            {
            }

            if (yy == 0)
            {
                StringBuilder dllconn = new StringBuilder("");
                StringBuilder S1 = new StringBuilder("JB01");
                string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "GB2312" + (char)34 + " standalong=" + (char)34 + "yes" + (char)34 + "?>";
                inxml = inxml + "<DATAPACKET Version=" + (char)34 + "2.0" + (char)34 + ">";
                inxml = inxml + "<METADATA>";
                inxml = inxml + "<FIELDS>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "blh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "36" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "brlb" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "patid" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "20" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "syxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "30" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "qqxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "50" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "tjrybh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "50" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "rq1" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "rq2" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "zxks" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "6" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "sqdxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "20" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "fph" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "20" + (char)34 + "/>";
                inxml = inxml + "</FIELDS>";
                inxml = inxml + "<PARAMS CHANGE_LOG=" + (char)34 + "1 0 4" + (char)34 + "/>";
                inxml = inxml + "</METADATA>";
                inxml = inxml + "<ROWDATA>";
                inxml = inxml + "<ROW RowState=" + (char)34 + "4" + (char)34 + " ";
                inxml = inxml + "blh=" + (char)34 + blh + (char)34 + " ";
                inxml = inxml + "brlb=" + (char)34 + brlb + (char)34 + " ";
                inxml = inxml + "patid=" + (char)34 + patid + (char)34 + " ";
                inxml = inxml + "syxh=" + (char)34 + syxh + (char)34 + " ";
                inxml = inxml + "qqxh=" + (char)34 + qqxh + (char)34 + " ";
                inxml = inxml + "tjrybh=" + (char)34 + "0" + (char)34 + " ";
                inxml = inxml + "rq1=" + (char)34 + rq1 + (char)34 + " ";
                inxml = inxml + "rq2=" + (char)34 + rq2 + (char)34 + " ";


                inxml = inxml + "zxks=" + (char)34 + zxks + (char)34 + " ";
                inxml = inxml + "sqdxh=" + (char)34 + "0" + (char)34 + " ";
                inxml = inxml + "fph=" + (char)34 + "0" + (char)34 + "/>";
                //inxml = inxml + "sqdxh=" + (char)34 + zxks + (char)34 + " ";
                //inxml = inxml + "zxks=" + (char)34 + "0" + (char)34 + "/>";

                inxml = inxml + "</ROWDATA>";
                inxml = inxml + "</DATAPACKET>";

                StringBuilder S2 = new StringBuilder(inxml);
                StringBuilder S3 = new StringBuilder(65536);

                sendemr("JB03", S2, S3);
                retstring = S3.ToString();
            }
            else
            {
                MessageBox.Show("连接His数据库失败！");
                if (Debug == "1")
                    log.WriteMyLog("连接His数据库失败！");
            }

            uninit();
            if (f.ReadInteger("savetohis", "unload", 1) != 0)
            { }
            else
            {

                loaddll.freeLoadDll();
            }

            return retstring;



        }

        public static string jb03(string brlb, string blh, string patid, string syxh, string qqxh, string tjrybh, string rq1, string rq2, string zxks, string Debug, string fph)
        {

            func.LoadDll("Ris_His_Interface.dll");
            //MessageBox.Show("开始检查申请单");
            dllconn init = (dllconn)loaddll.InvokeMethod("Init", typeof(dllconn));
            disdllconn uninit = (disdllconn)loaddll.InvokeMethod("UnInit", typeof(disdllconn));
            commit sendemr = (commit)loaddll.InvokeMethod("SendEmr", typeof(commit));

            StringBuilder dllconn33 = new StringBuilder("");
            string retstring = "";
            int yy = init(dllconn33);
            //MessageBox.Show("此处正常！");
            if (yy == 0)
            {
                StringBuilder dllconn = new StringBuilder("");
                StringBuilder S1 = new StringBuilder("JB01");
                string inxml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "GB2312" + (char)34 + " standalong=" + (char)34 + "yes" + (char)34 + "?>";
                inxml = inxml + "<DATAPACKET Version=" + (char)34 + "2.0" + (char)34 + ">";
                inxml = inxml + "<METADATA>";
                inxml = inxml + "<FIELDS>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "blh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "36" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "brlb" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "10" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "patid" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "20" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "syxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "30" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "qqxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "50" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "tjrybh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "50" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "rq1" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "rq2" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "16" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "zxks" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "6" + (char)34 + "/>";
                inxml = inxml + "<FIELD attrname=" + (char)34 + "sqdxh" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "20" + (char)34 + "/>";
                // inxml = inxml + "<FIELD attrname=" + (char)34 + "fph" + (char)34 + " fieldtype=" + (char)34 + "string" + (char)34 + " WIDTH=" + (char)34 + "20" + (char)34 + "/>";
                inxml = inxml + "</FIELDS>";
                inxml = inxml + "<PARAMS CHANGE_LOG=" + (char)34 + "1 0 4" + (char)34 + "/>";
                inxml = inxml + "</METADATA>";
                inxml = inxml + "<ROWDATA>";
                inxml = inxml + "<ROW RowState=" + (char)34 + "4" + (char)34 + " ";
                inxml = inxml + "blh=" + (char)34 + blh + (char)34 + " ";
                inxml = inxml + "brlb=" + (char)34 + brlb + (char)34 + " ";
                inxml = inxml + "patid=" + (char)34 + patid + (char)34 + " ";
                inxml = inxml + "syxh=" + (char)34 + syxh + (char)34 + " ";
                inxml = inxml + "qqxh=" + (char)34 + qqxh + (char)34 + " ";
                inxml = inxml + "tjrybh=" + (char)34 + "0" + (char)34 + " ";
                inxml = inxml + "rq1=" + (char)34 + rq1 + (char)34 + " ";
                inxml = inxml + "rq2=" + (char)34 + rq2 + (char)34 + " ";


                inxml = inxml + "zxks=" + (char)34 + zxks + (char)34 + " ";
                inxml = inxml + "sqdxh=" + (char)34 + "0" + (char)34 + "/>";
                //  inxml = inxml + "fph=" + (char)34 + fph + (char)34 + " ";
                //inxml = inxml + "zxks=" + (char)34 + "0" + (char)34 + "/>";

                inxml = inxml + "</ROWDATA>";
                inxml = inxml + "</DATAPACKET>";
                //MessageBox.Show("查找申请单的字符串："+inxml);
                StringBuilder S2 = new StringBuilder(inxml);
                StringBuilder S3 = new StringBuilder(65536);

                try
                {
                    sendemr("JB03", S2, S3);
                }
                catch (Exception aa)
                {
                    MessageBox.Show("出错" + aa.ToString());
                }
                retstring = S3.ToString();
            }
            else
            {
                MessageBox.Show("连接His数据库失败！");
                if (Debug == "1")
                    log.WriteMyLog("连接His数据库失败！");
            }

            uninit();
            if (f.ReadInteger("savetohis", "unload", 1) != 0)
            { }
            else
            {

                loaddll.freeLoadDll();
            }

            return retstring;



        }

        public static string Read(int length)
        {
            #region 读就诊卡变量
            /// <summary>
            /// 设备ID
            /// </summary>
            int _icdev = -1;
            /// <summary>
            /// 当前IC卡号
            /// </summary>
            int _cardId = 0;
            #endregion
            if (length > 720) throw new ArgumentException("要读取的长度太长");

            StringBuilder data = new StringBuilder(length);   //存放读取出的数据

            try
            {
                int result;                     //设备API函数返回的结果

                #region 初始化设备
                //连接设备并设置波特率
                _icdev = URF.rf_init(0, 9600);
                if (_icdev < 0)
                { MessageBox.Show(_icdev.ToString()); }
                //寻卡
                result = URF.rf_card((Int16)_icdev, 1, ref _cardId);
                if (result != 0)
                { MessageBox.Show(result.ToString()); }
                //加载密码
                result = URF.rf_load_key_hex(_icdev, 0, 0, "FFFFFFFFFFFF");
                if (result != 0)
                { MessageBox.Show(result.ToString()); }
                #endregion

                #region 读取
                int readedBlock = 0;                    //标识变量,标识已读取的块数

                int block = length / 16;                //要读取的字符长度所占的块数
                if (length % 16 != 0) block++;          //如果有字符占不满一个块就多加一个

                int sector = block / 3;                 //字符串所占的扇区
                if (block % 3 != 0) sector++;           //如果块占不满一个扇区同样多加一个扇区

                //遍历扇区
                for (int sq = 1; sq <= sector; sq++)
                {
                    //验证该扇区密码
                    result = URF.rf_authentication(_icdev, 0, sq);
                    if (result != 0)
                    {
                        // MessageBox.Show(result.ToString());
                    }
                    //遍历块
                    for (int bk = 0; bk < 3; bk++)
                    {
                        if (readedBlock < block)
                        {
                            StringBuilder tempData = new StringBuilder(64);
                            result = URF.rf_read(_icdev, sq * 4 + bk, tempData);

                            //如果超过16位后面的是乱码(原因未深究.)
                            if (tempData.Length > 16)
                                data.Append(tempData.ToString().Substring(0, 16));
                            else
                                data.Append(tempData.ToString());
                            readedBlock++;
                        }
                    }
                }
                #endregion
            }

            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                //释放设备
                if (_cardId != 0)
                {
                    URF.rf_halt((Int16)_icdev);
                    _cardId = 0;
                }
                if (_icdev > 0)
                {
                    URF.rf_exit(_icdev);
                    _icdev = -1;
                }
            }

            return data.ToString();
        }

    }
}
