using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace LGHISJKZGQ
{
    class PT_XML
    {
        public Dictionary<string, string> myDictionary = new Dictionary<string, string>();

        public PT_XML()
        {
            myDictionary.Add("���˱��", "");
            myDictionary.Add("����ID", "");
            myDictionary.Add("�������", "");
            myDictionary.Add("�����", "");
            myDictionary.Add("סԺ��", "");
            myDictionary.Add("����", "");
            myDictionary.Add("�Ա�", "");
            myDictionary.Add("����", "");
            myDictionary.Add("����", "");
            myDictionary.Add("��ַ", "");
            myDictionary.Add("�绰", "");
            myDictionary.Add("����", "");
            myDictionary.Add("����", "");
            myDictionary.Add("���֤��", "");
            myDictionary.Add("����", "");
            myDictionary.Add("ְҵ", "");
            myDictionary.Add("�ͼ����", "");
            myDictionary.Add("�ͼ�ҽ��", "");
            myDictionary.Add("�շ�", "0");
            myDictionary.Add("�걾����", "");
            myDictionary.Add("�ͼ�ҽԺ", "��Ժ");
            myDictionary.Add("ҽ����Ŀ", "");
            myDictionary.Add("����1", "");
            myDictionary.Add("����2", "");
            myDictionary.Add("�ѱ�", "");
            myDictionary.Add("�������", "");
            myDictionary.Add("�ٴ���ʷ", "");
            myDictionary.Add("�ٴ����", "");
            myDictionary.Add("��������", "");
        }

        public string rtn_XML(ref string exep)
        {
            try
            {
                exep = "";
                string xml = "<?xml version=" + (char)34 + "1.0" + (char)34 + " encoding=" + (char)34 + "gbk" + (char)34 + "?>";
                xml = xml + "<LOGENE>";
                xml = xml + "<row ";
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "���˱��=" + (char)34 + myDictionary["���˱��"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "��ȡ�ֶΣ����˱���쳣\r\n";
                    xml = xml + "���˱��=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "����ID=" + (char)34 + myDictionary["����ID"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "��ȡ�ֶΣ�����ID�쳣\r\n";
                    xml = xml + "����ID=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�������=" + (char)34 + myDictionary["�������"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "��ȡ�ֶΣ���������쳣\r\n";
                    xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�����=" + (char)34 + myDictionary["�����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "��ȡ�ֶΣ�������쳣\r\n";
                    xml = xml + "�����=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "סԺ��=" + (char)34 + myDictionary["סԺ��"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "��ȡ�ֶΣ�סԺ���쳣\r\n";
                    xml = xml + "סԺ��=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                xml = xml + "����=" + (char)34 + myDictionary["����"].Trim() + (char)34 + " ";
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�Ա�=" + (char)34 + myDictionary["�Ա�"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�Ա�=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��Ա��쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "����=" + (char)34 + myDictionary["����"].Trim().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ������쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "����=" + (char)34 + myDictionary["����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ������쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "��ַ=" + (char)34 + myDictionary["��ַ"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "��ַ=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ���ַ�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�绰=" + (char)34 + myDictionary["�绰"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�绰=" + (char)34 + " " + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��绰�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "����=" + (char)34 + myDictionary["����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ������쳣\r\n";
                }

                /////////////////////////////////////////////////////////////////

                try
                {
                    xml = xml + "����=" + (char)34 + myDictionary["����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ������쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "���֤��=" + (char)34 + myDictionary["���֤��"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "���֤��=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ����֤���쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "����=" + (char)34 + myDictionary["����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ������쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "ְҵ=" + (char)34 + myDictionary["ְҵ"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "ְҵ=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ�ְҵ�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�ͼ����=" + (char)34 + myDictionary["�ͼ����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�ͼ����=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��ͼ�����쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + myDictionary["�ͼ�ҽ��"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�ͼ�ҽ��=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��ͼ�ҽ���쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�շ�=" + (char)34 + myDictionary["�շ�"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�շ�=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��շ��쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�걾����=" + (char)34 + myDictionary["�걾����"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�걾����=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��걾�����쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + myDictionary["�ͼ�ҽԺ"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�ͼ�ҽԺ=" + (char)34 + "��Ժ" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��ͼ�ҽԺ�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "ҽ����Ŀ=" + (char)34 + myDictionary["ҽ����Ŀ"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "ҽ����Ŀ=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ�ҽ����Ŀ�쳣\r\n";
                }


                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "����1=" + (char)34 + myDictionary["����1"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����1=" + (char)34 + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ�����1�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "����2=" + (char)34 + myDictionary["����2"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "����2=" + (char)34 + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ�����2�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�ѱ�=" + (char)34 + myDictionary["�ѱ�"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�ѱ�=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ��ѱ��쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "�������=" + (char)34 + myDictionary["�������"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "�������=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "��ȡ�ֶΣ���������쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                xml = xml + "/>";

                try
                {
                    xml = xml + "<�ٴ���ʷ><![CDATA[" + myDictionary["�ٴ���ʷ"].Trim() + "]]></�ٴ���ʷ>";
                }
                catch
                {
                    xml = xml + "<�ٴ���ʷ><![CDATA[" + "" + "]]></�ٴ���ʷ>";
                    exep = exep + "��ȡ�ֶΣ��ٴ���ʷ�쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "<�ٴ����><![CDATA[" + myDictionary["�ٴ����"].Trim() + "]]></�ٴ����>";
                }
                catch
                {
                    xml = xml + "<�ٴ����><![CDATA[" + "" + "]]></�ٴ����>";
                    exep = exep + "��ȡ�ֶΣ��ٴ�����쳣\r\n";
                }
                /////////////////////////////////////////////////////////////////
                xml = xml + "</LOGENE>";

                return xml;
            }
            catch
            {
                return "0";
            }

        }

        ////myDictionary["���˱��"]="";
        ////myDictionary["����ID"]="";
        ////myDictionary["�������"]="";
        ////myDictionary["�����"]="";
        ////myDictionary["סԺ��"]="";
        ////myDictionary["����"]="";
        ////myDictionary["����"]="";
        ////myDictionary["����"]="";
        ////myDictionary["��ַ"]="";
        ////myDictionary["�绰"]="";
        ////myDictionary["����"]="";
        ////myDictionary["����"]="";
        ////myDictionary["���֤��"]="";
        ////myDictionary["����"]="";
        ////myDictionary["ְҵ"]="";
        ////myDictionary["�ͼ����"]="";
        ////myDictionary["�ͼ�ҽ��"]="";
        ////myDictionary["�շ�"]="";
        ////myDictionary["�걾����"]="";
        ////myDictionary["�ͼ�ҽԺ"]="";
        ////myDictionary["ҽ����Ŀ"]="";
        ////myDictionary["����1"]="";
        ////myDictionary["����2"]="";
        ////myDictionary["�ѱ�"]="";
        ////myDictionary["�������"]="";
        ////myDictionary["�ٴ���ʷ"]="";
        ////myDictionary["�ٴ����"]="";
    }
}
