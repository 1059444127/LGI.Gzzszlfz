using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using readini;
using System.Windows.Forms;
using System.Diagnostics;
using dbbase;
using LGZGQClass;


namespace PathnetCAzgq
{


    [ComVisible(true)]
    public interface iClass_CA
    {
        string Login_CA(string yymc);
       // string CA(string user, string passwd, string by);
        string DYBG(string yymc);
    }

    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class CA_Class : iClass_CA
    {

        public string DYBG(string cslb)
        {
            log.WriteMyLog("dybg:"+cslb);

            IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        
            string[] dystr = cslb.Split('^');

            if (dystr.Length > 4)
            {
                if (dystr[0].ToUpper() == "TYDYBG")
                {
                    //打印报告
                    //收到指令:TYDYBG^20018^1^CG^DY^zpt^zpt^^1
                    string blh = dystr[1].ToString();
                    string bgxh = dystr[2].ToString();
                    string bglx = dystr[3].ToString();
        

                    string yymc2 = "";
                    if (yymc2.Trim() == "")
                        yymc2 = f.ReadString("TYDYBG", "yymczgq", "").Replace("\0", "");
                                                                
                    if (yymc2.ToUpper() == "XM1Y")
                    {
                       Process.Start("d:\\pathqc\\XM1Y_CA_SHOW.exe",cslb);
                      
                      //  string[] aa = new string[2];
                      //  aa[0] = cslb;
                      //  aa[1] = "DY";
                      ////jk_c bgdy = new jk_cs.Form1(aa);
                      //  xm1ybgdy bgdy = new xm1ybgdy(cslb);
                      ////   bgdy.= new xm1ybgdy(cslb)
             
                       // XM1Y_CA_SHOW.xm1ybgdy bgdy = new XM1Y_CA_SHOW.xm1ybgdy("^^^^^^^^^^");
                    
                        //bgdy.Show();
                        //bgdy.Dispose();
                        //bgdy.Close();
                    
                  // System.Diagnostics.Process.Start("SealComExe.exe", "111" + "," + "11111").WaitForExit(1000);
                        return "0";
                    }
           
                    return cslb;
                }

            }


            return cslb;
        }

        public string Login_CA(string yhxx)
        {
            
////rpt^shyzkey RPT-审核时验证KEY 0/1
////Login_CA("TYSZQZ^" + psCURBLH + "^" + sCURXH + "^BD^QXQZ^" + psYHM + "^" + psYHMC + "^" + psYHBH + "^" + psYHMM)
////rpt^shhszqz RPT-审核后数字签字 0/1
////Login_CA("SHQTYSZQZ^" + psCURBLH + "^1^CG^QXSH^" + psYHM + "^" + psYHMC + "^" + psYHBH + "^" + psYHMM)
////只涉及常规报告的取消审核.
////rpt^shqszqz RPT-审核前数字签字 0/1
            ////审核：
            ////审核前  SH^ZPT^123456^张普通^1
            ////审核后  TYSZQZ^病理号^1^CG^QZ^ZPT^张普通^123456^1
            ////取消审核：
            ////取消审核前  SHQTYSZQZ^病理号^1^CG^QXSH^ZPT^张普通^123456^1
            ////取消审核后  TYSZQZ^病理号^1^CG^QXQZ^ZPT^张普通^123456^1

            ///
            ///审核前
            ///
            //if (type == "SH")
            //{
            //    //验证key
            //}

            /////
            /////审核后
            /////
            //if (type == "QZ" && type2 == "TYSZQZ")
            //{
            //}

            /////
            /////取消审核前
            /////
            //if (type == "QXSH" && type2 == "SHQTYSZQZ")
            //{
            //}

            /////
            /////取消审核后
            /////
            //if (type == "QXQZ" && type2 == "TYSZQZ")
            //{
            //}
          log.WriteMyLog("login ca :"+yhxx);


            IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
          
              string   yymc = f.ReadString("CA", "yymczgq", "").Replace("\0", "");
             
            if (yymc.Trim() == "")
                return "0";
            
            log.WriteMyLog(yymc);


            if (yymc == "XMDYYY")
            {
                XMDYYY xx = new XMDYYY();
                string xyd = xx.ca(yhxx);
                return xyd;
            }
            if (yymc == "gdszyy")
            {

                gdszyy gdsz= new gdszyy();
                string xyd = gdsz.ca(yhxx);
                return xyd;
            }
            if (yymc == "gdsdermyy")
            {
              
                gdsdermyy gds2 = new gdsdermyy();
                string xyd = gds2.ca(yhxx);
                return xyd;
            }

            if (yymc == "gzzszlly")
            {
                gzzszlly zsyy = new gzzszlly();
                string xyd = zsyy.ca(yhxx);
                return xyd;
            }

            if (yymc == "xmzsyy")
            {
                xmzsyy zsyy = new xmzsyy();
                string xyd = zsyy.ca(yhxx);
                return xyd;
            }


            if (yymc == "klmy")
            {
              
                klmy xx = new klmy();
                string xyd = xx.ca(yhxx);
                GC.Collect();
                return xyd;

            }
            if (yymc == "xm1y")
            {
                xm1y xx = new xm1y();
                 string xyd = xx.ca(yhxx);
                 return xyd;
            }
            if (yymc == "bhdxfsyy")
            {
                bhdxfsyy xx = new bhdxfsyy();
                string xyd = xx.ca(yhxx);
                return xyd;
            }
            //if (yymc == "gdca_zs2y")
            //{

         
            //   // log.WriteMyLog("CA开始：" + yhxx);
            //    gdca_zs2y xx = new gdca_zs2y();
            //    string xyd = xx.ca(yhxx);
            //   // log.WriteMyLog("CA结束：" + xyd);
            //    return xyd;
            //}
            else
            {
                MessageBox.Show("(zgq)此医院未设置CA认证！" + yymc);
                return "0";
            }
        }
    }
}

