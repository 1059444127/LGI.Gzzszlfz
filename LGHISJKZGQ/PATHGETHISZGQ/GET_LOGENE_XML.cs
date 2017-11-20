using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace PATHGETHISZGQ
{
    class GET_LOGENE_XML
    {
        public static  string GETXML()
       {

            string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
            try
            {
                xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
            }
            catch
            {
            }

            try
            {
                xml = xml + "就诊ID=" + (char)34 +""+ (char)34 + " ";
            }
            catch
            {
            }

            try
            {
                xml = xml + "申请序号=" + (char)34 + ""+ (char)34 + " ";
            }
            catch
            {
            }

            try
            {
                xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
            }
            catch
            {
            }

            try
            {
                xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
            }
            catch
            {
            }

            try
            {
                xml = xml + "姓名=" + (char)34 + "" + (char)34 + " ";
            }
            catch
            {
            }

            try
            {
                xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
            }
            catch
            {
            }

                try
                {
                    xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                }
                catch
                { 
                }
               
              try
              {
                  xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
              }
            catch
            {
            }
                try
                {
                    xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                }
                try
                {
                    xml = xml + "电话=" + (char)34  + "" + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "电话=" + (char)34 + " " + (char)34 + " ";
                }

                 try
                {
                    xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                }
                catch
                {
                     xml = xml + "病区=" + (char)34 +"" + (char)34 + " ";
                }
                
                try
                {
                    xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                }
                try
                {
                    xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "身份证号=" + (char)34 + " " + (char)34 + " ";
                }
                try
                {
                    xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "民族=" + (char)34 + " " + (char)34 + " ";
                }
                try
                {
                    xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                }
                catch

                {
                    xml = xml + "职业=" + (char)34 + " " + (char)34 + " ";
                }
            try
            {
                xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
            }
            catch
            {
            }
            try
            {
                xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
            }
            catch
            {
            }
            try
            {
                xml = xml + "收费=" + (char)34 + (char)34 + " ";
            }
            catch
            {
            }
            try
            {
                xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
            }
            catch
            {
            }
            try
            {
                xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
            }
            catch
            {
            }
            try
            {
                xml = xml + "医嘱项目=" + (char)34 + (char)34 + " ";
            }
            catch
            {
            }
            try
            {
                xml = xml + "备用1=" + (char)34 + (char)34 + " ";
            }
            catch
            {
            }
            try
            {
                xml = xml + "备用2=" + (char)34 + (char)34 + " ";
            }
            catch
            {
            }
            xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
            try
            {
                xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
            }
            catch
            {
            }
                xml = xml + "/>";
            try
            {
                xml = xml + "<临床病史><![CDATA[" + " " + "]]></临床病史>";
            }
            catch
            {
            }
            try
            {
                xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
            }
            catch
            {
            }
                xml = xml + "</LOGENE>";

                return xml;




            }
    }
    
}
