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

                XmlNode yy = readxml.SelectSingleNode(@"/LOGENE/�ٴ���ʷ");
                string LCBS = yy.InnerText;
                yy = readxml.SelectSingleNode(@"/LOGENE/�ٴ����");
                string LCZD = yy.InnerText;

               // LCZD = "\"12312";

                string xml2 = "<?xml version=\"1.0\" standalone=\"yes\"?>";
                xml2 = xml2 + "<DATAPACKET Version=\"2.0\">";
                xml2 = xml2 + "<METADATA>";
                xml2 = xml2 + "<FIELDS>";
                xml2 = xml2 + "<FIELD attrname=\"���˱��\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"����ID\" fieldtype=\"string\" WIDTH=\"100\"/>";
                xml2 = xml2 + "<FIELD attrname=\"�������\" fieldtype=\"string\" WIDTH=\"100\"/>";
                xml2 = xml2 + "<FIELD attrname=\"�����\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"סԺ��\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"����\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"�Ա�\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"����\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"����\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"��ַ\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"�绰\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"����\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"����\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"���֤��\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"����\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"ְҵ\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"�ͼ����\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"�ͼ�ҽ��\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"�շ�\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"�걾����\" fieldtype=\"string\" WIDTH=\"100\"/>";
                xml2 = xml2 + "<FIELD attrname=\"�ͼ�ҽԺ\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"ҽ����Ŀ\" fieldtype=\"string\" WIDTH=\"200\"/>";
                xml2 = xml2 + "<FIELD attrname=\"����1\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"����2\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"�ѱ�\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"�������\" fieldtype=\"string\" WIDTH=\"40\"/>";
                xml2 = xml2 + "<FIELD attrname=\"�ٴ���ʷ\" fieldtype=\"string\" WIDTH=\"2000\"/>";
                xml2 = xml2 + "<FIELD attrname=\"�ٴ����\" fieldtype=\"string\" WIDTH=\"2000\"/>";
                xml2 = xml2 + "</FIELDS>";
             //   xml2 = xml2 + "<PARAMS/>";
                xml2 = xml2 + "</METADATA>";
                xml2 = xml2 + "<ROWDATA>";
                xml2 = xml2 + "<ROW ";
                xml2 = xml2 + "���˱��=\"" + xx.Attributes["���˱��"].InnerText + "\" ";
                xml2 = xml2 + "����ID=\"" + xx.Attributes["����ID"].InnerText + "\" ";
                xml2 = xml2 + "�������=\"" + xx.Attributes["�������"].InnerText + "\" ";
                xml2 = xml2 + "�����=\"" + xx.Attributes["�����"].InnerText + "\" ";
                xml2 = xml2 + "סԺ��=\"" + xx.Attributes["סԺ��"].InnerText + "\" ";
                xml2 = xml2 + "����=\"" + xx.Attributes["����"].InnerText + "\" ";
                xml2 = xml2 + "�Ա�=\"" + xx.Attributes["�Ա�"].InnerText + "\" ";
                xml2 = xml2 + "����=\"" + xx.Attributes["����"].InnerText + "\" ";
                xml2 = xml2 + "����=\"" + xx.Attributes["����"].InnerText + "\" ";
                xml2 = xml2 + "��ַ=\"" + xx.Attributes["��ַ"].InnerText + "\" ";
                xml2 = xml2 + "�绰=\"" + xx.Attributes["�绰"].InnerText + "\" ";
                xml2 = xml2 + "����=\"" + xx.Attributes["����"].InnerText + "\" ";
                xml2 = xml2 + "����=\"" + xx.Attributes["����"].InnerText + "\" ";
                xml2 = xml2 + "���֤��=\"" + xx.Attributes["���֤��"].InnerText + "\" ";
                xml2 = xml2 + "����=\"" + xx.Attributes["����"].InnerText + "\" ";
                xml2 = xml2 + "ְҵ=\"" + xx.Attributes["ְҵ"].InnerText + "\" ";
                xml2 = xml2 + "�ͼ����=\"" + xx.Attributes["�ͼ����"].InnerText + "\" ";
                xml2 = xml2 + "�ͼ�ҽ��=\"" + xx.Attributes["�ͼ�ҽ��"].InnerText + "\" ";
                xml2 = xml2 + "�շ�=\"" + xx.Attributes["�շ�"].InnerText + "\" ";
                xml2 = xml2 + "�걾����=\"" + xx.Attributes["�걾����"].InnerText + "\" ";
                xml2 = xml2 + "�ͼ�ҽԺ=\"" + xx.Attributes["�ͼ�ҽԺ"].InnerText + "\" ";
                xml2 = xml2 + "ҽ����Ŀ=\"" + xx.Attributes["ҽ����Ŀ"].InnerText + "\" ";
                xml2 = xml2 + "����1=\"" + xx.Attributes["����1"].InnerText + "\" ";
                xml2 = xml2 + "����2=\"" + xx.Attributes["����2"].InnerText + "\" ";
                xml2 = xml2 + "�ѱ�=\"" + xx.Attributes["�ѱ�"].InnerText + "\" ";
                xml2 = xml2 + "�������=\"" + xx.Attributes["�������"].InnerText + "\" ";
                xml2 = xml2 + "�ٴ���ʷ=\"![CDATA[" + System.Security.SecurityElement.Escape(LCBS) + "]]\" ";
                xml2 = xml2 + "�ٴ����=\"![CDATA[" + System.Security.SecurityElement.Escape(LCZD) + "]]\" ";

                //xml2 = xml2 + "�ٴ���ʷ=\"" + "<2312312" + "\" ";
                //xml2 = xml2 + "�ٴ����=\"" + "\"3213123>" + "\" ";
                
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
