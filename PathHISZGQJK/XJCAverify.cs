using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;//这是用到DllImport时候要引入的包


namespace WindowsApplication2
{
   public static class XJCAverify
    {
       //--------------- 新疆一附院Ca测试--------------
       /// <summary>
       /// 通过CN与webservice进行认证√
       /// </summary>
        /// <param name="wsip">webservice的地址</param>
        /// <param name="appid">系统的应用ID</param>
        /// <param name="cn">证书的CN项</param>
       /// <returns></returns>
        /// [DllImport("affactingDll.dll", CharSet = CharSet.Ansi)] 
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_CertAuth", CharSet = CharSet.Ansi, SetLastError = true)]

       public static extern IntPtr XJCA_CertAuth(string wsip, string appid, string cn);
       


       /// <summary>
       /// 获取SN项√
       /// </summary>
       /// <param name="dwKeyType">dwKeyType证书类型</param> 
       /// <param name="szSerial">证书序列号</param>
       /// <param name="pdwSerialLen">缓冲区长度，返回序列号长度</param>
       /// <returns></returns>
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_GetCertSN", CharSet = CharSet.Ansi, SetLastError = false)]
       public static extern bool XJCA_GetCertSN(uint dwKeyType, StringBuilder szSerial, ref int pdwSerialLen);

       /// <summary>
       /// 修改密码√
       /// </summary>
       /// <param name="szOldPin">原密码</param>
       /// <param name="nOldPinLen">原密码长度</param>
       /// <param name="szNewPin">新密码</param>
       /// <param name="nNewPinLen">新密码长度</param>
       /// <param name="pnRetryTimes">允许输错的次数</param>
       /// <returns></returns>
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_ModifyPin", CharSet = CharSet.Ansi, SetLastError = false)]
       public static extern bool XJCA_ModifyPin( string szOldPin, int nOldPinLen,string szNewPin, int nNewPinLen,int pnRetryTimes);
       /// <summary>
       /// 验证密码√
       /// </summary>
       /// <param name="pszUserPin">密码</param>
       /// <param name="nszUserPinLen">密码长度</param>
       /// <param name="pnRetryTimes">允许输错的次数</param>
       /// <returns></returns>
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_VerifyPin", CharSet = CharSet.Ansi, SetLastError = false)]
       public static extern bool XJCA_VerifyPin(string pszUserPin,int nszUserPinLen,int pnRetryTimes);

       /// <summary>
       /// 判断Ukey是否插入√
       /// </summary>
       /// <returns></returns>
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_KeyInserted", CharSet = CharSet.Ansi, SetLastError = false)]
       public static extern bool XJCA_KeyInserted();


       /// <summary>
       /// 判断驱动是否安装√
       /// </summary>
       /// <returns></returns>
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_CspInstalled", CharSet = CharSet.Ansi, SetLastError = false)]
       public static extern bool XJCA_CspInstalled();
       
       
       /// <summary>
       ///证书复位 √
       /// </summary>
       /// <returns></returns>
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_ResetDevice", CharSet = CharSet.Ansi, SetLastError = false)]
       public static extern bool XJCA_ResetDevice();

       /// <summary>
       /// 时间戳√
       /// </summary>
       /// <param name="ip">时间戳服务的ip地址</param>
       /// <param name="djid">应用单据ID</param>
       /// <param name="hash">hash摘要</param>
       /// <returns></returns>
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_TimeSign", CharSet = CharSet.Ansi, SetLastError = false)]
       public static extern string XJCA_TimeSign(string ip,string djid, string hash);

       /// <summary>
       /// 时间戳验戳√
       /// </summary>
       /// <param name="ip">时间戳服务的ip地址</param>
       /// <param name="xml">cn字符串</param>
       /// <returns></returns>
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_TimeVerify", CharSet = CharSet.Ansi, SetLastError = false)]
       public static extern string XJCA_TimeVerify(string ip, string xml);

       /// <summary>
       /// 签章×
       /// </summary>
       /// <param name="src">明文文件</param>
       /// <param name="signxml">签后失败返回空</param>
       /// <returns></returns>
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_SignSeal", CharSet = CharSet.Ansi, SetLastError = false)]
       public static extern bool XJCA_SignSeal(string src, StringBuilder signxml,ref Int32 len);

       /// <summary>
       /// 验证签章是否有效×
       /// </summary>
       /// <param name="src">进行签章的源数据</param>
       /// <param name="signxml">签章后的xml格式数据</param>
       /// <returns></returns>
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_VerifySeal", CharSet = CharSet.Auto, SetLastError = false)]
       public static extern bool XJCA_VerifySeal(string src,StringBuilder signxml);

       /// <summary>
       /// 取图×
       /// </summary>
       /// <param name="type">属性名称“BMPH“：印章图片宽 （字符需要转化成int）“BMPW“：印章图片宽（字符需要转化成int）      “BMPV“：印章图片值 （base64后的图片buffer）   “CERT“:证书 （base64后的证书buffer）“SIGNNAME“  :印章名称 （base64后的名字）“SIGNUSER“ ： 印章所有者（base64后的名字</param>     
       /// <param name="signxml">签章后xml数据</param>
       /// <param name="orc">被签名的数据，获取图片的时候需要一个验证过程，如果没通过就是有画线的图</param>
       /// <returns></returns>
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_GetSealBMP", CharSet = CharSet.Ansi, SetLastError = false)]
       public static extern bool XJCA_GetSealBMP(StringBuilder xml,string  sign, string orc);
       
       
       
       /// <summary>
       /// 	获取签章图片
       /// </summary>
       /// <param name="ucData">条码数据</param>
       /// <param name="nDataLen">条码数据长度(注:是字节数) 长度<=1K</param>
       /// <param name="szBmpFileName">生成条码的图片文件名</param>
       /// <param name="nClumn">条码列数，取值范围1-30，根据具体数据字符串长度</param>
       /// <param name="nErr">纠错等级，取值范围0-7，建议取2-3；</param>
       /// <param name="nHLRatio">条码单位长度和宽度比例，取值范围1-5</param>
       /// <param name="nHeight">生成BMP图片高度,若为0则自动生成最合适的高度，否则按指定高度，保持长宽比例缩放生成的二维条码图片以上参数的设置将直接影响条码的可读性和条码图片的大小</param>
       /// <returns></returns>
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_MakePdf417ToFile", CharSet = CharSet.Ansi, SetLastError = false)]
       public static extern void XJCA_MakePdf417ToFile(StringBuilder ucData, int nDataLen, string szBmpFileName, int nClumn, int nErr, int nHLRatio, int nHeight);

       /// <summary>
       /// 	获取证书dn值√
       /// </summary>
       /// <param name="pszOwner">默认容器签名证书的拥有者信息字符串 返回pszOwner 需要分配的空间大小</param>
       /// <param name="pnszOwnerLen">分配大小</param>
       /// <returns></returns>
       [DllImport("XJCA_HOS.dll", EntryPoint = "XJCA_GetCertDN", CharSet = CharSet.Ansi, SetLastError = false)]
       public static extern bool XJCA_GetCertDN(StringBuilder pszOwner, ref int pnszOwnerLen);


      

       


    }
}
