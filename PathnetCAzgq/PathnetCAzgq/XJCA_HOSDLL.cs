using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using LGZGQClass;
namespace PathnetCAzgq
{
    class XJCA_HOSDLL
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(
        string lpAppName,
        string lpKeyName,
        string lpString,
        StringBuilder retVal,
            int size,
        string lpFileName
        );

        private delegate bool XJCA_SignSeal(
        string src,
        StringBuilder signxml,
         ref Int32 len
        );

        private delegate bool XJCA_GetSealBMPB(
        string filepath,
        int times
        );

        private delegate void XJCA_MakePdf417ToFile(
        StringBuilder ucData,
        int nDataLen,
        string szBmpFileName,
        int nClumn,
        int nErr,
        int nHLRatio,
        int nHeight
        );

        private delegate string XJCA_TimeSign(
        string ip,
        string djid,
        StringBuilder hash
        );

        public static bool Xjca_SignSeal(string src, StringBuilder signxml, ref Int32 len)
        {
            LoadDllapi dllloadz = new LoadDllapi();

            if ((int)dllloadz.initPath("XJCA_HOS.dll") == 0)
            {
                log.WriteMyLog("打印控件调用错误！");
                return false;
            }
            XJCA_SignSeal xx = (XJCA_SignSeal)dllloadz.InvokeMethod("XJCA_SignSeal", typeof(XJCA_SignSeal));

            return xx(src, signxml, ref len);
        }
        public static bool Xjca_GetSealBMPB(string filepath, int times)
        {
            LoadDllapi dllloadz = new LoadDllapi();

            if ((int)dllloadz.initPath("XJCA_HOS.dll") == 0)
            {
               log.WriteMyLog("打印控件调用错误！");
                return false;
            }
            XJCA_GetSealBMPB xx = (XJCA_GetSealBMPB)dllloadz.InvokeMethod("XJCA_GetSealBMPB", typeof(XJCA_GetSealBMPB));

            return xx(filepath, times);
        }
        public static void Xjca_MakePdf417ToFile(StringBuilder ucData, int nDataLen, string szBmpFileName, int nClumn, int nErr, int nHLRatio, int nHeight)
        {
            LoadDllapi dllloadz = new LoadDllapi();

            if ((int)dllloadz.initPath("XJCA_HOS.dll") == 0)
            {
                log.WriteMyLog("打印控件调用错误！");
                //return false;
            }
            else
            {
                XJCA_MakePdf417ToFile xx = (XJCA_MakePdf417ToFile)dllloadz.InvokeMethod("XJCA_MakePdf417ToFile", typeof(XJCA_MakePdf417ToFile));

                xx(ucData, nDataLen, szBmpFileName, nClumn, nErr, nHLRatio, nHeight);
            }
        }
        public static string Xjca_TimeSign(string ip, string djid, StringBuilder hash)
        {
            LoadDllapi dllloadz = new LoadDllapi();

            if ((int)dllloadz.initPath("XJCA_HOS.dll") == 0)
            {
                log.WriteMyLog("打印控件调用错误！");
                return "";
            }
            XJCA_TimeSign xx = (XJCA_TimeSign)dllloadz.InvokeMethod("XJCA_TimeSign", typeof(XJCA_TimeSign));

            return xx(ip, djid, hash);
        }

    }
}
