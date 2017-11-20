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
            myDictionary.Add("病人编号", "");
            myDictionary.Add("就诊ID", "");
            myDictionary.Add("申请序号", "");
            myDictionary.Add("门诊号", "");
            myDictionary.Add("住院号", "");
            myDictionary.Add("姓名", "");
            myDictionary.Add("性别", "");
            myDictionary.Add("年龄", "");
            myDictionary.Add("婚姻", "");
            myDictionary.Add("地址", "");
            myDictionary.Add("电话", "");
            myDictionary.Add("病区", "");
            myDictionary.Add("床号", "");
            myDictionary.Add("身份证号", "");
            myDictionary.Add("民族", "");
            myDictionary.Add("职业", "");
            myDictionary.Add("送检科室", "");
            myDictionary.Add("送检医生", "");
            myDictionary.Add("收费", "0");
            myDictionary.Add("标本名称", "");
            myDictionary.Add("送检医院", "本院");
            myDictionary.Add("医嘱项目", "");
            myDictionary.Add("备用1", "");
            myDictionary.Add("备用2", "");
            myDictionary.Add("费别", "");
            myDictionary.Add("病人类别", "");
            myDictionary.Add("临床病史", "");
            myDictionary.Add("临床诊断", "");
            myDictionary.Add("出生日期", "");
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
                    xml = xml + "病人编号=" + (char)34 + myDictionary["病人编号"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "提取字段：病人编号异常\r\n";
                    xml = xml + "病人编号=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "就诊ID=" + (char)34 + myDictionary["就诊ID"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "提取字段：就诊ID异常\r\n";
                    xml = xml + "就诊ID=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "申请序号=" + (char)34 + myDictionary["申请序号"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "提取字段：申请序号异常\r\n";
                    xml = xml + "申请序号=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "门诊号=" + (char)34 + myDictionary["门诊号"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "提取字段：门诊号异常\r\n";
                    xml = xml + "门诊号=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "住院号=" + (char)34 + myDictionary["住院号"].Trim() + (char)34 + " ";
                }
                catch
                {
                    exep = exep + "提取字段：住院号异常\r\n";
                    xml = xml + "住院号=" + (char)34 + "" + (char)34 + " ";
                }
                /////////////////////////////////////////////////////////////////
                xml = xml + "姓名=" + (char)34 + myDictionary["姓名"].Trim() + (char)34 + " ";
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "性别=" + (char)34 + myDictionary["性别"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "性别=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：性别异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "年龄=" + (char)34 + myDictionary["年龄"].Trim().Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "年龄=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：年龄异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "婚姻=" + (char)34 + myDictionary["婚姻"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "婚姻=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：婚姻异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "地址=" + (char)34 + myDictionary["地址"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "地址=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：地址异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "电话=" + (char)34 + myDictionary["电话"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "电话=" + (char)34 + " " + (char)34 + " ";
                    exep = exep + "提取字段：电话异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "病区=" + (char)34 + myDictionary["病区"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "病区=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：病区异常\r\n";
                }

                /////////////////////////////////////////////////////////////////

                try
                {
                    xml = xml + "床号=" + (char)34 + myDictionary["床号"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "床号=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：床号异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "身份证号=" + (char)34 + myDictionary["身份证号"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "身份证号=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：身份证号异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "民族=" + (char)34 + myDictionary["民族"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "民族=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：民族异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "职业=" + (char)34 + myDictionary["职业"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "职业=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：职业异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "送检科室=" + (char)34 + myDictionary["送检科室"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "送检科室=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：送检科室异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "送检医生=" + (char)34 + myDictionary["送检医生"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "送检医生=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：送检医生异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "收费=" + (char)34 + myDictionary["收费"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "收费=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：收费异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "标本名称=" + (char)34 + myDictionary["标本名称"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "标本名称=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：标本名称异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "送检医院=" + (char)34 + myDictionary["送检医院"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "送检医院=" + (char)34 + "本院" + (char)34 + " ";
                    exep = exep + "提取字段：送检医院异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "医嘱项目=" + (char)34 + myDictionary["医嘱项目"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "医嘱项目=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：医嘱项目异常\r\n";
                }


                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "备用1=" + (char)34 + myDictionary["备用1"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "备用1=" + (char)34 + (char)34 + " ";
                    exep = exep + "提取字段：备用1异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "备用2=" + (char)34 + myDictionary["备用2"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "备用2=" + (char)34 + (char)34 + " ";
                    exep = exep + "提取字段：备用2异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "费别=" + (char)34 + myDictionary["费别"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "费别=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：费别异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "病人类别=" + (char)34 + myDictionary["病人类别"].Trim() + (char)34 + " ";
                }
                catch
                {
                    xml = xml + "病人类别=" + (char)34 + "" + (char)34 + " ";
                    exep = exep + "提取字段：病人类别异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                xml = xml + "/>";

                try
                {
                    xml = xml + "<临床病史><![CDATA[" + myDictionary["临床病史"].Trim() + "]]></临床病史>";
                }
                catch
                {
                    xml = xml + "<临床病史><![CDATA[" + "" + "]]></临床病史>";
                    exep = exep + "提取字段：临床病史异常\r\n";
                }
                /////////////////////////////////////////////////////////////////
                try
                {
                    xml = xml + "<临床诊断><![CDATA[" + myDictionary["临床诊断"].Trim() + "]]></临床诊断>";
                }
                catch
                {
                    xml = xml + "<临床诊断><![CDATA[" + "" + "]]></临床诊断>";
                    exep = exep + "提取字段：临床诊断异常\r\n";
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

        ////myDictionary["病人编号"]="";
        ////myDictionary["就诊ID"]="";
        ////myDictionary["申请序号"]="";
        ////myDictionary["门诊号"]="";
        ////myDictionary["住院号"]="";
        ////myDictionary["姓名"]="";
        ////myDictionary["年龄"]="";
        ////myDictionary["婚姻"]="";
        ////myDictionary["地址"]="";
        ////myDictionary["电话"]="";
        ////myDictionary["病区"]="";
        ////myDictionary["床号"]="";
        ////myDictionary["身份证号"]="";
        ////myDictionary["民族"]="";
        ////myDictionary["职业"]="";
        ////myDictionary["送检科室"]="";
        ////myDictionary["送检医生"]="";
        ////myDictionary["收费"]="";
        ////myDictionary["标本名称"]="";
        ////myDictionary["送检医院"]="";
        ////myDictionary["医嘱项目"]="";
        ////myDictionary["备用1"]="";
        ////myDictionary["备用2"]="";
        ////myDictionary["费别"]="";
        ////myDictionary["病人类别"]="";
        ////myDictionary["临床病史"]="";
        ////myDictionary["临床诊断"]="";
    }
}
