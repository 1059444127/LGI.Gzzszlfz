using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using netcapscertapphlpLib;
using NetcaPkiLib;
namespace NETCA
{
    public class NETCAPKI
    {
        private COMNCertAppHelper oCertHlp = new COMNCertAppHelper();
        /// <summary>
        /// 产生随机数
        /// </summary>
        /// <param name="len">随机数的长度</param>
        /// <returns>随机数</returns>
        public string GenRandom(short len)
        {
            string rand = oCertHlp.GenRandom(len);
            return rand;
        }
        
        /// <summary>
        /// 弹出证书选择框，用户选择证书，用选择的证书进行P7签名
        /// </summary>
        /// <param name="content">签名原文</param>
        /// <param name="isHavCont">签名值是否包括原文，true包括原文，false不包括原文</param>
        /// <returns>base64编码的签名值</returns>
        public string SignPKCS7(string content, bool isHavCont)
        {
            string filter = "InValidity='True'&&(IssuerCN~'NETCA')";
            int oCert = oCertHlp.SelectMyCert(1, filter, 1, 1);
            if (oCert == 0)
            {
                throw new Exception("没有选择符合条件的证书！");
            }
            object oContent = oCertHlp.StrDecode("UTF-8", content);
            object oSignVal = oCertHlp.SignPKCS7(oCert, oContent, isHavCont, "", "");
            string b64SignVal = oCertHlp.StrEncode("Base64", oSignVal);
            return b64SignVal;
        }
        /// <summary>
        /// 验证P7的签名
        /// </summary>
        /// <param name="signVal">base64编码的签名值</param>
        /// <param name="content">签名原文</param>
        /// <returns>验证成功返回签名证书</returns>
        public int VerifyPKCS7(string signVal, string content)
        {
            object oContent = null;
            int oSignCert = 0;
            object oSignVal = null;
            if (!String.IsNullOrEmpty(content))
            {
                oContent = oCertHlp.StrDecode("UTF-8", content);
            }
            try
            {
                oSignVal = oCertHlp.StrDecode("Base64", signVal);
                oSignCert = oCertHlp.SimpleVerifyPKCS7(oSignVal, oContent);
            }
            catch(Exception ex)
            {
                throw new Exception("签名验证失败！");
            }
            return oSignCert;
        }
        /// <summary>
        /// 带时间戳的数据签名
        /// </summary>
        /// <param name="content">签名原文</param>
        /// <param name="isHavCont">签名值是否包括原文，true为包括原文，false为不包括原文</param>
        /// <param name="tasUrl">时间戳URL</param>
        /// <returns>签名值</returns>
        public string SignPKCS7WithTSA(string content, bool isHavCont, string tasUrl)
        {
            string filter = "(IssuerCN~'NETCA')";
            int oCert = oCertHlp.SelectMyCert(1, filter, 1, 1);
            if (oCert == 0)
            {
                throw new Exception("没有选择符合条件的证书！");
            }
            object oContent = oCertHlp.StrDecode("UTF-8", content);
            object oSignVal = oCertHlp.SignPKCS7(oCert, oContent, isHavCont, tasUrl, "");
            string b64SignVal = oCertHlp.StrEncode("Base64", oSignVal);
            return b64SignVal;
        }

        public string GetInfoFromSignedData(string signVal, string content, int type)
        {
            string val = oCertHlp.GetInfoFromSignedDataByP7(signVal, content, type);
            return val;
        }

        /// <summary>
        /// 连接NETCA网关验证证书有效性
        /// </summary>
        /// <param name="url">NETCA网关地址</param>
        /// <param name="b64Cert">NETCA网关服务器证书，base64编码字符串</param>
        /// <param name="gwType">NETCA网关类型，0为新型网关，1为旧型网关</param>
        /// <param name="cert">需要验证的证书，证书对象类型</param>
        /// <returns>证书有效返回true，失败抛异常信息</returns>
        public bool VerifyCert(string url, string b64Cert, int gwType, int cert)
        {
            COMNCertVerifier oVerify = oCertHlp.CreateCVSCertVerifier(url, b64Cert, gwType);
            int status = oVerify.VerifyCert(cert, "", 3, "", "");
            if (status == 0)
            {
                return true;
            }
            else
            {
                string errMsg = ErrMsg(status);
                throw new Exception(errMsg, null);
            }
        }
        /// <summary>
        /// 获取证书扩展值
        /// </summary>
        /// <param name="cert">证书对象</param>
        /// <param name="oid">证书oid值</param>
        /// <returns>证书扩展值</returns>
        public string GetCertExtensionStringValue(int cert, string oid)
        {
            return oCertHlp.GetCertExtensionStringValue(cert, oid);
        }
        /// <summary>
        /// 获取证书信息
        /// </summary>
        /// <param name="cert">证书对象</param>
        /// <param name="type">证书参数项</param>
        /// <returns>证书信息</returns>
        public string GetCertInfo(int cert, int type)
        {
            return oCertHlp.GetCertInfo(cert, type);
        }

        /// <summary>
        /// NETCA网关返回的错误码转成相应的信息
        /// </summary>
        /// <param name="status">NETCA网关返回的状态码</param>
        /// <returns>错误信息</returns>
        private string ErrMsg(int status)
        {
            string msg = "";
            switch (status)
            {
                case 1:
                    msg = "验证处理失败";
                    break;
                case 2:
                    msg = "证书格式有误";
                    break;
                case 3:
                    msg = "证书不在有效期内";
                    break;
                case 4:
                    msg = "密钥用途不合";
                    break;
                case 5:
                    msg = "证书名字不合";
                    break;
                case 6:
                    msg = "证书策略不合";
                    break;
                case 7:
                    msg = "证书扩展不合";
                    break;
                case 8:
                    msg = "证书链验证失败";
                    break;
                case 9:
                    msg = "证书被注销";
                    break;
                case 10:
                    msg = "注销状态不能确定";
                    break;
                case 11:
                    msg = "证书未注册";
                    break;
                case 12:
                    msg = "证书被暂时锁定/未激活";
                    break;
                default:
                    break;
            }
            return msg;
        }
        public static byte[] base64Decode(string sData)
        {
            Utilities oUtilities = new Utilities();
            return (byte[])oUtilities.Base64Decode(sData, Constants.NETCAPKI_BASE64_ENCODE_NO_NL);
        }
    }
}
