using System;
using System.Data;
using System.Configuration;

using System.Collections;
using System.Text;
using SecuInter;
using System.Web;
using System.IO;
namespace NETCA
{
    /// <summary>NETCA PKI 中间件接口；
    /// 
    /// secuinter.dll v4.1.0.1请保障注册的是这个版本；
    /// 
    /// 需添加secuinter.dll的引用，本类即可使用；
    /// 
    /// 用户可以修改该文件，已达到自己需求，但须保障功能正确性；
    /// </summary>
    public class NETCAPKIv4
    {
        public NETCAPKIv4()
        {

            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 5.1	常量定义
        public const SECUINTER_STORE_LOCATION SECUINTER_LOCAL_MACHINE_STORE = SECUINTER_STORE_LOCATION.SECUINTER_LOCAL_MACHINE_STORE;
        public const SECUINTER_STORE_LOCATION SECUINTER_CURRENT_USER_STORE = SECUINTER_STORE_LOCATION.SECUINTER_CURRENT_USER_STORE;

        public const SECUINTER_STORE_NAME SECUINTER_MY_STORE = SECUINTER_STORE_NAME.SECUINTER_MY_STORE;
        public const SECUINTER_STORE_NAME SECUINTER_OTHER_STORE = SECUINTER_STORE_NAME.SECUINTER_OTHER_STORE;
        public const SECUINTER_STORE_NAME SECUINTER_CA_STORE = SECUINTER_STORE_NAME.SECUINTER_CA_STORE;
        public const SECUINTER_STORE_NAME SECUINTER_ROOT_STORE = SECUINTER_STORE_NAME.SECUINTER_ROOT_STORE;

        public const int SECUINTER_CERTTYPE_ALL = 0;
        public const int SECUINTER_CERTTYPE_SIGN = 1;
        public const int SECUINTER_CERTTYPE_ENV = 2;

        public const int SECUINTER_NETCA_ALL = 0;
        public const int SECUINTER_NETCA_YES = 1;
        public const int SECUINTER_NETCA_NO = 2;//非网证通证书
        /*NETCA和其他CA*/
        public const int SECUINTER_NETCA_OTHER = 3;//网证通证书+其他CA证书
        public static String[] CASTR = new String[3] { "CN=NETCA", "CN=GDCA", "CN=WGCA" };//根据实际情况修订
        public static String[] CERTVALUEPARSE = new String[61] {
            "证书PEM编码","证书姆印", "证书序列号", "证书主题","证书颁发者主题", "证书有效期起",
            "证书有效期止", "密钥用法", "","用户证书绑定值", "旧的用户证书绑定值",//0~9
            "证书主题名称", "Subject的人名(CN)", "Subject中的单位(O)","Subject中的地址项(L)", "Subject中的Email(E)",
            "Subject中的部门名(OU)", "国家名(C)", "省州名(S)","", "",//10~19
            "CA ID", "证书类型", "证书唯一标识","", "",
            "", "", "","", "",//20~29
            "NETCA:旧姆印信息", "纳税人编码", "企业法人代码","税务登记号", "证书来源地",
            "证件号码信息", "", "","", "",//30~39
            "", "", "","", "",
            "", "", "","", "",//40~49
            "GDCA:信任号TrustID", "", "","", "",
            "", "", "","", "",//50~59
        };//根据实际情况修订



        public const int SECUINTER_SHA1_ALGORITHM = 1;
        public const int SECUINTER_ALGORITHM_RC2 = 0;
        public const int SECUINTER_ALGORITHM_DES = 6;
        public const int SECUINTER_SHA1WithRSA_ALGORITHM = 2;

        #endregion

        #region 5.2	证书类

        /// <summary>5.2.1 获取证书集 2011-12-19
        /// </summary>
        /// <param name="StoreLocation">SECUINTER_LOCAL_MACHINE_STORE = 0;SECUINTER_CURRENT_USER_STORE= 1;</param>
        /// <param name="StoreName">SECUINTER_MY_STORE=0(个人);SECUINTER_OTHER_STORE:1(其他人);SECUINTER_CA_STORE= 2;SECUINTER_ROOT_STORE= 3;</param>
        /// <param name="certType">SECUINTER_CERTTYPE_ALL= 0;SECUINTER_CERTTYPE_SIGN= 1;SECUINTER_CERTTYPE_ENV= 2;</param>
        /// <param name="netcaType">SECUINTER_NETCA_ALL= 0;SECUINTER_NETCA_YES= 1;SECUINTER_NETCA_NO= 2;SECUINTER_NETCA_OTHER=3</param>
        /// <returns></returns>
        public static SecuInter.X509Certificates getX509Certificates(
            SECUINTER_STORE_LOCATION StoreLocation, SECUINTER_STORE_NAME StoreName, int certType, int netcaType)
        {
            SecuInter.Store oMyStore = new SecuInter.Store();
            SecuInter.X509Certificates oMyCerts = new SecuInter.X509Certificates();
            SecuInter.Utilities oUtil = new Utilities();
            SecuInter.Store oMyStore2 = oUtil.CreateStoreObject();

            try
            {
                oMyStore.Open(StoreLocation, StoreName);
            }
            catch (Exception)
            {
                throw new Exception("打开证书库失败");
            }
            SecuInter.X509Certificates certs = (SecuInter.X509Certificates)oMyStore.X509Certificates;
            oMyStore.Close();
            oMyStore = null;


            IEnumerator oEnum = certs.GetEnumerator();
            while (oEnum.MoveNext())
            {
                SecuInter.X509Certificate oCert = (SecuInter.X509Certificate)oEnum.Current;

                String issuer = oCert.get_Issuer(SECUINTER_NAMESTRING_TYPE.SECUINTER_X500_NAMESTRING);
                if (certType == SECUINTER_CERTTYPE_ALL)
                {

                    if (netcaType == SECUINTER_NETCA_ALL)
                    {
                        oMyCerts.Add(oCert);
                    }
                    else if (netcaType == SECUINTER_NETCA_YES)
                    {
                        if (issuer.IndexOf("CN=NETCA") >= 0)
                        {
                            oMyCerts.Add(oCert);
                        }
                    }
                    else if (netcaType == SECUINTER_NETCA_NO)
                    {
                        if (issuer.IndexOf("CN=NETCA") < 0)
                        {
                            oMyCerts.Add(oCert);
                        }
                    }
                    //限制可以使用NETCA证书和其他CA证书
                    else if (netcaType == SECUINTER_NETCA_OTHER)
                    {
                        for (int j = 0; j < CASTR.Length; j++)
                        {
                            if (issuer.IndexOf(CASTR[j]) >= 0)
                            {
                                oMyCerts.Add(oCert);
                            }
                        }
                    }
                }
                else if (certType == SECUINTER_CERTTYPE_SIGN)
                {

                    if (netcaType == SECUINTER_NETCA_ALL)
                    {
                        if (oCert.KeyUsage == 3)
                        {
                            oMyCerts.Add(oCert);
                        }
                        if (oCert.KeyUsage == -1)
                        {
                            oMyCerts.Add(oCert);
                        }

                    }
                    else if (netcaType == SECUINTER_NETCA_YES)
                    {
                        if (issuer.IndexOf("CN=NETCA") >= 0)
                        {
                            if (oCert.KeyUsage == 3)
                            {
                                oMyCerts.Add(oCert);
                            }
                            if (oCert.KeyUsage == -1)
                            {
                                oMyCerts.Add(oCert);
                            }
                        }
                    }
                    else if (netcaType == SECUINTER_NETCA_NO)
                    {
                        if (issuer.IndexOf("CN=NETCA") < 0)
                        {
                            if (oCert.KeyUsage == 3)
                            {
                                oMyCerts.Add(oCert);
                            }
                            if (oCert.KeyUsage == -1)
                            {
                                oMyCerts.Add(oCert);
                            }
                        }
                    }

                    //限制可以使用NETCA证书和其他CA证书
                    else if (netcaType == SECUINTER_NETCA_OTHER)
                    {
                        for (int j = 0; j < CASTR.Length; j++)
                        {
                            if (issuer.IndexOf(CASTR[j]) >= 0)
                            {
                                if (oCert.KeyUsage == 3)
                                {
                                    oMyCerts.Add(oCert);
                                }
                                if (oCert.KeyUsage == -1)
                                {
                                    oMyCerts.Add(oCert);
                                }
                            }
                        }

                    }

                }
                else if (certType == SECUINTER_CERTTYPE_ENV)
                {

                    if (netcaType == SECUINTER_NETCA_ALL)
                    {
                        if (oCert.KeyUsage == 12)
                        {
                            oMyCerts.Add(oCert);
                        }
                        if (oCert.KeyUsage == -1)
                        {
                            oMyCerts.Add(oCert);
                        }

                    }
                    else if (netcaType == SECUINTER_NETCA_YES)
                    {
                        if (issuer.IndexOf("CN=NETCA") >= 0)
                        {
                            if (oCert.KeyUsage == 12)
                            {
                                oMyCerts.Add(oCert);
                            }
                            if (oCert.KeyUsage == -1)
                            {
                                oMyCerts.Add(oCert);
                            }
                        }
                    }
                    else if (netcaType == SECUINTER_NETCA_NO)
                    {
                        if (issuer.IndexOf("CN=NETCA") < 0)
                        {
                            if (oCert.KeyUsage == 12)
                            {
                                oMyCerts.Add(oCert);
                            }
                            if (oCert.KeyUsage == -1)
                            {
                                oMyCerts.Add(oCert);
                            }
                        }
                    }

                    //限制可以使用NETCA证书和其他CA证书
                    else if (netcaType == SECUINTER_NETCA_OTHER)
                    {
                        for (int j = 0; j < CASTR.Length; j++)
                        {
                            if (issuer.IndexOf(CASTR[j]) >= 0)
                            {
                                if (oCert.KeyUsage == 12)
                                {
                                    oMyCerts.Add(oCert);
                                }
                                if (oCert.KeyUsage == -1)
                                {
                                    oMyCerts.Add(oCert);
                                }
                            }
                        }
                    }
                }
            }//END FOR
            return oMyCerts;

        }

        /// <summary>5.2.2 获取证书对象 2011-12-19
        /// 
        /// </summary>
        /// <param name="StoreLocation"></param>
        /// <param name="StoreName"></param>
        /// <param name="certType"></param>
        /// <param name="netcaType"></param>
        /// <returns></returns>
        public static SecuInter.X509Certificate getX509Certificate(
            SECUINTER_STORE_LOCATION StoreLocation, SECUINTER_STORE_NAME StoreName, int certType, int netcaType)
        {
            SecuInter.X509Certificates oMyCerts = getX509Certificates(StoreLocation, StoreName, certType, netcaType);
            if (oMyCerts == null)
            {
                return null;
            }
            if (oMyCerts.Count > 0)
            {
                return (SecuInter.X509Certificate)oMyCerts.SelectCertificate();
            }
            return null;

        }

        /// <summary>5.2.3 根据证书字符串获取证书
        /// 
        /// </summary>
        /// <param name="sX509Certificate"></param>
        /// <returns></returns>
        public static SecuInter.X509Certificate getX509Certificate(string sX509Certificate)
        {
            SecuInter.X509Certificate oX509Certificate = new SecuInter.X509Certificate();
            oX509Certificate.Decode(sX509Certificate);
            return oX509Certificate;
        }

        /// <summary>5.2.4	根据特定域的值，获取证书对象 2011-12-19
        /// 
        /// </summary>
        /// <param name="StoreLocation"></param>
        /// <param name="StoreName"></param>
        /// <param name="certType"></param>
        /// <param name="netcaType"></param>
        /// <returns></returns>
        public static SecuInter.X509Certificate getX509Certificate(
            SECUINTER_STORE_LOCATION StoreLocation, SECUINTER_STORE_NAME StoreName, int certType, int netcaType,
            int iValueType, String certValue)
        {
            SecuInter.X509Certificates oMyCerts = getX509Certificates(StoreLocation, StoreName, certType, netcaType);
            if (oMyCerts == null)
            {
                return null;
            }
            if (oMyCerts.Count > 0)
            {
                IEnumerator oEnum = oMyCerts.GetEnumerator();
                while (oEnum.MoveNext())
                {
                    SecuInter.X509Certificate oCert = (SecuInter.X509Certificate)oEnum.Current;
                    if (getX509CertificateInfo(oCert, iValueType).Equals(certValue))
                    {
                        return oCert;
                    }
                }
            }
            return null;
        }

        /// <summary>5.2.5	获取证书信息*** 2012-10-29 Update
        /// 
        /// </summary>
        /// <param name="oCert"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String getX509CertificateInfo(SecuInter.X509Certificate oCert, int type)
        {
            if (oCert == null)
            {
                throw new Exception("证书为空!");
            }
            if (type == 0)//获取证书BASE64格式编码字符串 2012-12-03 modify luhanmin
            {
                String certHeader = "-----BEGIN CERTIFICATE-----\r\n";
                String certEnd = "-----END CERTIFICATE-----\r\n";
                String certPem = oCert.get_Encoded(SECUINTER_CERT_ENCODE_TYPE.SECUINTER_CERT_ENCODE_PEM).ToString();
                if (certPem.IndexOf(certHeader) >= 0)
                {
                    certPem = certPem.Substring(certHeader.Length, certPem.Length - certHeader.Length);
                    certPem = certPem.Substring(0, certPem.Length - certEnd.Length);
                }
                return certPem;
            }
            if (type == 1)//证书姆印
            {
                SecuInter.Utilities oUtil = new SecuInter.Utilities();
                return oUtil.BinaryToHex(oCert.get_Thumbprint(SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM)).ToUpper();
            }
            else if (type == 2)//证书序列号
            {
                return oCert.SerialNumber;
            }
            else if (type == 3)//证书Subject
            {
                return oCert.get_Subject(SECUINTER_NAMESTRING_TYPE.SECUINTER_X500_NAMESTRING);

            }
            else if (type == 4)//证书颁发者Subject
            {
                return oCert.get_Issuer(SECUINTER_NAMESTRING_TYPE.SECUINTER_X500_NAMESTRING);

            }
            else if (type == 5)//证书有效期起
            {
                return oCert.ValidFromDate.ToString();

            }
            else if (type == 6)//证书有效期止
            {
                return oCert.ValidToDate.ToString();

            }
            else if (type == 7)//KeyUsage 密钥用法
            {
                return oCert.KeyUsage.ToString();
            }
            else if (type == 9)//UsrCertNO：证书绑定值；(系统改造时,建议采用该值)
            {
                if (getX509CertificateInfo(oCert, 21).Equals("1"))
                {
                    String rt = getX509CertificateInfo(oCert, 23);//取证书唯一标识
                    if (String.IsNullOrEmpty(rt))
                    {
                        rt = getX509CertificateInfo(oCert, 36);//取证书证件号码扩展域信息
                        if (String.IsNullOrEmpty(rt))
                        {
                            rt = getX509CertificateInfo(oCert, 1);//取证书姆印
                        }
                    }
                    return rt;
                }
                if (getX509CertificateInfo(oCert, 21).Equals("2"))
                {
                    return getX509CertificateInfo(oCert, 51);
                }
            }
            else if (type == 10)//OldUsrCertNo：旧的用户证书绑定值；(证书更新后的原有9的取值)
            {
                if (getX509CertificateInfo(oCert, 21).Equals("1"))
                {
                    String rt = getX509CertificateInfo(oCert, 23);//取证书唯一标识
                    if (String.IsNullOrEmpty(rt))
                    {
                        rt = getX509CertificateInfo(oCert, 36);//取证书证件号码扩展域信息
                        if (String.IsNullOrEmpty(rt))
                        {
                            rt = getX509CertificateInfo(oCert, 31);//取证书旧姆印
                        }
                    }
                    return rt;
                }
                if (getX509CertificateInfo(oCert, 21).Equals("2"))
                {
                    return getX509CertificateInfo(oCert, 51);
                }
            }
            else if (type == 11)//证书主题名称；有CN项取CN项值；无CN项，取O的值
            {
                if (String.IsNullOrEmpty(getX509CertificateInfo(oCert, 12)))
                {
                    return getX509CertificateInfo(oCert, 13);
                }
                else
                {
                    return getX509CertificateInfo(oCert, 12);
                }
            }
            else if (type == 12)//Subject中的CN项（人名）
            {
                String subject = getX509CertificateInfo(oCert, 3);
                return parseDN(subject, "CN");
                //return oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_SUBJECT_SIMPLE_NAME);
            }
            else if (type == 13)//Subject中的O项（人名）
            {
                String subject = getX509CertificateInfo(oCert, 3);
                return parseDN(subject, "O");

            }
            else if (type == 14)//Subject中的地址（L项）
            {
                String subject = getX509CertificateInfo(oCert, 3);
                return parseDN(subject, "L");
            }
            else if (type == 15)//证书颁发者的Email
            {
                return oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_SUBJECT_EMAIL);
            }
            else if (type == 16)//Subject中的部门名（OU项）
            {
                String subject = getX509CertificateInfo(oCert, 3);
                return parseDN(subject, "OU");
            }
            else if (type == 17)//用户国家名（C项）
            {
                String subject = getX509CertificateInfo(oCert, 3);
                // oCert.GetUTF8ExtValue(
                return parseDN(subject, "C");
            }
            else if (type == 18)//用户省州名（S项）
            {
                String subject = getX509CertificateInfo(oCert, 3);
                return parseDN(subject, "S");
            }

            else if (type == 21)//CA ID
            {
                for (int i = 0; i < CASTR.Length; i++)
                {
                    if (getX509CertificateInfo(oCert, 4).IndexOf(CASTR[i]) > 0)
                    {
                        return "" + (i + 1);
                    }
                }
                return "0";
            }
            else if (type == 22)//证书类型
            {

                return "0";
            }
            else if (type == 23)//证书唯一标识(一般为客户号等)
            {
                if (getX509CertificateInfo(oCert, 21).Equals("1"))
                {
                    return "";
                }
                if (getX509CertificateInfo(oCert, 21).Equals("2"))
                {
                    return getX509CertificateInfo(oCert, 51);
                }
            }
            else if (type == 31)//证书旧姆印
            {
                try
                {
                    SecuInter.Utilities oUtil = new SecuInter.Utilities();
                    return oUtil.BinaryToHex(oCert.get_PrevCertThumbprint(SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM)).ToUpper();
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (type == 32)//纳税人编码
            {
                try
                {
                    return oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_TAXPAYERID);
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (type == 33)//组织机构代码号
            {
                try
                {
                    return oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_ORGANIZATIONCODE);
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (type == 34)//税务登记号
            {
                try
                {
                    return oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_TAXATIONNUMBER);
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (type == 35)//证书来源地
            {
                try
                {
                    return oCert.GetInfo(SECUINTER_CERT_INFO_TYPE.SECUINTER_CERT_INFO_CERTSOURCE);
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (type == 36)//证书证件号码扩展域
            {
                try
                {
                    //注意选择不同项目
                    //第1个表达式为 NETCA通用定义OID
                    //第1个表达式为 深圳项目中采用（3家CA都采用此做唯一标识）   2.16.156.112548
                    String rt = oCert.GetUTF8ExtValue("1.3.6.1.4.1.18760.1.12.11");
                    //String rt = oCert.GetUTF8ExtValue("2.16.156.112548");
                    return rt;
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else if (type == 51)//GDCA 证书信任号
            {
                try
                {
                    return "GDCA 未实现";
                    //return oCert.GetUTF8ExtValue("1.2.156.0.2.1");
                }
                catch (Exception)
                {
                    return "";
                }
            }
            return "";
        }

        /// <summary>5.2.5.1 获取证书姆印 2011-12-19
        /// 
        /// </summary>
        /// <param name="oCert"></param>
        /// <returns></returns>
        public static String getX509CertificateThumbprint(SecuInter.X509Certificate oCert)
        {
            if (oCert == null)
            {
                throw new Exception("证书为空!");
            }
            SecuInter.Utilities oUtil = new SecuInter.Utilities();
            return oUtil.BinaryToHex(oCert.get_Thumbprint(SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM)).ToUpper();
        }

        /// <summary>5.2.6	获取证书特定扩展域信息
        /// 
        /// </summary>
        /// <param name="oCert"></param>
        /// <param name="OID"></param>
        /// <returns>UTF8编码</returns>
        public static String getX509CertificateInfo(SecuInter.X509Certificate oCert, String OID)
        {
            return oCert.GetUTF8ExtValue(OID);
        }

        /// <summary>5.2.7	从HTTPS通信中获取证书对象（SSL用）
        /// HttpClientCertificate hCert = Request.ClientCertificate;
        /// byte[] bCert=hCert.Certificate;
        /// </summary>
        /// <param name="hCert"></param>
        /// <returns></returns>
        public static SecuInter.X509Certificate getX509Certificate(byte[] bCert)
        {
            SecuInter.X509Certificate oCert = new SecuInter.X509Certificate();
            try
            {
                oCert.Decode(bCert);
                return oCert;
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>5.2.8	获取服务器证书
        /// 注意：服务器端使用
        /// 
        /// </summary>
        /// <param name="subject">证书主题中CN项值</param>
        /// <returns></returns>
        public static String getServerCert(String subject)
        {

            SecuInter.X509Certificate oCert = getX509Certificate(
            NETCAPKIv4.SECUINTER_LOCAL_MACHINE_STORE, NETCAPKIv4.SECUINTER_MY_STORE,
            NETCAPKIv4.SECUINTER_CERTTYPE_ALL,
            NETCAPKIv4.SECUINTER_NETCA_ALL, 11,
            subject);
            return getX509CertificateInfo(oCert, 0);

        }

        #endregion

        #region 5.3	签名类

        /// <summary>5.3.1 带PIN码PKCS7签名(对应以前的sign函数) 2011-12-19 **
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="isNotHasSource"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static String signPKCS7ByPwd(String sSource, Boolean isNotHasSource, String pwd)
        {
            SecuInter.X509Certificate oCert = getX509Certificate(SECUINTER_STORE_LOCATION.SECUINTER_CURRENT_USER_STORE, SECUINTER_STORE_NAME.SECUINTER_MY_STORE, NETCAPKIv4.SECUINTER_CERTTYPE_SIGN, NETCAPKIv4.SECUINTER_NETCA_YES);
            if (oCert == null)
            {
                throw new Exception("未选择证书,请检查是否插入密钥!");
            }
            return signPKCS7ByCertificate(sSource, isNotHasSource, pwd, oCert);
        }

        /// <summary>5.3.2 使用证书进行PKCS7签名 2011-12-19  
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="isNotHasSource"></param>
        /// <param name="pwd"></param>
        /// <param name="oCert"></param>
        /// <returns></returns>
        public static String signPKCS7ByCertificate(String sSource, Boolean isNotHasSource, String pwd, SecuInter.X509Certificate oCert)
        {
            SecuInter.Signer oSigner = new SecuInter.Signer();
            SecuInter.SignedData oSignedData = new SecuInter.SignedData();
            SecuInter.Utilities oUtil = new SecuInter.Utilities();
            if (sSource == "")
            {
                throw new Exception("原文内容为空!");
            }


            oSigner.Certificate = oCert;
            oSigner.HashAlgorithm = SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM;
            oSigner.UseSigningCertificateAttribute = false;
            oSigner.UseSigningTime = false;
            if (!String.IsNullOrEmpty(pwd))
            {
                bool ok = oSigner.SetUserPIN(pwd);
                if (!ok)
                {
                    throw new Exception("密码有误！");

                }
            }
            oSignedData.Content = sSource;
            oSignedData.Detached = isNotHasSource;

            object arrRT = oSignedData.Sign(oSigner, SECUINTER_CMS_ENCODE_TYPE.SECUINTER_CMS_ENCODE_BASE64);
            oSignedData = null;
            oSigner = null;
            return arrRT.ToString();
        }

        /// <summary>5.3.3 PKCS7签名(常用,兼容以前代码) 2011-12-19
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="isNotHasSource"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static String signNETCA(String sSource, Boolean isNotHasSource, String pwd)
        {
            return signPKCS7ByPwd(sSource, isNotHasSource, "");
        }

        /// <summary>5.3.4 PKCS7签名验证并获取证书 2011-12-19
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="sSignature"></param>
        /// <param name="isNotHasSource"></param>
        /// <returns></returns>
        public static SecuInter.X509Certificate verifyPKCS7(String sSource, string sSignature, Boolean isNotHasSource)
        {
            SecuInter.X509Certificate oCertSign = null;
            SignedData signedData = new SignedData();
            Utilities util = new Utilities();
            if (isNotHasSource == true)
            {//不含原文情况,将原文设入签名数据中
                signedData.Content = sSource;
            }

            if (!signedData.Verify(sSignature, SecuInter.SECUINTER_SIGNEDDATA_VERIFY_FLAG.SECUINTER_SIGNEDDATA_VERIFY_SIGNATURE_ONLY))
            {
                throw new Exception("签名验证不正确");
            }
            if (isNotHasSource == false)
            {//含原文情况,比对原文和签名信息,进行验证
                if (!sSource.Equals(util.ByteArraytoString(signedData.Content)))//
                {
                    throw new Exception("发生错误,签名原文不一致!");
                }
            }
            // '判断验证结果与签名时数据是否一致
            SecuInter.Signers signers = signedData.Signers;
            IEnumerator enumer = signers.GetEnumerator();

            while (enumer.MoveNext()) //第一张证书为客户端签名证书
            {
                SecuInter.Signer signer = (SecuInter.Signer)enumer.Current;
                SecuInter.X509Certificate oCert = (SecuInter.X509Certificate)signer.Certificate;

                oCertSign = oCert; //'验证通过，取签名的证书
                break;
            }
            if (oCertSign == null)
            {
                throw new Exception("签名信息中无证书!");
            }
            signedData = null;
            util = null;
            return oCertSign;
        }

        /// <summary>5.3.5 带原文PKCS7签名,验证并获取原文 2011-12-19  
        /// 含原文签名情况下使用
        /// </summary>
        /// <param name="sSignature"></param>
        /// <returns></returns>
        public static String getSourceFromPKCS7SignData(string sSignature)
        {
            String sSource = "";
            SignedData oSignedData = new SignedData();
            Utilities oUtilities = new Utilities();
            if (!oSignedData.Verify(sSignature, SecuInter.SECUINTER_SIGNEDDATA_VERIFY_FLAG.SECUINTER_SIGNEDDATA_VERIFY_SIGNATURE_ONLY))
            {
                throw new Exception("签名验证不正确");
            }
            SecuInter.Signers signers = oSignedData.Signers;
            IEnumerator enumer = signers.GetEnumerator();

            while (enumer.MoveNext()) //第一张证书为客户端签名证书
            {
                SecuInter.Signer signer = (SecuInter.Signer)enumer.Current;
                SecuInter.X509Certificate oCert = (SecuInter.X509Certificate)signer.Certificate;
                oCert.Display();
            }
            sSource = oUtilities.ByteArraytoString(oSignedData.Content);
            oSignedData = null;
            oUtilities = null;
            return sSource;
        }

        /// <summary>5.3.6 PKCS1签名 2011-12-19
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="oCert"></param>
        /// <returns></returns>
        public static String signPKCS1ByCert(String sSource, SecuInter.X509Certificate oCert)
        {
            SecuInter.signature oSignature = new SecuInter.signature();
            SecuInter.Utilities oUtil = new SecuInter.Utilities();


            oSignature.Certificate = oCert;
            oSignature.Algorithm = SECUINTER_SIGNATURE_ALGORITHM.SECUINTER_SHA1WithRSA_ALGORITHM;
            object arrRT = oSignature.Sign(sSource);

            String rt = oUtil.Base64Encode(arrRT);
            oSignature = null;
            oUtil = null;
            return rt;
        }

        /// <summary>5.3.7 PKCS1签名验证 2011-12-19
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="bSignData"></param>
        /// <param name="oCert"></param>
        /// <returns></returns>
        public static Boolean verifyPKCS1(String sSource, String bSignData, SecuInter.X509Certificate oCert)
        {
            Boolean isOK = false;

            if (oCert == null)
            {
                throw new Exception("未选择证书!");
            }
            SecuInter.signature oSignature = new SecuInter.signature();
            SecuInter.Utilities oUtil = new SecuInter.Utilities();

            oSignature.Certificate = oCert;
            oSignature.Algorithm = SECUINTER_SIGNATURE_ALGORITHM.SECUINTER_SHA1WithRSA_ALGORITHM;

            if (oSignature.Verify(sSource, oUtil.Base64Decode(bSignData)))
            {
                isOK = true;
            }
            else
            {
                throw new Exception("验证不通过!");
            }

            oSignature = null;
            oUtil = null;
            return isOK;
        }

        /// <summary>5.3.7	PKCS#1签名验证
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="bSignData"></param>
        /// <param name="sX509Certificate"></param>
        /// <returns></returns>
        public static Boolean verifyPKCS1(String sSource, String bSignData, String sX509Certificate)
        {
            SecuInter.X509Certificate oCert = new SecuInter.X509Certificate();
            oCert.Decode(sX509Certificate);

            Boolean isOK = false;

            if (oCert == null)
            {
                throw new Exception("未选择证书!");
            }
            SecuInter.signature oSignature = new SecuInter.signature();
            SecuInter.Utilities oUtil = new SecuInter.Utilities();

            oSignature.Certificate = oCert;
            oSignature.Algorithm = SECUINTER_SIGNATURE_ALGORITHM.SECUINTER_SHA1WithRSA_ALGORITHM;
            byte[] bContent = Encoding.Default.GetBytes(sSource);
            if (oSignature.Verify(bContent, oUtil.Base64Decode(bSignData)))
            {
                isOK = true;
            }
            else
            {
                throw new Exception("验证不通过!");
            }

            oSignature = null;
            oUtil = null;

            return isOK;
        }

        /// <summary>
        /// PKCS#7时间戳签名
        /// </summary>
        /// <param name="bContent">签名内容</param>
        /// <param name="tsaUrl">时间戳服务器URL</param>
        /// <param name="IsNotHasSource"></param>
        /// <returns>签名值</returns>
        public static String signPKCS7WithTSA(String bContent, String tsaUrl, Boolean IsNotHasSource)
        {
            if (bContent == "")
            {
                throw new Exception("原文内容为空!");

            }
            if (tsaUrl == "")
            {
                throw new Exception("时间戳URL为空!");
            }

            SecuInter.X509Certificate oCert = getX509Certificate(SECUINTER_CURRENT_USER_STORE, SECUINTER_MY_STORE, SECUINTER_CERTTYPE_SIGN, SECUINTER_NETCA_OTHER);
            if (oCert == null)
            {
                throw new Exception("未选择证书!");
            }

            SecuInter.Signer oSigner = new SecuInter.Signer();
            SecuInter.SignedData oSignedData = new SecuInter.SignedData();
            SecuInter.X509Certificate oX509Certificate = new SecuInter.X509Certificate();
            //oX509Certificate = oCert;         
            oSigner.Certificate = oCert;
            oSigner.HashAlgorithm = SecuInter.SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM;
            oSigner.UseSigningCertificateAttribute = false;
            oSigner.UseSigningTime = true;
            oSignedData.Content = bContent;
            oSignedData.Detached = IsNotHasSource;

            Object arrRT = oSignedData.SignWithTSATimeStamp(oSigner, tsaUrl, "", oX509Certificate, SECUINTER_CMS_ENCODE_TYPE.SECUINTER_CMS_ENCODE_BASE64);
            oSignedData = null;
            oSigner = null;
            oCert = null;
            oX509Certificate = null;
            return arrRT.ToString();
        }
        #endregion
        /// <summary>5.3.4 PKCS7签名验证并获取证书 2011-12-19
        /// 
        /// </summary>
        /// <param name="sSource"></param>
        /// <param name="sSignature"></param>
        /// <param name="isNotHasSource"></param>
        /// <returns></returns>
        public static SecuInter.X509Certificate verifyPKCS7(String sSource, string sSignature, Boolean isNotHasSource, ref String signTime)
        {
            SecuInter.X509Certificate oCertSign = null;
            SignedData signedData = new SignedData();
            Utilities util = new Utilities();

            if (isNotHasSource == true)
            {//不含原文情况,将原文设入签名数据中
                signedData.Content = sSource;
            }

            if (!signedData.Verify(sSignature, SecuInter.SECUINTER_SIGNEDDATA_VERIFY_FLAG.SECUINTER_SIGNEDDATA_VERIFY_SIGNATURE_ONLY))
            {
                throw new Exception("签名验证不正确");
            }
            if (isNotHasSource == false)
            {//含原文情况,比对原文和签名信息,进行验证
                if (!sSource.Equals(util.ByteArraytoString(signedData.Content)))//
                {
                    throw new Exception("发生错误,签名原文不一致!");
                }
            }
            int iCertCount = signedData.Signers.Count;
            //获取签名时间
            if (iCertCount == 1)
            {
                if (signedData.HasTSATimestamp(0))
                {
                    signTime = (signedData.getTSATimeStamp(0).ToString("yyyy-MM-dd HH:mm:ss"));
                }
            }
            else
            {
                for (var i = 0; i < iCertCount; i++)
                {
                    signedData.Signers[i].Certificate.Display();
                    if (signedData.HasTSATimestamp(i))
                    {
                        signTime = (signedData.getTSATimeStamp(i).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                }
            }
            // '判断验证结果与签名时数据是否一致
            SecuInter.Signers signers = signedData.Signers;
            IEnumerator enumer = signers.GetEnumerator();

            while (enumer.MoveNext()) //第一张证书为客户端签名证书
            {
                SecuInter.Signer signer = (SecuInter.Signer)enumer.Current;
                SecuInter.X509Certificate oCert = (SecuInter.X509Certificate)signer.Certificate;

                oCertSign = oCert; //'验证通过，取签名的证书
                break;
            }
            if (oCertSign == null)
            {
                throw new Exception("签名信息中无证书!");
            }
            signedData = null;
            util = null;
            return oCertSign;
        }

        #region 5.4	加密类

        /// <summary>5.4.1	PKCS#7加密
        /// 
        /// </summary>
        /// <param name="bSource"></param>
        /// <returns></returns>
        public static String encryptPKCS7(byte[] bSource)
        {
            SecuInter.X509Certificate oCert = getX509Certificate(SECUINTER_STORE_LOCATION.SECUINTER_CURRENT_USER_STORE, SECUINTER_STORE_NAME.SECUINTER_MY_STORE, NETCAPKIv4.SECUINTER_CERTTYPE_ENV, NETCAPKIv4.SECUINTER_NETCA_YES);
            if (oCert == null)
            {
                throw new Exception("未选择证书,请检查是否插入密钥!");
            }
            return encryptPKCS7(bSource, oCert);
        }

        /// <summary>5.4.2	PKCS#7加密
        /// 
        /// </summary>
        /// <param name="bSource"></param>
        /// <returns></returns>
        public static String encryptPKCS7(byte[] bSource, SecuInter.X509Certificate oCert)
        {
            if (oCert == null)
            {
                throw new Exception("未选择证书,请检查是否插入密钥!");
            }
            if (bSource.Length == 0)
            {
                throw new Exception("原文为空!");
            }



            SecuInter.EnvelopedData oEnv = new SecuInter.EnvelopedData();
            oEnv.Algorithm = SECUINTER_ENCRYPT_ALGORITHM.SECUINTER_ALGORITHM_DES;
            oEnv.Recipients.Add(oCert);
            oEnv.Content = bSource;

            object arrRT = oEnv.Encrypt(SECUINTER_CMS_ENCODE_TYPE.SECUINTER_CMS_ENCODE_BASE64);
            oEnv = null;
            return arrRT.ToString();
        }

        /// <summary>5.4.3	PKCS#7解密
        /// 
        /// </summary>
        /// <param name="sSignData"></param>
        /// <returns></returns>
        public static byte[] decryptPKCS7(String sSignData)
        {
            try
            {
                Utilities oUtilities = new Utilities();
                byte[] bSignData = (byte[])oUtilities.Base64Decode(sSignData);
                SecuInter.EnvelopedData oEnv = new SecuInter.EnvelopedData();
                oEnv.Decrypt(bSignData);
                byte[] rt = (byte[])oEnv.Content;
                //IEnumerator oenum=oEnv.Recipients.GetEnumerator();
                //((SecuInter.X509Certificate)oenum.Current).Display();
                oEnv = null;
                return rt;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region 5.5	工具类

        /// <summary>5.5.1	Base64编码
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String base64Encode(byte[] str)
        {
            Utilities oUtilities = new Utilities();

            return oUtilities.Base64Encode(str);
        }

        /// <summary>5.5.2	Base64解码
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] base64Decode(String str)
        {
            Utilities oUtilities = new Utilities();
            return (byte[])oUtilities.Base64Decode(str);

        }

        /// <summary>5.5.3 工具：获取随机数
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static String getRandom(int length)
        {
            try
            {
                Utilities oUtilities = new Utilities();
                byte[] bSignData = (byte[])oUtilities.GetRandom(length);
                return oUtilities.Base64Encode(bSignData);
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>5.5.4	获取信息摘要码（HASH码，sha1)
        /// 2013-04-02 修订，解决传入字符串编码的问题
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String hashData(String str)
        {
            try
            {
                Utilities oUtilities = new Utilities();
                //byte[] b = Encoding.Default.GetBytes(str);

                byte[] bSignData = (byte[])oUtilities.Hash(SECUINTER_HASH_ALGORITHM.SECUINTER_SHA1_ALGORITHM, oUtilities.GBKEncode(str));
                //2013-03-26 modify luhanmin@cnca.net
                //utilobj.BinaryToHex(utilobj.Hash(SECUINTER_SHA1_ALGORITHM, strBase64));
                return oUtilities.BinaryToHex(bSignData);
            }
            catch (Exception)
            {
                return "";
            }
        }
        /// <summary>获取信息摘要码（HASH码，MD5)
        /// 2013-04-02 修订，解决传入字符串编码的问题
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String hashDataMD5(String str)
        {
            try
            {
                Utilities oUtilities = new Utilities();
                //byte[] b =oUtilities.GBKEncode(str);
                byte[] bSignData = (byte[])oUtilities.Hash(SECUINTER_HASH_ALGORITHM.SECUINTER_MD5_ALGORITHM, oUtilities.GBKEncode(str));
                //2013-03-26 modify luhanmin@cnca.net
                //utilobj.BinaryToHex(utilobj.Hash(SECUINTER_SHA1_ALGORITHM, strBase64));

                return oUtilities.BinaryToHex(bSignData);
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static byte[] readFile(String filename)
        {
            byte[] filedata;
            int filesize;
            FileStream fileStream = new FileStream(filename, FileMode.Open);
            filesize = (int)fileStream.Length;

            filedata = (new BinaryReader(fileStream).ReadBytes(filesize));

            fileStream.Dispose();
            fileStream.Close();

            return filedata;
        }
        #endregion

        #region 其他私有方法
        /// <summary>解析x509证书
        /// * 解析原则：
        ///* 关键是找到符合条件的字符起始、截止位。
        ///* 缺点:,会截断字符
        ///* 1.找到需要解析的字符，如"CN"，取整个字符在"CN"后面的字符
        ///* 2.判断后面的字符是否有"=",并且等号很近（C,O特例，否则必须在1到2个字符间）
        ///* 3.取等号后面的字符，取","
        ///* 4.找到=和,之间的字符
        /// * 5.上述条件不满足，就循环找下一个满足条件的。
        /// </summary>
        /// <param name="dn"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static String parseDN(String dn, String name)
        {

            String superDn = dn.ToUpper();//临时DN
            String superName = name.ToUpper();
            int beginDot = 0; //临时开始点


            while (true)
            {
                int beginName = superDn.IndexOf(superName); //开始点
                if (beginName < 0) return ""; //找不到

                superDn = superDn.Substring(beginName + superName.Length, superDn.Length - beginName - superName.Length); //取后面的串；
                int beginDH = superDn.IndexOf("=");
                if (beginDH < 0)
                { //后面没等号
                    return "";
                }
                else if (beginDH > 1)//后面=号过远
                {
                    beginDot = beginName + beginDot + superName.Length;
                    continue;
                }
                else if ((superName.Equals("C") || superName.Equals("O")) && beginDH == 1)
                { //区别C和CN
                    beginDot = beginDot + beginName + superName.Length;
                    continue; //后面=号过远
                }
                superDn = superDn.Substring(beginDH + "=".Length, superDn.Length - beginDH - "=".Length); //取后面的串；
                int end = superDn.IndexOf(",");
                beginDot = beginDot + beginName + superName.Length + beginDH + "=".Length;
                if (end < 0)//后面没,号
                {
                    //endDot = beginDot + superDn.Length;
                    return dn.Substring(beginDot, superDn.Length);
                }
                else//后面,号,取后面到,号的值
                {
                    //endDot = beginDot + end;
                    return dn.Substring(beginDot, end);
                }

            }

        }
        #endregion
    }
}
