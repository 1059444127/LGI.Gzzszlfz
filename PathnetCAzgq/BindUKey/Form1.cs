using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Forms;
using dbbase;
using Netca_PDFSign_COM;
using NETCA;
using readini;
using SecuInter;

namespace BindUKey
{
    public partial class Form1 : Form
    {

        string imgPath = "";

        public Form1()
        {
            InitializeComponent();

            #region 获取图片地址

            IniFiles f = new IniFiles("sz.ini");
            imgPath = f.ReadString("view", "szqmlj", @"\\127.0.0.1\pathqc\rpt-szqm\YSBMP\");
            txtImgPath.Text = imgPath;

            #endregion
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtReadUKey_Click(object sender, EventArgs e)
        {
            NETCAPKIv4 oNETCAPKIv4 = new NETCAPKIv4();
            ///// 获取用户证书
            string CertUID = "";
            string key_DW = "";
            string key_Name = "";
            string key_Sfzh = "";

            #region 获取ca信息

            try
            {
                try
                {
                    X509Certificate oCert = NETCAPKIv4.getX509Certificate(
                        NETCAPKIv4.SECUINTER_CURRENT_USER_STORE, NETCAPKIv4.SECUINTER_MY_STORE,
                        NETCAPKIv4.SECUINTER_CERTTYPE_SIGN,
                        NETCAPKIv4.SECUINTER_NETCA_YES);
                    if (oCert == null)
                    {
                        MessageBox.Show("未找到证书");
                        return;
                    }
                    ////用户证书绑定值
                    CertUID = (NETCAPKIv4.getX509CertificateInfo(oCert, 9));
                    ////单位
                    key_DW = (NETCAPKIv4.getX509CertificateInfo(oCert, 13));
                    ////用户名称
                    key_Name = (NETCAPKIv4.getX509CertificateInfo(oCert, 12));
                    ////证书序列号
                    //CertID=(oNETCAPKIv4.GetCertInfo(Cert, 2));
                    ////证件号
                    key_Sfzh = (NETCAPKIv4.getX509CertificateInfo(oCert, 36));

                    txtCaId.Text = CertUID;
                    txtCaName.Text = key_Name;
                }
                catch (Exception ee1)
                {
                    if (ee1.Message == "证书选择失败")
                    {
                        MessageBox.Show("证书选择失败，请确认Key盘是否插入");
                    }
                    else
                    {
                        MessageBox.Show(ee1.Message);
                    }
                    return;
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("程序初始化失败");
                return;
            }

            #endregion
        }

        private void btnBindUser_Click(object sender, EventArgs e)
        {
            #region 保存前验证

            if (string.IsNullOrEmpty(txtUserCode.Text.Trim()))
            {
                MessageBox.Show("用户编码不能为空!");
                return;
            }
            if (string.IsNullOrEmpty(txtImgPath.Text.Trim()))
            {
                MessageBox.Show("图片路径不能为空!");
                return;
            }

            var caUserName = txtCaName.Text.Trim();
            if (string.IsNullOrEmpty(txtCaId.Text.Trim() + caUserName))
            {
                MessageBox.Show("CAId和Ca用户名不能为空,请先点击读取!");
            }

            var dtYh = GetYHXX(txtUserCode.Text.Trim());
            if (dtYh.Rows.Count == 0)
            {
                MessageBox.Show($"没有找到用户:" + txtUserCode.Text.Trim());
                return;
            }

            var dbUserName = dtYh.Rows[0]["F_YHMC"].ToString().Trim();
            if (dbUserName != caUserName)
            {
                DialogResult r = MessageBox.Show($"用户姓名验证失败!\r\nUKey姓名为:{dbUserName};朗珈用户姓名为:{caUserName}\r\n是否继续绑定?",
                    "", MessageBoxButtons.YesNo);
                if (r != DialogResult.Yes)
                    return;
            }

            #endregion

            #region 把CaId保存到用户表BY2字段

            odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
            string sql = $"update t_yh set F_YH_BY2='{txtCaId.Text.Trim()}' where f_yhm='{txtUserCode.Text.Trim()}' ";
            try
            {
                aa.ExecuteSQL(sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("绑定失败,执行sql时出现异常:" + ex);
                return;
            }
            finally
            {
                aa.Close();
            }

            #endregion
            
            #region 获取ca签名图片并上传到指定路径,图片名称为 ca用户姓名.bmp

            IPDFSign iPDFSign = new PDFSign();
            //选择证书
            iPDFSign.SelectCert("netca", 0);
            IUtilTool iUtilTool = new UtilTool();
            //传入选中的签名证书的base64编码
            string CertBase64 = iPDFSign.SignCertBase64Encode;
            if (CertBase64.Trim() == "")
            {
                MessageBox.Show("获取签名证书的base64编码失败");
                return;
            }
            try
            {
                //log.WriteMyLog("CertBase64:"+CertBase64);
                byte[] image = iUtilTool.GetImageFromDevicByCert(CertBase64);
                try
                {
                    MemoryStream memoryStream = new MemoryStream(image, 0, image.Length);
                    memoryStream.Write(image, 0, image.Length);
                    //转成图片
                    Image ii = Image.FromStream(memoryStream);
                    ii.Save(txtImgPath.Text + caUserName + ".bmp", ImageFormat.Bmp);
                }
                catch (Exception ee4)
                {
                    MessageBox.Show("保存签名图片失败：" + ee4.Message);
                    return;
                }
            }
            catch (Exception ex1)
            {
                MessageBox.Show("保存签名图片时出现异常:" + ex1);
                return;
            }

            #endregion

            MessageBox.Show("绑定成功!");
        }

        private DataTable GetYHXX(string f_yhm)
        {
            DataTable Dt_Yhxx = new DataTable();
            if (f_yhm.Trim() != "")
            {
                try
                {
                    dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
                    Dt_Yhxx = aa.GetDataTable("select top 1 * from T_YH  where f_yhm='" + f_yhm + "' ", "YHXX");
                }
                catch (Exception e1)
                {
                    MessageBox.Show(e1.Message);
                }
            }
            return Dt_Yhxx;
        }
    }
}