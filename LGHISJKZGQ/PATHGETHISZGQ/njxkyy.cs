using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection; // ʹ�� Assembly �����ô� �����ռ�
using System.Reflection.Emit; // ʹ�� ILGenerator ���ô� �����ռ�
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
           //     MessageBox.Show("��ʼ���ɹ���");
           // }
           // else
           // {
           //     MessageBox.Show("��ʼ��ʧ�ܣ���������ֵΪ��" + i.ToString());
           // }

           // UnInit();


           // string MsgCode = "";
           // MsgCode = textBox4.Text.Trim();
           // if (String.IsNullOrEmpty(MsgCode))
           // {
           //     MessageBox.Show("��������Ϣ�룡");
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
           //     MessageBox.Show("����SendEmr������������ֵΪ��"+i.ToString());
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
            if (Sslbx == "סԺ��")
            {
                brlb = "1";
                codetype = "1";
                sqd = f.ReadInteger("סԺ��", "sqd", 0);
                zxks = f.ReadString("סԺ��", "zxks", "222").Replace("\0", "");
            }
            if (Sslbx == "��Ʊ��")
            {
                brlb = "0";
                codetype = "9";
                sqd = f.ReadInteger("��Ʊ��", "sqd", 0);
                zxks = f.ReadString("��Ʊ��", "zxks", "21AA").Replace("\0", "");
            }
            if (Sslbx == "����")
            {
                try
                {
                    Ssbz = Read(100);
                }
                catch
                {
                    MessageBox.Show("�뽲�����ڶ�������");
                    return "0";
                }
                //  MessageBox.Show(Ssbz.ToString());
                brlb = "0";
                codetype = "2";
                sqd = f.ReadInteger("����", "sqd", 0);
                zxks = f.ReadString("����", "zxks", "21AA").Replace("\0", "");
            }
            if (Sslbx == "�����")
            {
                brlb = "0";
                codetype = "1";
                sqd = f.ReadInteger("�����", "sqd", 0);
                zxks = f.ReadString("�����", "zxks", "21AA").Replace("\0", "");
            }
            if (Sslbx == "����")
            {

                return "0";
            }


            if (brlb == "")
            {
                MessageBox.Show("�޴�" + Sslbx);
                return "0";
            }

            string readxml = jb01zgq(brlb, codetype, Ssbz, Debug);
            MessageBox.Show("222");
            MessageBox.Show("HIS�������ַ���"+readxml);
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
                    log.WriteMyLog("xmlת��ʧ�ܣ�");
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
            //���뵥��ѯ
            if (sqd == 1)
            {
                string his_blh = ds1.Tables[0].Rows[0]["hospno"].ToString().Trim();
                string his_brlb = brlb;
                string his_brxm = ds1.Tables[0].Rows[0]["Patname"].ToString().Trim();
                string his_patid = ds1.Tables[0].Rows[0]["Patientid"].ToString().Trim();

                string his_syxh = "";
                //2014-10-29��ӷ�Ʊ��
             
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
                    dtx.Columns.Add("���", Type.GetType("System.String"));
                    dtx.Columns.Add("������", Type.GetType("System.String"));
                    dtx.Columns.Add("����ID", Type.GetType("System.String"));
                    dtx.Columns.Add("��������", Type.GetType("System.String"));
                    dtx.Columns.Add("�����Ŀ", Type.GetType("System.String"));
                    dtx.Columns.Add("����", Type.GetType("System.String"));
                    dtx.Columns.Add("����", Type.GetType("System.String"));
                    dtx.Columns.Add("��λ", Type.GetType("System.String"));
                    dtx.Columns.Add("�������", Type.GetType("System.String"));
                    dtx.Columns.Add("����ҽ��", Type.GetType("System.String"));
                    dtx.Columns.Add("��������", Type.GetType("System.String"));


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
                                log.WriteMyLog("δѡ��ҽ����");
                            return "0";
                        }
                    }
                    //dataGridView1.DataSource = dtx;
                }
                catch (Exception ex)
                {
                    log.WriteMyLog(ex.Message);
                    // MessageBox.Show("δ���ҵ�����¼��");

                }
            }
            //���뵥��ѯ����

            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
            xml = xml + "<LOGENE>";
            xml = xml + "<row ";

            if (brlb == "0")
            {
                xml = xml + "���˱��=" + (char)34 + ds1.Tables[0].Rows[xh]["patientid"].ToString().Trim() + (char)34 + " ";
                xml = xml + "����ID=" + (char)34 + ds1.Tables[0].Rows[xh]["cardno"].ToString().Trim() + (char)34 + " ";
                xml = xml + "�����=" + (char)34 + ds1.Tables[0].Rows[xh]["HospNo"].ToString().Trim() + (char)34 + " ";
                xml = xml + "סԺ��=" + (char)34 + (char)34 + " ";
            }
            else if (brlb == "1")
            {

                xml = xml + "���˱��=" + (char)34 + ds1.Tables[0].Rows[xh]["CureNO"].ToString().Trim() + (char)34 + " ";
                xml = xml + "����ID=" + (char)34 + ds1.Tables[0].Rows[xh]["PatientID"].ToString().Trim() + (char)34 + " ";
                xml = xml + "�����=" + (char)34 + (char)34 + " ";
                xml = xml + "סԺ��=" + (char)34 + ds1.Tables[0].Rows[xh]["HospNo"].ToString().Trim() + (char)34 + " ";
            }
            else
            {
                xml = xml + "���˱��=" + (char)34 + ds1.Tables[0].Rows[xh]["patientid"].ToString().Trim() + (char)34 + " ";
                xml = xml + "����ID=" + (char)34 + ds1.Tables[0].Rows[xh]["cardno"].ToString().Trim() + (char)34 + " ";
                xml = xml + "�����=" + (char)34 + ds1.Tables[0].Rows[xh]["HospNo"].ToString().Trim() + (char)34 + " ";
                xml = xml + "סԺ��=" + (char)34 + (char)34 + " ";

            }
            xml = xml + "�������=" + (char)34 + sqdh + (char)34 + " ";

            xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["PatName"].ToString().Trim() + (char)34 + " ";
            string xb = "";
            if (ds1.Tables[0].Rows[xh]["Sex"].ToString().Trim() == "1") xb = "��";
            if (ds1.Tables[0].Rows[xh]["Sex"].ToString().Trim() == "2") xb = "Ů";
            if (ds1.Tables[0].Rows[xh]["Sex"].ToString().Trim() == "3") xb = "����";

            xml = xml + "�Ա�=" + (char)34 + xb + (char)34 + " ";
            //string nl = datediff(DateTime.Now, Convert.ToDateTime(ds1.Tables[0].Rows[xh]["birthday"].ToString().Trim()));

            try
            {
                xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["Age"].ToString().Trim() + ds1.Tables[0].Rows[xh]["AgeUnit"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
            }
            xml = xml + "����=" + (char)34 + (char)34 + " ";
            try
            {
                xml = xml + "��ַ=" + (char)34 + ds1.Tables[0].Rows[xh]["Address"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "��ַ=" + (char)34 + " " + (char)34 + " ";
            }
            try
            {
                xml = xml + "�绰=" + (char)34 + ds1.Tables[0].Rows[xh]["Phone"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "�绰=" + (char)34 + (char)34 + " ";
            }
            try
            {
                xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["bqmc"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "����=" + (char)34 + (char)34 + " ";
            }
            try
            {
                xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["BedNo"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "����=" + (char)34 + (char)34 + " ";
            }
            try
            {
                xml = xml + "���֤��=" + (char)34 + ds1.Tables[0].Rows[xh]["IDNum"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "���֤��=" + (char)34 + (char)34 + " ";
            }
            try
            {
                xml = xml + "����=" + (char)34 + ds1.Tables[0].Rows[xh]["Nation"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "����=" + (char)34 + (char)34 + " ";
            }
            xml = xml + "ְҵ=" + (char)34 + (char)34 + " ";
            try
            {
                xml = xml + "�ͼ����=" + (char)34 + ds1.Tables[0].Rows[xh]["DeptName"].ToString().Trim() + (char)34 + " ";
            }
            catch
            {
                xml = xml + "�ͼ����=" + (char)34 + (char)34 + " ";
            }
            xml = xml + "�ͼ�ҽ��=" + (char)34 + sjys + (char)34 + " ";
            //xml = xml + "�ٴ����=" + (char)34 + (char)34 + " ";
            //xml = xml + "�ٴ���ʷ=" + (char)34 + (char)34 + " ";
            xml = xml + "�շ�=" + (char)34 + (char)34 + " ";
            xml = xml + "�걾����=" + (char)34 + (char)34 + " ";
            xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
            xml = xml + "ҽ����Ŀ=" + (char)34 + yzxm + (char)34 + " ";
            xml = xml + "����1=" + (char)34 + (char)34 + " ";
            xml = xml + "����2=" + (char)34 + (char)34 + " ";
            string fkfs = "";

            //if (ds1.Tables[0].Rows[xh]["ChargeType"].ToString().Trim() == "7") fkfs = "�ɱ�";

            xml = xml + "�ѱ�=" + (char)34 + fkfs + (char)34 + " ";
            if (brlb == "1")
            {
                xml = xml + "�������=" + (char)34 + "סԺ" + (char)34 + " ";
            }
            else
            {
                xml = xml + "�������=" + (char)34 + "����" + (char)34 + " ";
            }
            xml = xml + "/>";
            xml = xml + "<�ٴ���ʷ><![CDATA[" + " " + "]]></�ٴ���ʷ>";
            xml = xml + "<�ٴ����><![CDATA[" + " " + "]]></�ٴ����>";
            xml = xml + "</LOGENE>";
            if (Debug == "1")
                log.WriteMyLog("���ص�xml�ַ���:" + xml);
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
                MessageBox.Show("����His���ݿ�ʧ�ܣ�");
                if (Debug == "1")
                    log.WriteMyLog("����His���ݿ�ʧ�ܣ�");
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
                MessageBox.Show("��ʼ���ɹ���");
            }
            else
            {
                MessageBox.Show("��ʼ��ʧ�ܣ���������ֵΪ��" + i.ToString());
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
                    MessageBox.Show("����SendEmr������������ֵΪ��" + i.ToString());
                }

            }
            else
            {
                MessageBox.Show("����His���ݿ�ʧ�ܣ�");
                if (Debug == "1")
                    log.WriteMyLog("����His���ݿ�ʧ�ܣ�");
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
                MessageBox.Show("���ض�̬�����"+dd.ToString());
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
            //MessageBox.Show("����HIS���ַ�����"+inxml);

            StringBuilder S2 = new StringBuilder(inxml);
            StringBuilder S3 = new StringBuilder(65536);

            object[] Parameters = new object[] { dllconn }; // ʵ��Ϊ 0 

            Type[] ParameterTypes = new Type[] { typeof(StringBuilder) }; // ʵ������Ϊ int 

            DLLWrapper.ModePass[] themode = new DLLWrapper.ModePass[] { DLLWrapper.ModePass.ByValue }; // ���ͷ�ʽΪֵ�� 

            Type Type_Return = typeof(int); // ��������Ϊ int 
            int xx = 0;
            try
            {
                xx = (int)func.Invoke(Parameters, ParameterTypes, themode, Type_Return);

            }
            catch
            {
                MessageBox.Show("����HIS���ݿ�ʧ�ܣ�");
                if (Debug == "1")
                    log.WriteMyLog("����HIS���ݿ�ʧ�ܣ�");

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
                    Parameters = new object[] { "JB01", S2, S3 }; // ʵ��Ϊ 3 
                    ParameterTypes = new Type[] { typeof(String), typeof(StringBuilder), typeof(StringBuilder) }; // ʵ������Ϊ pchar 
                    themode = new DLLWrapper.ModePass[] { DLLWrapper.ModePass.ByValue, DLLWrapper.ModePass.ByValue, DLLWrapper.ModePass.ByValue }; // ���ͷ�ʽΪֵ�� 
                    xx = (int)func.Invoke(Parameters, ParameterTypes, themode, Type_Return);
                }
                catch (Exception e1)
                {
                    MessageBox.Show("�����ַ�����ʽ���ԣ�" + e1);
                }

            }
            else
            {
                MessageBox.Show("����HIS���ݿ�ʧ�ܣ�");
                if (Debug == "1")
                    log.WriteMyLog("����HIS���ݿ�ʧ�ܣ�");

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
                //Parameters = new object[] { }; // ʵ��Ϊ 0
                //ParameterTypes = new Type[] { }; // ʵ������Ϊ pchar 
                //themode = new DLLWrapper.ModePass[] { }; // ���ͷ�ʽΪֵ�� 
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

                MessageBox.Show("δ��ѯ��������Ϣ��");
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
            // ������ʾ����ʾ���� myfun.Invoke �����Ľ���������� count ����                        

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
                MessageBox.Show("����His���ݿ�ʧ�ܣ�");
                if (Debug == "1")
                    log.WriteMyLog("����His���ݿ�ʧ�ܣ�");
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
            //MessageBox.Show("��ʼ������뵥");
            dllconn init = (dllconn)loaddll.InvokeMethod("Init", typeof(dllconn));
            disdllconn uninit = (disdllconn)loaddll.InvokeMethod("UnInit", typeof(disdllconn));
            commit sendemr = (commit)loaddll.InvokeMethod("SendEmr", typeof(commit));

            StringBuilder dllconn33 = new StringBuilder("");
            string retstring = "";
            int yy = init(dllconn33);
            //MessageBox.Show("�˴�������");
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
                //MessageBox.Show("�������뵥���ַ�����"+inxml);
                StringBuilder S2 = new StringBuilder(inxml);
                StringBuilder S3 = new StringBuilder(65536);

                try
                {
                    sendemr("JB03", S2, S3);
                }
                catch (Exception aa)
                {
                    MessageBox.Show("����" + aa.ToString());
                }
                retstring = S3.ToString();
            }
            else
            {
                MessageBox.Show("����His���ݿ�ʧ�ܣ�");
                if (Debug == "1")
                    log.WriteMyLog("����His���ݿ�ʧ�ܣ�");
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
            #region �����￨����
            /// <summary>
            /// �豸ID
            /// </summary>
            int _icdev = -1;
            /// <summary>
            /// ��ǰIC����
            /// </summary>
            int _cardId = 0;
            #endregion
            if (length > 720) throw new ArgumentException("Ҫ��ȡ�ĳ���̫��");

            StringBuilder data = new StringBuilder(length);   //��Ŷ�ȡ��������

            try
            {
                int result;                     //�豸API�������صĽ��

                #region ��ʼ���豸
                //�����豸�����ò�����
                _icdev = URF.rf_init(0, 9600);
                if (_icdev < 0)
                { MessageBox.Show(_icdev.ToString()); }
                //Ѱ��
                result = URF.rf_card((Int16)_icdev, 1, ref _cardId);
                if (result != 0)
                { MessageBox.Show(result.ToString()); }
                //��������
                result = URF.rf_load_key_hex(_icdev, 0, 0, "FFFFFFFFFFFF");
                if (result != 0)
                { MessageBox.Show(result.ToString()); }
                #endregion

                #region ��ȡ
                int readedBlock = 0;                    //��ʶ����,��ʶ�Ѷ�ȡ�Ŀ���

                int block = length / 16;                //Ҫ��ȡ���ַ�������ռ�Ŀ���
                if (length % 16 != 0) block++;          //������ַ�ռ����һ����Ͷ��һ��

                int sector = block / 3;                 //�ַ�����ռ������
                if (block % 3 != 0) sector++;           //�����ռ����һ������ͬ�����һ������

                //��������
                for (int sq = 1; sq <= sector; sq++)
                {
                    //��֤����������
                    result = URF.rf_authentication(_icdev, 0, sq);
                    if (result != 0)
                    {
                        // MessageBox.Show(result.ToString());
                    }
                    //������
                    for (int bk = 0; bk < 3; bk++)
                    {
                        if (readedBlock < block)
                        {
                            StringBuilder tempData = new StringBuilder(64);
                            result = URF.rf_read(_icdev, sq * 4 + bk, tempData);

                            //�������16λ�����������(ԭ��δ�.)
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
                //�ͷ��豸
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
