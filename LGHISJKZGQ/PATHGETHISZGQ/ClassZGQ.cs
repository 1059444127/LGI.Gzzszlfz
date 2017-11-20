using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace LGHISJKZGQ
{
   

    [ComVisible(true)]
    public interface iClass1
    {
        string LGGetHISINFO(string sHISName, string Sslbx, string Ssbz, string Debug,string by);
        string LGGetHtml(string sHISName, string yzid, string brlb, string by);
        string ASPGetXX(string zyh);
    }
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]

    public class lgjkzgq:iClass1
    {
        /// <summary>
        ///取 ＨＩＳ 信息接口
        /// </summary>
        /// <param name="sHISName">医院名称</param>
        /// <param name="Sslbx">识别类型</param>
        /// <param name="Ssbz">识别值</param>
        /// <param name="Debug">是否写日志</param>
        /// <param name="by">备用</param>
        /// <returns></returns>
        public string LGGetHISINFO(string sHISName, string Sslbx, string Ssbz, string Debug,string by)
        {

            ///Pweb
            if (sHISName.ToUpper() == "PWEB")
            {
                return "123456789012345";
            }
            if (sHISName.ToUpper() == "ZSDXZLYYFZ")
            {
                return zsdxzlyy_fz.ptxml(Sslbx, Ssbz, Debug);
            }
            ///新疆肿瘤
            if (sHISName.ToUpper() == "XJZLYY")
            {
                return xjzlyy.ptxml(Sslbx, Ssbz, Debug);
            }
            //威海市立
            if (sHISName.ToUpper() == "WHSLYY")
            {
                return sdswhslyy.ptxml(Sslbx, Ssbz, Debug);
            }

            //锦州
            if (sHISName.ToUpper() == "JZYKDXFSYY")
            {
                return jzykdxfsyy.ptxml(Sslbx, Ssbz, Debug);
            }

            ///湘雅2院
            if (sHISName.ToUpper() == "XY2Y")
            {
                return xy2y.ptxml(Sslbx, Ssbz, Debug);
            }


            ///中山大学眼科医院
            if (sHISName.ToUpper() == "ZSDXYKYY")
            {
                return zsdxykyy.ptxml(Sslbx, Ssbz, Debug);
            }

            ///广东省中医院   住院 Servlet /Json
            if (sHISName.ToUpper() == "GDSZYY")
            {
                return gdszyy.ptxml(Sslbx, Ssbz, Debug);
            }
          
            ///张家港市第一人民医院
            if (sHISName.ToUpper() == "ZJGSDYRMYY")
            {
                return zjgsdyrmyy.ptxml(Sslbx, Ssbz, Debug);
            }

            ///东莞市东华医院
            if (sHISName.ToUpper() == "DGSDHYY")
            {
                return dgdhyy.ptxml(Sslbx, Ssbz, Debug);
            }

            ///福州市第二医院
            if (sHISName.ToUpper() == "FZS2Y")
            {
                return fzs2y.ptxml(Sslbx, Ssbz, Debug);
            }

            ///安医大二附院  
            if (sHISName.ToUpper() == "AYD2FY")
            {
                return AYD2FY.ptxml(Sslbx, Ssbz, Debug);
            }
            ///存储过程
            if (sHISName.ToUpper() == "CCGC")
            {
                return SP_SELECT.ptxml(Sslbx, Ssbz, Debug);
            }
            ///多条数据选择窗
            if (sHISName.ToUpper() == "DTSJXS")
            {
                return SP_SELECT.ptxml2(Sslbx, Ssbz, Debug);
            }
            ////宁波北仑人民医院
            if (sHISName.ToUpper() == "NBBLMRYY") 
            {
                return NBBLRMYY.ptxml(Sslbx, Ssbz, Debug);
            }
            //////盛泽医院
            if (sHISName.ToUpper() == "SZSZYY") 
            {
                return SZSZYY.ptxml(Sslbx, Ssbz, Debug);
            }
            //////宁波鄞州人民医院
            if (sHISName.ToUpper() == "NBYZRMYY")
            {
                return NBYZRMYY.ptxml(Sslbx, Ssbz, Debug);
            }
            ////长沙中心医院
            if (sHISName.ToUpper() == "CSZXYY")
            {
                return CSZXYY.ptxml(Sslbx, Ssbz, Debug);
            }
            ////武汉协和医院
            if (sHISName.ToUpper() == "WHXHYY")
            {
                return WHXHYY.ptxml(Sslbx, Ssbz, Debug);
            }
            //////安徽省肿瘤医院
            if (sHISName.ToUpper() == "AHSZLYY")
            {
                return AHSZLYY.ptxml(Sslbx, Ssbz, Debug);
            }
            //////宁波鄞州第二医院
            if (sHISName.ToUpper() == "NBYZD2YY")
            {
                return NBYZ2Y.ptxml(Sslbx, Ssbz, Debug);
            }
            ////山西肿瘤医院
            if (sHISName.ToUpper() == "SXSZLYY")
            {
                return SXSZLYY.ptxml(Sslbx, Ssbz, Debug);
            }
            ////重庆市医科大学附属永川医院
            if (sHISName.ToUpper() == "CQYCYY")
            {
                return CQYCYY.ptxml(Sslbx, Ssbz, Debug);
            }
            ////湘雅3院
            if (sHISName.ToUpper() == "XY3Y")
            {
                return XY3Y.xy3yxml(Sslbx, Ssbz, Debug);
            }
            ////湘雅附一医院
            if (sHISName.ToUpper() == "XYFYYY")
            {
                return XYFYYY.ptxml(Sslbx, Ssbz, Debug);

            }
            //// 武夷山市立医院
            if (sHISName.ToUpper() == "WYSSLYY")
            {
                return WYSSLYY.ptxml(Sslbx, Ssbz, Debug);                                         
            }
            //// 汕头中心医院
            if (sHISName.ToUpper() == "STZXYY")
            {
                return STZXYY.ptxml(Sslbx, Ssbz, Debug);
            }
            //// 北京宣武医院
            if (sHISName.ToUpper() == "BJXWYY")
            {
                return bjxwyy.ptxml(Sslbx, Ssbz, Debug);
            }
            //// 北京航天医院
            if (sHISName.ToUpper() == "BJHTYY")
            {
                return BJHTYY.ptxml(Sslbx, Ssbz, Debug);
            }
            ////福建省省立医院
            if (sHISName.ToUpper() == "FJSSLYY")
            {
                return FJSSLYY.ptxml(Sslbx, Ssbz, Debug);
            }
            ////邵阳市中心医院--HL7-撤销申请单
            if (sHISName.ToUpper() == "SYSZXYY")
            {
                return SYSZXYY.ptxml(Sslbx, Ssbz, Debug);
            }
            ////湖南省人民医院   webservices
            if (sHISName.ToUpper() == "HNSRMYY")
            {
                return HNSRMYY.ptxml(Sslbx, Ssbz, Debug);
            }
            ////长沙市妇幼  jsdwn'dll  蔡
            if (sHISName.ToUpper() == "CSSFYBJY")
            {
                return LGHISJK.cssfybjy.cssfybjyhx(Sslbx, Ssbz, Debug);
            }
            ////长沙市妇幼  体检
            if (sHISName.ToUpper() == "CSSFY")
            {
                return CSSFY.ptxml(Sslbx, Ssbz, Debug);
            }
            ////上海市仁济医院  接口  jsdwn'dll
            if (sHISName.ToUpper() == "SHSRJYY")
            {
                return LGHISJK.shsrjyy.shsrjyyXML(Sslbx, Ssbz, Debug);
            }
            ////山西大二院  webservices 西门子
            if (sHISName.ToUpper() == "SXDEY")
            {
                return SXDEY.ptxml(Sslbx, Ssbz, Debug);
            }
            ////中山肿瘤 webservices
            if (sHISName.ToUpper() == "ZSZL")
            {
                return zsdxzlyy.ptxml(Sslbx, Ssbz, Debug);
            }
            ////中山肿瘤 平台
            if (sHISName.ToUpper() == "ZSZLPT")
            {
                return zsdxzlyyPT.ptxml(Sslbx, Ssbz, Debug);
            }
            ////南京胸科医院 webservices
            if (sHISName.ToUpper() == "NJXKYY")
            {
                return LGHISJK.njxkyy.njxkyyXML(Sslbx, Ssbz, Debug);
            }
            ////福建省妇幼保健院 webservices+HL7
            if (sHISName.ToUpper() == "FJSFY")
            {
                return FJSFYBJY.ptxml(Sslbx, Ssbz, Debug);
            }
            ////上海市嘉定区中医医院 webservices
            if (sHISName.ToUpper() == "SHJDZYYY")
            {
                return shsjdqzyyy.ptxml(Sslbx, Ssbz, Debug);
            }
            //// 常德市第一人民医院 平台
            if (sHISName.ToUpper() == "CDSDYRMYY")
            {
                return cdsdyrmyy.ptxml(Sslbx, Ssbz, Debug);
            }
            //// 南京中大医院
            if (sHISName.ToUpper() == "NJZDYY")
            {
                return njzdyy.ptxml(Sslbx, Ssbz, Debug);
            }
            //// 石河子一附院
            if (sHISName.ToUpper() == "XJSHZYY")
            {
                return xjshzyy.ptxml(Sslbx, Ssbz, Debug);
            }
            //// 石河子一附院  体检接口--czf
            if (sHISName.ToUpper() == "SHZ1FY")
            {
                return shz1fy.cz1yxml(Sslbx, Ssbz, Debug);
            }
            //// 宁波象山人民医院  视图
            if (sHISName.ToUpper() == "NBXSRMYY")
            {
                return nbxsrmyy.ptxml(Sslbx, Ssbz, Debug);
            }
            //// 厦门第一医院
            if (sHISName.ToUpper() == "XMDYYY")
            {
               return XMDYYY.ptxml(Sslbx, Ssbz, Debug);
            }
            //// 南京高淳--不用，改用czf的
            if (sHISName.ToUpper() == "NJSGCRMYY")
            {
                return NJSGCRMYY.ptxml(Sslbx, Ssbz, Debug);
            }
            //// 苏附一
            if (sHISName.ToUpper() == "SFYYY")
            {
                return jsszsfy.ptxml(Sslbx, Ssbz, Debug);
            }
            //// 青海大学附属医院
            if (sHISName.ToUpper() == "QHDXFSYY")
            {
                return qhdxfsyy.ptxml(Sslbx, Ssbz, Debug);
            }
            //// 厦门中山医院 mq医惠,客户端安装MQ
            if (sHISName.ToUpper() == "XMZSYY")
            {
                return xmzsyy.ptxml(Sslbx, Ssbz);
            }
            //// 厦门中山医院 mq医惠，服务器安装MQ，解决客户端安装MQ，初次提取信息时缓慢的问题
            if (sHISName.ToUpper() == "XMSZSYY")
            {
                return xmszsyy.ptxml(Sslbx, Ssbz);
            }
            else
            {
                MessageBox.Show(sHISName+",无此医院参数！", "PATHGETHISZGQ");
                return "0";
            }
        }
        /// <summary>
   
        /// </summary>
        /// <param name="sHISName"></param>
        /// <param name="yzid"></param>
        /// <param name="brlb"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public string LGGetHtml(string sHISName, string yzid, string brlb,string by)
        {
         
            MessageBox.Show("无此医院参数！","PATHGETHISZGQ");
            return "0";
 
        }

        public string tyjk(string hslx, string csxml)
        {
           IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
           string  yymczgq = f.ReadString("SF", "yymczgq", "").Replace("\0", "");
           
            if (hslx.ToUpper() == "SF")
           {
                //湘雅一院收费接口
               if (yymczgq == "xyfyyy")
               {
                   frm_sfjk sf = new frm_sfjk(csxml, "1");
                   sf.ShowDialog();
                   return sf.F_sfje.ToString();
               }
               return "0";
           }
           else
           {
               return "0";
           }

        }

        public string ASPGetXX(string zyh)
        {
            return "1234567890";
        }
        
    }
}
