using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LGHISJKZGQ;


namespace PATHGETHISQL
{
    public class TJXQYY
    {


        /// <summary>
        ///  Json    钱龙
        /// </summary>
        /// <param name="Sslbx"></param>
        /// <param name="Ssbz"></param>
        /// <param name="Debug"></param>
        /// <returns></returns>
        public static string ptxml(string Sslbx, string Ssbz, string Debug)
        {
            string rexml = "";
            if (Sslbx == "申请单号")
            {
                rexml = GetZYBRXX(Ssbz, Debug);
                if (rexml == "0")
                {
                    MessageBox.Show("无此病人信息，请检查条码号是否正确！");

                }
            }
            return rexml;
        }

        private static string GetZYBRXX(string Ssbz, string Debug)
        {
            //string reqmessage=@"{""headers"" : {""channelFlag"" : ""PTLCCHANNEL"",""msgCode"" : ""HIS90040"",""sendTime"" : ""2015-03-13 09:24:57"",""msgType"" : ""noBroadcast"",""sendType"" : ""request"",""msgSerial"" : ""bd8a4b6f-eb9a-45c5-a3f0-198619a2a6ed"",""sysCode"" : ""PTLC"",""destination"" : ""HIS""},""payload"" : {""request"" : { ""applicationid"" : ""申请单号""}}}";
            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);

            writer.WriteStartObject();
            writer.WritePropertyName("headers");
            writer.WriteStartObject();
            writer.WritePropertyName("msgCode");
            writer.WriteValue("HIS90040");
            writer.WritePropertyName("channelFlag");
            writer.WriteValue("PTLCCHANNEL");
            writer.WritePropertyName("sendTime");
            writer.WriteValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            writer.WritePropertyName("sendType");
            writer.WriteValue("request");
            writer.WritePropertyName("msgType");
            writer.WriteValue("noBroadcast");
            writer.WritePropertyName("msgSerial");
            string uuid = Guid.NewGuid().ToString();
            writer.WriteValue(uuid);
            writer.WritePropertyName("sysCode");
            writer.WriteValue("PACS");
            writer.WritePropertyName("destination");
            writer.WriteValue("HIS");
            writer.WriteEndObject();
            writer.WritePropertyName("payload");
            writer.WriteStartObject();
            writer.WritePropertyName("request");
            writer.WriteStartObject();
            writer.WritePropertyName("applicationid");
            writer.WriteValue(Ssbz);
            writer.WriteEndObject();
            writer.WriteEndObject();
            writer.WriteEndObject();
            writer.Flush();

            //string reqtext = sw.GetStringBuilder().ToString();
            //TJXQYYWEB.pacsChannel tjxqweb = new PATHGETHISQL.TJXQYYWEB.pacsChannel();
            //IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
            //string url = f.ReadString("申请单号", "url", "http://192.168.15.195:8081/esb-gather/webservice/pacsChannel?wsdl");
            //tjxqweb.Url = url;
          //  string rontext = tjxqweb.invoke(reqtext);
            string rontext = "";
            JObject ja = (JObject)JsonConvert.DeserializeObject(rontext);
            //288442
            //288462

            if (ja["payload"]["response"]["result"].ToString() == "00000000")
            {
                //LogeneXmlHelper lxh = new LogeneXmlHelper();

                //lxh.F_SJKS = ja["payload"]["response"]["reqdepartmentname"].ToString();
                //lxh.F_SJYS = ja["payload"]["response"]["reqdoctorname"].ToString();
                ////lxh.F_YZXM = ja["payload"]["response"]["hischeckitem"].ToString();
                //lxh.F_LCBS = ja["payload"]["response"]["abstracthistory"].ToString();
                //lxh.F_LCZD = ja["payload"]["response"]["diagnose"].ToString();
                //lxh.F_MZ = ja["payload"]["response"]["folk"].ToString();
                //lxh.F_XM = ja["payload"]["response"]["patientname"].ToString();
                //lxh.F_BRBH = ja["payload"]["response"]["hispatientid"].ToString();
                //lxh.F_SQXH = ja["payload"]["response"]["applicationid"].ToString();
                //lxh.F_XB = ja["payload"]["response"]["sex"].ToString();
                //lxh.F_AGE = ja["payload"]["response"]["age"].ToString();
                //lxh.F_SFZH = ja["payload"]["response"]["idnumber"].ToString();
                //lxh.F_TELEPHONE = ja["payload"]["response"]["phonenumber"].ToString();
                //lxh.F_ADDRESS = ja["payload"]["response"]["address"].ToString();
                //lxh.F_BBMC = ja["payload"]["response"]["specimen"].ToString();
                //int zycs = int.Parse(ja["payload"]["response"]["visitid"].ToString());
                //if (zycs == 0)
                //{
                //    lxh.F_BRLB = "门诊";
                //    lxh.F_MZH = ja["payload"]["response"]["hispatientid"].ToString();
                //}
                //else if (zycs >= 1)
                //{
                //    lxh.F_BRLB = "住院";
                //    lxh.F_ZYH = ja["payload"]["response"]["hispatientid"].ToString();
                //    lxh.F_BQ = ja["payload"]["response"]["wardname"].ToString();
                //    lxh.F_CH = ja["payload"]["response"]["bedno"].ToString();
                //}
                //else
                //{
                //    lxh.F_BRLB = "其他";
                //}
               
                ////lxh.F_BY2 = uuid;
                //string resultxml = lxh.ReturnLogeneXML();
                //return resultxml;

                return "0";
            }
            else
            {
                MessageBox.Show("错误代码：" + ja["payload"]["response"]["result"].ToString() + "\t错误信息：" + ja["payload"]["response"]["resultText"].ToString());
                return "0";
            }
        }
    }
}