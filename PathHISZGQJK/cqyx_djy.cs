using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using dbbase;
using ZgqClassPub;
//using YXCISWrit;


namespace PathHISZGQJK
{
    // 都江堰人民医院
    class cdyx_djy// : LisReport
    {
        private static IniFiles f = new IniFiles(System.Windows.Forms.Application.StartupPath + "\\sz.ini");
        string ksbm = f.ReadString("ksmc", "ksbm", "304");//08（yj）
        string czyid = f.ReadString("yh", "yhbh", "").Replace("\0", "");

        string czyname = f.ReadString("yh", "yhmc", "").Replace("\0", "");
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        DataTable brxx = new DataTable();

        public void RTXML(string blh, string Debug1)
        {
            string Debug = f.ReadString("savetohis", "Debug", "").Replace("\0", "");
            brxx = aa.GetDataTable("select * from T_JCXX where F_blh='" + blh + "'", "brxx");
            DataTable yhqx = aa.GetDataTable("select * from t_yh where F_YHMC='" + czyname + "'", "yhqx");
            int brsta = 0; 
            try
            {
                if (brxx.Rows.Count > 0)
                {
              
                    string brlb = brxx.Rows[0]["F_BRLB"].ToString();
                    string hxbz = brxx.Rows[0]["F_HXBZ"].ToString().Trim();
                    string zyh = brxx.Rows[0]["F_zyh"].ToString().Trim();
                    string mzh = brxx.Rows[0]["F_mzh"].ToString().Trim();
                    string sqdh = brxx.Rows[0]["F_BGZT"].ToString().Trim();
                    string sqxh = brxx.Rows[0]["F_SQXH"].ToString().Trim();
                    string blk = brxx.Rows[0]["F_Blk"].ToString().Trim();
                    switch (brlb)
                    {
                        case "住院": brsta = 1; break;
                        case "门诊": brsta = 2; break;
                        case "体检": brsta = 3; break;
                    }

                    if (brxx.Rows[0]["F_SQXH"].ToString().Trim() != "")
                    {
                        
                        //LisReportClass lc = new LisReportClass();
                     
                        //bool b = lc.Connect(0, "40220", "159159"); ///HIS提供连接用户名
                        //if (Debug == "1")
                        //    MessageBox.Show(b.ToString());
                        //if (sqdh == "已审核")
                        //{   
                        //    string sq2 = "update T_JCXX set F_HXBZ='2' where f_blh='" + blh + "'";
                        //    if (b)
                        //    {
                        //        if (Debug == "1")
                        //            MessageBox.Show(b.ToString());
                        //        if (hxbz == "")
                        //        {
                        //            if (Debug == "1")
                        //                MessageBox.Show(b.ToString());
                        //            if (lc.WriteRegInfo(1, brsta, zyh + mzh, sqxh + "|" + czyid + "|" + czyname))
                        //            {
                        //                if (lc.DoPerform(1, brsta, zyh + mzh, sqxh, "*"))
                        //                {

                        //                    MessageBox.Show("执行成功！");
                        //                     LGZGQClass.log.WriteMyLog(blh + "执行成功！");     ///标志改为2,因为已经审核

                        //                }
                        //            }
                        //        }
                        //        if (hxbz == "2")
                        //        {

                        //            if (lc.RemoveReport(brsta, zyh + mzh, sqxh, "*"))
                        //            {  LGZGQClass.log.WriteMyLog(blh + "在HIS中删除成功！"); }

                        //        }

                        //        string xmlpath = "D:\\pathqc\\UploadFiles";
                        //        if (!System.IO.Directory.Exists(xmlpath))
                        //        {
                        //            System.IO.Directory.CreateDirectory(xmlpath);
                        //        }

                        //        string shys = brxx.Rows[0]["F_SHYS"].ToString();

                        //        XmlTextWriter xtw = new XmlTextWriter("" + xmlpath + "\\" + blh + ".xml", System.Text.Encoding.GetEncoding("GB2312"));
                        //        xtw.WriteStartDocument();
                        //        xtw.WriteStartElement("MSG");     ///根元素
                        //        #region MSH
                        //        xtw.WriteStartElement("MSH");     ///MSH块
                        //        xtw.WriteElementString("MSH.1", "BGD");
                        //        xtw.WriteElementString("MSH.2", "05");
                        //        xtw.WriteElementString("MSH.3", "");
                        //        xtw.WriteEndElement();

                        //        #endregion

                        //        #region BGD
                        //        xtw.WriteStartElement("BGD");
                        //        #region ZYX块
                        //        xtw.WriteStartElement("ZYX");
                        //        xtw.WriteElementString("ZYX.1", "'" + brxx.Rows[0]["F_ZYH"].ToString().Trim() + "'");

                        //        xtw.WriteEndElement();
                        //        #endregion

                        //        #region MZX
                        //        xtw.WriteStartElement("MZX");
                        //        xtw.WriteElementString("MZX.1", "'" + brxx.Rows[0]["F_MZH"].ToString().Trim() + "'");

                        //        xtw.WriteEndElement();
                        //        #endregion

                        //        #region SQD
                        //        xtw.WriteStartElement("SQD");
                        //        xtw.WriteElementString("SQD.1", "" + brxx.Rows[0]["F_SQXH"].ToString().Trim() + "");
                        //        xtw.WriteElementString("SQD.26", "" + brxx.Rows[0]["F_BLK"] + "");
                        //        xtw.WriteEndElement();
                        //        #endregion
                        //        xtw.WriteElementString("BGD.1", "" + brxx.Rows[0]["F_BLH"].ToString().Trim() + "");
                        //        xtw.WriteElementString("BGD.2", " ");
                        //        xtw.WriteElementString("BGD.3", " ");
                        //        xtw.WriteElementString("BGD.4", "" + brxx.Rows[0]["F_SDRQ"].ToString().Trim() + "");
                        //        xtw.WriteElementString("BGD.5", " ");
                        //        xtw.WriteElementString("BGD.6", "" + brxx.Rows[0]["F_BGRQ"].ToString().Trim() + "");
                        //        xtw.WriteElementString("BGD.7", "" + brxx.Rows[0]["F_BGYS"].ToString().Trim() + "");
                        //        xtw.WriteElementString("BGD.8", "" + brxx.Rows[0]["F_BLZD"].ToString().Trim() + "");
                        //        xtw.WriteElementString("BGD.56", "" + ksbm + "");
                        //        xtw.WriteElementString("BGD.57", "病理科");
                        //        xtw.WriteElementString("BGD.61", "" + brxx.Rows[0]["F_SPARE5"] + "");
                        //        xtw.WriteElementString("BGD.62", "" + czyid + "");
                        //        xtw.WriteElementString("BGD.63", "" + brxx.Rows[0]["F_SHYS"].ToString().Trim() + "");
                        //        xtw.WriteElementString("BGD.65", "" + brxx.Rows[0]["F_BBMC"].ToString().Trim() + "");
                        //        xtw.WriteEndElement();
                        //        #endregion
                        //        xtw.WriteEndElement();
                        //        xtw.WriteEndDocument();
                        //        xtw.Close();
                        //        aa.Close();
                        //        ///读取文件夹中xml文件
                        //        StreamReader obj = new StreamReader(xmlpath + "\\" + blh + ".xml", System.Text.Encoding.GetEncoding("gb2312"));
                        //        string xml = obj.ReadToEnd();
                        //        obj.Close();
                        //        if (xml.Trim() != "")
                        //        {
                        //            try
                        //            {
                        //                if (b)
                        //                {
                        //                    if (lc.WriteReport(brsta, zyh + mzh, sqxh, "*", xml))     ///此处调用com  上传xml函数。返回执行标志，如果为true
                        //                    {
                        //                        int exjcxx = aa.ExecuteSQL(sq2);
                        //                         LGZGQClass.log.WriteMyLog(blh + "报告回传成功！");
                        //                    }
                        //                }
                        //            }
                        //            catch (Exception e)
                        //            {
                        //                 LGZGQClass.log.WriteMyLog(blh + "回写报告失败！" + e.ToString());
                        //            }
                        //            DelFile(xmlpath, blh + ".xml");                           ///上传成功True删除指定 对应xml文件  
                        //        }
                        //        else
                        //        {
                        //             LGZGQClass.log.WriteMyLog(blh + "xml文件为空");
                        //        }

                        //    }

                        //}
                        //else
                        //{
                        //    if (hxbz == "2")
                        //    {
                        //        if (b)
                        //        {
                        //            if (Debug == "1")
                        //                MessageBox.Show("连接成功0");
                        //            if (lc.RemoveReport(brsta, zyh + mzh, "JC" + sqxh, "*"))
                        //            {
                        //                string sq1 = "update T_JCXX set F_HXBZ='1' where f_blh='" + blh + "'";
                        //                int exjcxx2 = aa.ExecuteSQL(sq1);
                        //                 LGZGQClass.log.WriteMyLog(blh + "在HIS中报告删除成功！");
                        //            }
                        //        }
                        //    }
                        //    else if (hxbz == "")
                        //    {

                        //        if (lc.WriteRegInfo(1, brsta, zyh + mzh, sqxh + "|" + czyid + "|" + czyname))
                        //        {

                        //            if (lc.DoPerform(1, brsta, zyh + mzh, sqxh, "*"))
                        //            {
                        //                string sq1 = "update T_JCXX set F_HXBZ='1' where f_blh='" + blh + "'";
                        //                int exjcxx2 = aa.ExecuteSQL(sq1);
                        //                 LGZGQClass.log.WriteMyLog(blh + "执行成功！");     ///标志改为1

                        //            }
                        //        }
                        //    }
                        //}

                        //lc.DisConnect();
                        //aa.Close();
                    }
                    else
                    {

                        log.WriteMyLog(blh + " 无申请单号不处理!");
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("抛出异常");
                log.WriteMyLog(blh + e.Message.ToString());
            }


        }
        public void DelFile(string filepath, string filename)
        {
            string path = filepath + "\\" + filename;
            if (File.Exists(path))
            {
                File.Delete(path);
                log.WriteMyLog(path + "已删除！");

            }

            else
            {
                log.WriteMyLog(filename + "不存在，删除失败！");
            }
        }







        #region ILisReport 成员

        public object AS_ApplyUpdates(string ProviderName, object Delta, int MaxErrors, out int ErrorCount, ref object OwnerData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object AS_DataRequest(string ProviderName, object Data)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AS_Execute(string ProviderName, string CommandText, ref object Params, ref object OwnerData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object AS_GetParams(string ProviderName, ref object OwnerData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object AS_GetProviderNames()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object AS_GetRecords(string ProviderName, int Count, out int RecsOut, int Options, string CommandText, ref object Params, ref object OwnerData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public object AS_RowRequest(string ProviderName, object Row, int RequestType, ref object OwnerData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool BatchConfirm(int OptType, int PatientClass, string Items)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public sbyte CheckAccount(string CBRH, string SheetID, string ItemData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool ConfirmItem(int OptType, int PatientClass, string PatientID, string SheetID, string ItemData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Connect(int AppHandle, string UID, string PWD)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool DisConnect()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool DoCharge(int OptType, int PatientClass, string PatientID, string SheetID, string ItemData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool DoPerform(int OptType, int PatientClass, string PatientID, string SheetID, string ItemData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool ExecCharge(string InValue, out string OutValue)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetLastError()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsConnected()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Login(string UID, string PWD)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OpenReqSheet(int PatientClass, string PatientID, string SheetID)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void PrintReqSheet(int PatientClass, string PatientID, string SheetID, int PreView)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string ReadCard(string CardNo)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool RemoveReport(int PatientClass, string PatientID, string SheetID, string ItemData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ShowPatientHealthInfo(int PatientClass, string PatientID)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool WriteRegInfo(int OprType, int PatientClass, string PatientID, string RegInfo)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool WriteReport(int PatientClass, string PatientID, string SheetID, string ItemData, string XMLDATA)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

}
