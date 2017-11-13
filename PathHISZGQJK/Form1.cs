using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dbbase;
using System.IO;
using System.Collections;
using System.Xml;
using System.Diagnostics;
using ZgqClassPub;
using PathHISJK;
using System.Threading;
namespace PathHISZGQJK
{
    public partial class Form1 : Form
    {
        public string[] execslb;
        string args = ""; 
        public Form1(string[] args1)
        {

            bglx = ""; bgxh = ""; czlb = "";
            InitializeComponent();
            try
            {
                
                args = args1[0].ToString();
              
                execslb = args.Split(',');
                if (args.Contains(","))
                {

                    int arg = args1[0].IndexOf(",");

                    if (arg < 0)
                    {
                        fromexit = true;
                    }
                    else
                    {
                        try
                        {
                            blh = args1[0].Substring(0, arg);
                            string hisbz = args1[0].Substring(arg + 1);
                            if (hisbz == "bz")
                            {
                                yymc = f.ReadString("savetohis", "yymc2", "123").Replace("\0", "");
                            }
                            else
                            {
                                yymc = hisbz;
                            }
                        }
                        catch
                        {
                            blh = "";
                            yymc = "";
                        }
                    }
                }
                else
                {  //复杂接口,打印时上传
                    //args1[0] = 病理号^cg/bd/bc^bgxh^new/old^save/qxsh
                    if (args1[0].IndexOf("^") > -1)
                    {   
                        try
                        {
                            string[] aa = args1[0].Split('^');
                            execslb = aa;
                            blh = aa[0].ToString();
                            bglx = aa[1].ToString().ToLower();//cg/bd/bc
                            bgxh = aa[2].ToString();
                            czlb = aa[3].ToString().ToLower();//new/old
                            dz = aa[4].ToString().ToLower();//save/qxsh/dy/qxdy
                        }
                        catch
                        {
                            ZgqClassPub.log.WriteMyLog("传入参数解析出错");
                            return;
                        }
                        fromexit = false;
                        yymc = f.ReadString("savetohis", "yymc", "123").Replace("\0", "");
                    }
                    else
                        fromexit = true;
                }
            }
            catch
            {
                MessageBox.Show("参数无效");
                fromexit = true;
            }
          
        }
       
       
        public IniFiles f = new IniFiles(Application.StartupPath+"\\sz.ini");
        public bool fromexit = false;
        public string blh, yymc,bglx,bgxh,dz,czlb,msg,debug; 
        private void Form1_Load(object sender, EventArgs e)
        {

            this.Hide();
            debug = f.ReadString("savetohis", "debug", "");
            msg = f.ReadString("savetohis", "msg", "");
            if (fromexit)
            {
                MessageBox.Show("无权启动程序！+ver 1.0.0.0", "ZGQ");
                this.Close();
            }
            else
            {

                if (debug == "1")
                    ZgqClassPub.log.WriteMyLog(yymc+"   "+ args);
                if (yymc == "pdf")//
                {
                    pdfcs gz = new pdfcs();
                    gz.pdfjpg(blh, bglx, bgxh, czlb, dz, msg, debug, "pdf");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "jpg")//
                {
                    pdfcs gz = new pdfcs();
                    gz.pdfjpg(blh, bglx, bgxh, czlb, dz, msg, debug, "jpg");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "xjca")//
                {
                    xjca ca = new xjca();
                    ca.CAQM(blh, bglx, bgxh, msg, debug,dz);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                 if (yymc == "sxsszyy")//
                {
                    sxssgyy szyy = new sxssgyy();
                    szyy.pathtohis(blh);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "xjzlyy")//
                {

                    xjzlyy xjzl = new xjzlyy();
                    xjzl.pathtohis(blh, bglx, bgxh, msg, debug, dz);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "klmyzxyy")//成功中医药大学附属医院 体检接口
                {

                    klmyzxyy klmy = new klmyzxyy();
                    klmy.CAQM(blh, bglx, bgxh, msg, debug, dz);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "cdzyydx")//成功中医药大学附属医院 体检接口
                {

                    cdzyydx cdzyy = new cdzyydx();
                    cdzyy.pathtohis(blh, debug);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "whslyy")//威海市立医院
                {

                    sdswhslyy whslyy = new sdswhslyy();
                    whslyy.pathtohis(blh, debug);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "zsdx2y")//中山大学二院
                {

                    zsdx2y zs2y = new zsdx2y();
                    zs2y.pathtohis(blh,bglx,bgxh,msg,debug,dz);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "gdsdermyy")//广东省第二人民医院
                {

                    gdsdermyy gds2 = new gdsdermyy();
                    gds2.pathtohis(blh, bglx, bgxh, czlb, dz,msg ,debug);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }

                if (yymc == "jzykdxfsyy")//锦州
                {

                    jzykdxfsyy jz = new jzykdxfsyy();
                    jz.pathtohis(blh, bglx, bgxh,czlb,dz, debug);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }

                if (yymc == "bjxwyy")//北京宣武医院 生成pdf
                {
                    PathHISJK.DJYXPT dj = new PathHISJK.DJYXPT();
                    dj.djyxpt_hx(blh, bglx, bgxh, czlb, dz, yymc);
                    Thread.Sleep(1000);
                    bjxwyy bjxw = new bjxwyy();
                    bjxw.pathtohis(blh, bglx, bgxh, czlb, dz, msg, debug);

                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }

                if (yymc == "xyfyyy")//湘雅附一医院
                {
                 
                    xyfyyy cdyy = new xyfyyy();
                    cdyy.pathtohis(blh, bglx, bgxh, czlb, dz, debug, execslb);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }

                if (yymc == "xyyy_jx")//湘雅附一医院绩效接口
                {
                    xyyy_gzl cdyy = new xyyy_gzl();
                    cdyy.pathtohis(blh, "");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }


                if (yymc == "xy2y")//湘雅2院
                {
                    xy2y xy = new xy2y();
                    xy.pathtohis(blh,debug);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }

                if (yymc == "gdszyy")//广东省中医院
                {
                    gdszyy gdsz = new gdszyy();
                    gdsz.pathtohis(blh, bglx, bgxh, msg, debug, execslb);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }

                if (yymc == "cdrmyypt")//常德人民医院 IBMMQ平台
                {
                    cdrmyyPT cdyy = new cdrmyyPT();
                    cdyy.pathtohis_PT(blh, bglx, bgxh, msg, debug, execslb);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "zjstzyy")//浙江省台州医院
                {
                    zjstzyy gdsz = new zjstzyy();
                    gdsz.pathtohis(blh, bglx, bgxh, msg, debug, execslb);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }

                if (yymc == "tzyysm")//浙江省台州医院chj
                {
                     debug = f.ReadString("savetohis", "debug", "0");
                    tzyysm tzyy = new tzyysm();
                    tzyy.tzyyhx(blh, debug, bglx, bgxh, czlb, dz);
                    this.Close();
                }
                if (yymc == "sh5y")// 
                {

                    sh5y gz = new sh5y();
                    gz.pathtohis(blh,debug);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "gzslyy")// 赣州市立医院
                {

                    gzslyy gz = new gzslyy();
                    gz.pathtohis(blh, bglx, bgxh, debug, dz, msg);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }

                if (yymc == "klmyrmyy")//克拉玛依人民医院
                {
                    //xmzyjcpt szyy = new xmzyjcpt();
                    //szyy.pathtohis(blh, yymc);
                    klmysrmyy klmy = new klmysrmyy();
                    klmy.pathtohis(blh,yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }

                if (yymc == "fjljxyy")// 福建连江县医院
                {

                    fjljxyy lj = new fjljxyy();
                    lj.pathtohis(blh,debug);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }


                if (yymc == "qzey")// 泉州2院
                {

                    qz2y qz = new qz2y();
                    qz.pathtohis_fz(blh,bglx,bgxh,czlb,dz);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }

                if (yymc == "myzxyy")// 绵阳市中心医院
                {
                    myzxyy myzx= new myzxyy();
                    myzx.myzxyy_hx(blh,debug);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }

            

                if (yymc == "ayd2fy")// 安医大二附院，智业hl7
                {
                    ayd2fy xm = new ayd2fy();
                    xm.pathtohis(blh, bglx, bgxh, msg, debug, execslb);

                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }

                if (yymc.ToUpper().Trim() == "XMDYYY")// 厦门第一医院，智业hl7
                {
                    XMDYYY xm = new XMDYYY();
                    xm.pathtohis(blh, bglx, bgxh, msg, debug, dz);

                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }

                if (yymc.ToUpper().Trim() == "HNHHSY")// 湖南怀化3院
                {
                    hnhhsy qh = new hnhhsy();
                    qh.pathtohis(blh, debug);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc.ToUpper().Trim() == "XMZSYY")// 厦门中山医院
                {
                    xmzsyy qh = new xmzsyy();
                    qh.pathtohis(blh, bglx, bgxh, msg, debug, execslb);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }


                if (yymc == "nbblrmyy")//宁波北仑人民医院
                {
                    nbblrmyy blyy = new nbblrmyy();
                    blyy.blrmyytohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "szszyy")//苏州盛泽医院
                {
                    szszyy szyy = new szszyy();
                    szyy.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "bjyhyy")//北京燕化医院
                {
                    bjyhyy szyy = new bjyhyy();
                    szyy.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "xmzyjcpt")//克拉玛依人民医院
                {
                    xmzyjcpt szyy = new xmzyjcpt();
                    szyy.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "nbyzrmyy")//宁波鄞州人民医院
                {
                    nbyzrmyy yzyy = new nbyzrmyy();
                    yzyy.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "whxhyy")//武汉协和医院
                {
                    whxhyy whxh = new whxhyy();
                    whxh.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "nbyzd2yy")//宁波鄞州第二医院
                {
                    nbyzd2yy yz2y = new nbyzd2yy();
                    yz2y.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "cszxyy")//长沙中心医院
                {

                    cszxyy zxyy = new cszxyy();
                    zxyy.pathtohis(blh, yymc, bglx);
                    zxyy.pdf(blh, bglx, bgxh, msg, debug, execslb);
                   // yz2y.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "xjdszyy")//新疆独山子医院
                {
                    xjdszyy dszyy = new xjdszyy();
                    dszyy.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "cqjjzxyy")//重庆-江津中心医院
                {

                    cdyx_cqjj cqjj = new cdyx_cqjj();
                    cqjj.RTXML(blh, "1");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "djyrmyy")//都江堰人民医院
                {
                    cdyx_djy djy = new cdyx_djy();
                    djy.RTXML(blh, "1");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "cqycyy")//重庆医科大学附属永川医院
                {
                    cqycyy cqyc = new cqycyy();
                    cqyc.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }

                if (yymc == "cdrmyy")//常德人民医院
                {
                    cdrmyy cdyy = new cdrmyy();
                    cdyy.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }

                if (yymc == "npsdryy")//南平市第二医院
                {

                    npsdryy cdyy = new npsdryy();
                    cdyy.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "wysslyy")//武夷山市立医院
                {

                    wysslyy wys = new wysslyy();
                    wys.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "xm1y")// 厦门一院
                {

                    xm1y xm = new xm1y();
                    xm.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "qz2y")// 泉州2院
                {

                    qz2y qz = new qz2y();
                    qz.pathtohis(blh, yymc, args);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "cdbasys")// //成都博奥独立医学实验室
                {

                    cdbadlyxsys cdbasys = new cdbadlyxsys();
                    cdbasys.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "bjhtyy")// //北京航天医院
                {

                    bjhtyy bjht = new bjhtyy();
                    bjht.bjhtyy_hx(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "syszxyy")// //邵阳市中心医院--HL7-费用确认
                {

                    syszxyy sy = new syszxyy();
                    sy.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "hzhhyy")// //杭州红会医院-体检接口，JPG
                {
                    hzhhyy hh = new hzhhyy();
                    hh.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "hnsrmyy")// //湖南省人民医院-体检接口，JPG
                {
                    hnsrmyy hh = new hnsrmyy();
                    hh.pathtohis(blh,debug);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "gddgsdhyy")// //东莞市东华医院，JPG
                {
                    gddgsdhyy hh = new gddgsdhyy();
                    hh.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "cssfy")// //湖南省长沙市妇幼保健院-体检接口，JPG
                {
                    cssfy hh = new cssfy();
                    hh.pathtohis(blh, bglx, bgxh, czlb, dz, msg, debug);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "shsrjyy")// //上海市仁济医院  his 存储过程
                {
                    shsrjyy hh = new shsrjyy();
                    hh.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "gdsrmyy")// //广东省人民医院  集成平台-webservices
                {
                    gdsrmyy sy = new gdsrmyy();
                    sy.pathtohis(blh, debug, msg);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
            
                if (yymc == "zsdxzlyy")// 中山大学肿瘤医院
                {
                    zsdxzlyy zlyy = new zsdxzlyy();
                    zlyy.pathtohis(blh, bglx, bgxh, msg, debug, execslb);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();


                }
                if (yymc == "zsdxzlyypt")// 中山大学肿瘤医院平台
                {
                    zsdxzlyyPT zlyy = new zsdxzlyyPT();
                    zlyy.pathtohis(blh, bglx, bgxh, msg, debug, execslb);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "zsdxzlyyfzpt")// 中山大学肿瘤医院平台
                {
                    zsdxzlyyfzPT zlyy = new zsdxzlyyfzPT();
                    zlyy.pathtohis(blh, bglx, bgxh, msg, debug, execslb);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
                if (yymc == "sxdey")//  山西医科大第二医院  webservices
                {
                    sxeyy sxey = new sxeyy();
                    sxey.sxeyy_hx(blh);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "hnndnkyy")//  海南那大农垦医院  数据库写表
                {
                    hnndnkyy sxey = new hnndnkyy();
                    sxey.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "lzyxyfsyy")// 泸州医学院附属医院  
                {
                    lzyxyfsyy fsyy = new lzyxyfsyy();
                    fsyy.pathtohis(blh, msg, debug);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "fjsfybjy")// 福建省妇幼保健院   智业 hl7
                {
                    fjsfybjy fjsfy = new fjsfybjy();
                    fjsfy.pathtohis(blh, bglx, bgxh, msg, debug, execslb);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }

                if (yymc == "shsjdqzyyy")// 上海市嘉定区中医医院--复高--webservices
                {
                    shhjdqzyyy shjd = new shhjdqzyyy();
                    shjd.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "zgsdsrmyy")// 自贡市第四人民医院
                {
                    zgsdsrmyy shjd = new zgsdsrmyy();
                    shjd.pathtohis(blh, bglx, bgxh, czlb, dz, debug, msg);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }

                if (yymc == "xjshzyy")//新疆石河子医院，卫宁 hl7
                {
                    xjshzdxfsyy shjd = new xjshzdxfsyy();
                    shjd.pathtohis(blh, yymc);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "nbsxrmyy")// 宁波象山人民医院
                {
                    nbxsrmyy xs = new nbxsrmyy();
                    xs.pathtohis(blh, debug, msg);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
               
                if (yymc == "nysy")// 南方医科大学第三医院 ，体检接口
                {
                    nysy ny = new nysy();
                    ny.pathtohis(blh, "");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "fjsd2yy")// 福建省二院，生成pdf存服务器，供门诊打印
                {                  
                    fjsey fjse = new fjsey();
                   
                    fjse.pathtohis(blh, bglx, bgxh, msg, debug, execslb);
                    //调用老的PathHISJK接口，未找到代码
                  
                    string oldjk = f.ReadString("savetohis", "oldjk", "1");
                    if (oldjk.Trim()=="1")
                    {
                        try
                        {
                            Process.Start(System.Windows.Forms.Application.StartupPath + "\\PathHISJK.exe", blh + ",fjsey");
                        }
                        catch
                        {
                        }
                    }
        
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                 

                }

                if (yymc == "qhdxfsyy")// 青海大学附属医院,oracle 表
                {
                    qhdxfsyy qh = new qhdxfsyy();
                    qh.pathtohis(blh, "");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();

                }
                if (yymc == "zsdxykyy")// 中山大学眼科医院,审核后发送短信，平台接口，HIS接口
                {
                    zsdxykyy zsyk = new zsdxykyy();
                    zsyk.pathtohis(blh, bglx, bgxh, msg, debug, execslb);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    this.Close();
                }
            if (yymc.ToUpper().Trim() == "ZZZXYY")//株洲中心医院---钱龙
            {
                ZZZXYY zzzxyy = new ZZZXYY();
                zzzxyy.pathtohis(blh, yymc);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                this.Close();
            }
      
                MessageBox.Show(yymc + "无此医院参数zgq！", "ZGQJK", MessageBoxButtons.OK);
                this.Close();
                Application.Exit();
            }
        }
 

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

    }
}