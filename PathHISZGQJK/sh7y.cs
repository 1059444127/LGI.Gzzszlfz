using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using dbbase;
using System.IO;
using Newtonsoft.Json;
using ZgqClassPub;

namespace PathHISZGQJK
{
    class sh7y
    {

      private static IniFiles f = new IniFiles(Application.StartupPath+"\\sz.ini");
      
        public void pathtohis(string blh, string yymc)
        {
            string url = f.ReadString("savetohis", "webservicesurl", "");
            string debug = f.ReadString("savetohis", "debug", "");


            dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            DataTable bljc = new DataTable();
            bljc = aa.GetDataTable("select * from T_jcxx where F_blh='" + blh + "'", "blxx");
            if (bljc == null)
            {
                MessageBox.Show("病理数据库设置有问题！");
                log.WriteMyLog("病理数据库设置有问题！");
                return;
            }
            if (bljc.Rows.Count < 1)
            {
                MessageBox.Show("病理号有错误！");
                log.WriteMyLog("病理号有错误！");
                return;
            }
            if (bljc.Rows[0]["F_sqxh"].ToString().Trim() == "")
            {
                log.WriteMyLog("无申请序号（单据号），不处理！");
                return;
            }

            sh7yWeb.webserviceService web7y = new PathHISZGQJK.sh7yWeb.webserviceService();

           
            string jyzx = "";

            StringWriter sw = new StringWriter();
            JsonWriter writer = new JsonTextWriter(sw);
            writer.WriteStartObject();
            writer.WritePropertyName("studyid");
            writer.WriteValue(bljc.Rows[0]["F_BRBH"].ToString().Trim());
            writer.WritePropertyName("Itemcode");
            writer.WriteValue(bljc.Rows[0]["F_YZXM"].ToString().Split('^')[0].Trim());
            writer.WritePropertyName("Result");
            writer.WriteValue(bljc.Rows[0]["F_BLZD"].ToString().Trim());
            writer.WritePropertyName("itemunicode");
            writer.WriteValue(bljc.Rows[0]["F_BLH"].ToString().Trim());
            writer.WriteEndObject();
            writer.Flush();
            string jsonstr = sw.GetStringBuilder().ToString();

           string  rtn_xml =web7y.validateItem(jsonstr, jyzx);



           StringWriter sw2 = new StringWriter();
           JsonWriter writer2 = new JsonTextWriter(sw2);
           writer2.WriteStartObject();
           writer2.WritePropertyName("studyid");
           writer2.WriteValue(bljc.Rows[0]["F_BRBH"].ToString().Trim());
           writer2.WritePropertyName("Itemcode");
           writer2.WriteValue(bljc.Rows[0]["F_YZXM"].ToString().Split('^')[0].Trim());
           writer2.WritePropertyName("jcrq");
           writer2.WriteValue(bljc.Rows[0]["F_SDRQ"].ToString().Trim());
           writer2.WritePropertyName("jcsj");
           writer2.WriteValue(bljc.Rows[0]["F_BGRQ"].ToString().Trim());
           writer2.WritePropertyName("jcry");
           writer2.WriteValue(bljc.Rows[0]["F_BGYS"].ToString().Trim());
           writer2.WritePropertyName("jgtxt");
           writer2.WriteValue(bljc.Rows[0]["F_BLZD"].ToString().Trim());
           writer2.WritePropertyName("unit");
           writer2.WriteValue("");
           writer2.WritePropertyName("yxbz");
           writer2.WriteValue(bljc.Rows[0]["F_YYX"].ToString().Trim());
           writer2.WritePropertyName("defvalue");
           writer2.WriteValue("");
           writer2.WriteEndObject();
           writer2.Flush();
           string jsonstr2 = sw2.GetStringBuilder().ToString();

           string rtn_xml2 = web7y.updateItem(jsonstr2, jyzx);


        }
    }
}
