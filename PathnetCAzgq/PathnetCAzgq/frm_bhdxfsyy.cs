using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using readini;

namespace PathnetCAzgq
{
    public partial class frm_bhdxfsyy : Form
    {
        //������ѧ����ҽԺ
        public frm_bhdxfsyy(string lx, string cslb)
        {
            czlx = lx;
            cs = cslb;
            InitializeComponent();
        }
        private string czlx = "";
        IniFiles f = new IniFiles(Application.StartupPath + "\\sz.ini");
        private string ysbmppath = "";
        public string ysxm = "";
        private string cs = "";
        private static string F_zhbh = "";
        public string SN_ID = "";
        public string SU_ID = "";

        private void frm_bhdxfsyy_Load(object sender, EventArgs e)
        {
                if (czlx.ToLower() == "login")
                {
                    ysxm = "0";
                    ysbmppath = f.ReadString("CA", "bmppath", "d:\\pathqc\\rpt-szqm\\ysbmp").Replace("\0", "");

                    if (System.IO.Directory.Exists(ysbmppath))
                    {
                    }
                    else
                    {
                        try
                        {
                            System.IO.Directory.CreateDirectory(ysbmppath);
                        }
                        catch
                        {
                            MessageBox.Show("ǩ��Ŀ¼����ʧ�ܣ�");
                            ysxm = "0";
                            this.Close();
                        }
                    }
                    bool yy = axAXSecurity1.OpenCert(0, "1", 1);
                    if (yy)
                    {
                        //string zslb = f.ReadString("CA", "zslb", @"D:\pathqc\rl37.crl");
                        //yy = axAXSecurity1.CheckCRL(0, zslb);
                        //if (yy)
                       // {
                            string msg = axAXSecurity1.GetCertInfo(1, "");
                            string name = msg.Split(',')[0].Split('=')[1].Split('U')[0];
                            //֤�����к�
                            string zhbh = axAXSecurity1.GetCertInfo(2, "");
                            //֤��������Կ��ʶ
                            string zsmcID = axAXSecurity1.GetCertInfo(4, "");
                            F_zhbh = zhbh;
                            SN_ID = zhbh;
                            SU_ID = zsmcID;



                            bool xx = axAXSecurity1.SetSignerCert(2, zhbh);
                            if (xx)
                            {
                                string bmp = axAXSecurity1.ReadFileFromKey(0,2);
                                if (bmp != "")
                                {                 
                                    axAXSecurity1.B64DecodeSToFile(bmp, @"c:\temp\" + msg.Split(',')[0] + ".gif");
                                    Bitmap bm1 = new Bitmap(@"c:\temp\" + msg.Split(',')[0] + ".gif");
                                    bm1.Save(ysbmppath + "\\" + name + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                                 
                                  string m= axAXSecurity1.SignString("cs", true);
                                    
                                    if(m.Trim()=="")
                                        ysxm = "0";
                                    else
                                    ysxm = name;
                                }
                                else
                                    ysxm = "0";
                            }
                            else
                                MessageBox.Show("������Ϣ��" + axAXSecurity1.GetLastError());
                        //}
                        //else
                        //{
                        //    MessageBox.Show("֤����Ч��������Ϣ��" + axAXSecurity1.GetLastError());
                        //    ysxm = "0";
                        //}
                    }
                    else
                    {
                        MessageBox.Show("��֤������Ϣ��" + axAXSecurity1.GetLastError());
                        ysxm = "0";
                    }

                    this.Close();
                }
                if (czlx.ToLower() == "sh")
                {
                    ysxm = "0";
                    
                    if (axAXSecurity1.OpenCert(2, F_zhbh, 1))
                    {
                      
                        bool xx = axAXSecurity1.SetSignerCert(2, F_zhbh);
                        if (xx)
                        {
                            if (cs == "shq")
                                ysxm = "1";
                            else
                            {
                                string yw = cs;
                                string mw = axAXSecurity1.SignString(yw, true);
                                if (mw != "")
                                ysxm = "֤����:" + F_zhbh + " ����:" + mw;
                                else
                                 ysxm = "0";
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("ukey�Ѱγ�������������µ�¼��");
                        ysxm = "0";
                    }
                    this.Close();
                }
                else
                {
                    this.Close();
                }
                this.Close();
            
        }
    }
}