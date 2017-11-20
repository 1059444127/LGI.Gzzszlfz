using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.IO;
using System.Data;


namespace LGHISJKZGQ
{
    class gyxxml
    {
        public static string zh(string xml)
        {
            try
            {

                XmlDocument readxml = new XmlDocument();
                readxml.LoadXml(xml);
                XmlNode xx = readxml.SelectSingleNode(@"/LOGENE/row");                

                XmlNode yy = readxml.SelectSingleNode(@"/LOGENE/临床病史");
                string LCBS = yy.InnerText;
                yy = readxml.SelectSingleNode(@"/LOGENE/临床诊断");
                string LCZD = yy.InnerText;

               // LCZD = "\"12312";

                string xml2 = "<?xml version=\"1.0\" standalone=\"yes\"?>";
                xml2 = xml2 + "<DATAPACKET Version=\"2.0\">";
                xml2 = xml2 + "<METADATA>";
                xml2 = xml2 + "<FIELDS>";
                xml2 = xml2 + "<FIELD attrname=\"病人编号\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"就诊ID\" fieldtype=\"string\" WIDTH=\"100\"/>";
                xml2 = xml2 + "<FIELD attrname=\"申请序号\" fieldtype=\"string\" WIDTH=\"100\"/>";
                xml2 = xml2 + "<FIELD attrname=\"门诊号\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"住院号\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"姓名\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"性别\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"年龄\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"婚姻\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"地址\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"电话\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"病区\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"床号\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"身份证号\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"民族\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"职业\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"送检科室\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"送检医生\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"收费\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"标本名称\" fieldtype=\"string\" WIDTH=\"100\"/>";
                xml2 = xml2 + "<FIELD attrname=\"送检医院\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"医嘱项目\" fieldtype=\"string\" WIDTH=\"200\"/>";
                xml2 = xml2 + "<FIELD attrname=\"备用1\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"备用2\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"费别\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"病人类别\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"临床病史\" fieldtype=\"string\" WIDTH=\"2000\"/>";
                xml2 = xml2 + "<FIELD attrname=\"临床诊断\" fieldtype=\"string\" WIDTH=\"2000\"/>";
                xml2 = xml2 + "</FIELDS>";
             //   xml2 = xml2 + "<PARAMS/>";
                xml2 = xml2 + "</METADATA>";
                xml2 = xml2 + "<ROWDATA>";
                xml2 = xml2 + "<ROW ";
                xml2 = xml2 + "病人编号=\"" + xx.Attributes["病人编号"].InnerText + "\" ";
                xml2 = xml2 + "就诊ID=\"" + xx.Attributes["就诊ID"].InnerText + "\" ";
                xml2 = xml2 + "申请序号=\"" + xx.Attributes["申请序号"].InnerText + "\" ";
                xml2 = xml2 + "门诊号=\"" + xx.Attributes["门诊号"].InnerText + "\" ";
                xml2 = xml2 + "住院号=\"" + xx.Attributes["住院号"].InnerText + "\" ";
                xml2 = xml2 + "姓名=\"" + xx.Attributes["姓名"].InnerText + "\" ";
                xml2 = xml2 + "性别=\"" + xx.Attributes["性别"].InnerText + "\" ";
                xml2 = xml2 + "年龄=\"" + xx.Attributes["年龄"].InnerText + "\" ";
                xml2 = xml2 + "婚姻=\"" + xx.Attributes["婚姻"].InnerText + "\" ";
                xml2 = xml2 + "地址=\"" + xx.Attributes["地址"].InnerText + "\" ";
                xml2 = xml2 + "电话=\"" + xx.Attributes["电话"].InnerText + "\" ";
                xml2 = xml2 + "病区=\"" + xx.Attributes["病区"].InnerText + "\" ";
                xml2 = xml2 + "床号=\"" + xx.Attributes["床号"].InnerText + "\" ";
                xml2 = xml2 + "身份证号=\"" + xx.Attributes["身份证号"].InnerText + "\" ";
                xml2 = xml2 + "民族=\"" + xx.Attributes["民族"].InnerText + "\" ";
                xml2 = xml2 + "职业=\"" + xx.Attributes["职业"].InnerText + "\" ";
                xml2 = xml2 + "送检科室=\"" + xx.Attributes["送检科室"].InnerText + "\" ";
                xml2 = xml2 + "送检医生=\"" + xx.Attributes["送检医生"].InnerText + "\" ";
                xml2 = xml2 + "收费=\"" + xx.Attributes["收费"].InnerText + "\" ";
                xml2 = xml2 + "标本名称=\"" + xx.Attributes["标本名称"].InnerText + "\" ";
                xml2 = xml2 + "送检医院=\"" + xx.Attributes["送检医院"].InnerText + "\" ";
                xml2 = xml2 + "医嘱项目=\"" + xx.Attributes["医嘱项目"].InnerText + "\" ";
                xml2 = xml2 + "备用1=\"" + xx.Attributes["备用1"].InnerText + "\" ";
                xml2 = xml2 + "备用2=\"" + xx.Attributes["备用2"].InnerText + "\" ";
                xml2 = xml2 + "费别=\"" + xx.Attributes["费别"].InnerText + "\" ";
                xml2 = xml2 + "病人类别=\"" + xx.Attributes["病人类别"].InnerText + "\" ";
                xml2 = xml2 + "临床病史=\"![CDATA[" + System.Security.SecurityElement.Escape(LCBS) + "]]\" ";
                xml2 = xml2 + "临床诊断=\"![CDATA[" + System.Security.SecurityElement.Escape(LCZD) + "]]\" ";

                //xml2 = xml2 + "临床病史=\"" + "<2312312" + "\" ";
                //xml2 = xml2 + "临床诊断=\"" + "\"3213123>" + "\" ";
                
                xml2 = xml2 + "/>";
                xml2 = xml2 + "</ROWDATA>";
                xml2 = xml2 + "</DATAPACKET>";
                return xml2;
            }
            catch
            {
                return "0";
            }
           
        }
    }
}
