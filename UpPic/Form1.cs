using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using dbbase;
using DevExpress.Xpf.Core.Native;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Native;

namespace UpPic
{
    public partial class Form1 : XtraForm
    {
        private string folderPath = "";
        private List<ReportImage> reportImages = new List<ReportImage>();
        private IniFiles f = new IniFiles("sz.ini");
        dbbase.odbcdb aa = new odbcdb("DSN=pathnet;UID=pathnet;PWD=4s3c2a1p", "", "");
        public Form1()
        {
            InitializeComponent();
        }

        private FtpWeb GetFtp()
        {
            string ftpserver = f.ReadString("ftp", "ftpip", "").Replace("\0", "");
            string ftpuser = f.ReadString("ftp", "user", "ftpuser").Replace("\0", "");
            string ftppwd = f.ReadString("ftp", "pwd", "ftp").Replace("\0", "");
            string ftpremotepath = f.ReadString("ftp", "ftpremotepath", "pathimages").Replace("\0", "");
            return new FtpWeb(ftpserver, ftpremotepath, ftpuser, ftppwd);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //获取图片路径,如果路径为空,则让用户选择路径,并保存到config
            folderPath = ConfigurationManager.AppSettings["ImageFolder"];
            if (string.IsNullOrEmpty(folderPath))
            {
                MessageBox.Show("没有找到图片文件夹,请在config文件中修改ImageFolder为图片文件夹地址.");
                Application.Exit();
            }

            //获取图片
            GetImages();
        }

        private void GetFloderPath()
        {
            var fd = new FolderBrowserDialog();
            var r = fd.ShowDialog();
            if (r == DialogResult.OK)
            {
                folderPath = fd.SelectedPath;
            }
            else if(string.IsNullOrEmpty(folderPath))
            {
                folderPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
            ConfigurationManager.AppSettings.Set("ImageFolder", folderPath);
        }

        private void GetImages()
        {
            var files = Directory.GetFiles(folderPath);
            var fileInfos = files.Select(o => new FileInfo(o)).ToList();
            reportImages.Clear();
            pictureEdit1.Image = null;
            
            foreach (FileInfo fileInfo in fileInfos)
            {
                var names = fileInfo.Name.Replace(fileInfo.Extension, "").Split('_');
                if (names.Length < 4) continue;

                var reportImage = new ReportImage();
                reportImage.FileFullName = fileInfo.FullName;
                reportImage.FileSafeName = fileInfo.Name;
                reportImage.Xm = names[0];
                reportImage.Blh = names[1];
                reportImage.Txxh = Convert.ToInt32(names.Last());

                reportImages.Add(reportImage);
            }
            reportImages.Sort(CompareByBlhAndXh);

            reportImageBindingSource.DataSource = reportImages;
            gridView1.BestFitColumns();
        }

        private int CompareByBlhAndXh(ReportImage x, ReportImage y)
        {
            if (String.Compare(x.Blh, y.Blh, StringComparison.Ordinal) != 0)
                return String.Compare(x.Blh, y.Blh, StringComparison.Ordinal);
            return x.Txxh.CompareTo(y.Txxh);}

        private void reportImageBindingSource_CurrentItemChanged(object sender, EventArgs e)
        {
            ReportImage ri = reportImageBindingSource.Current as ReportImage;
            if (ri == null) return;

            //防止image占用文件导致无法删除本地文件           
            Image img = Image.FromStream(new MemoryStream(File.ReadAllBytes(ri.FileFullName)));
            pictureEdit1.Image = img;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var importList = reportImages.Where(o => o.IsUpload).ToList();

            var imgBlh = (from o in reportImages group o by o.Blh into g select g.Key).ToList();
            //获取图像目录,如果没有则设置为yyyyMM,并更新jcxx
            foreach (string blh in imgBlh)
            {
                var dtJcxxMl = aa.GetDataTable($" select f_txml from t_jcxx where f_blh='{blh}' ", "dt1");
                if (dtJcxxMl.Rows.Count == 0)
                {
                    MessageBox.Show($"分子诊断戏中未找到病理号为:[{blh}]的患者,本次导入被终止,请检查病人信息!");
                    return;
                }
                var txml = dtJcxxMl.Rows[0][0].ToString();

                //如果图像目录为空 设置为年月,并更新jcxx
                if (string.IsNullOrEmpty(txml.Trim()))
                {
                    txml = DateTime.Now.ToString("yyyyMM");
                    aa.ExecuteSQL($" update t_jcxx set f_txml = '{txml}',F_SFCT='是' where f_blh='{blh}' ");
                }
                
                //txml赋值给对象
                reportImages.Where(o=>o.Blh==blh).ForEach(o=>o.Txml=txml);

                //FTP尝试新建文件夹
                string errMakeDir = "";
//                var ftp = GetFtp();
//                ftp.Makedir(txml+ "/" + blh + "/", out errMakeDir);

                string sqlDel = $" delete t_tx where f_blh='{blh}' and f_txsm='二代测序上传' ";
                //清空t_tx历史数据
                aa.ExecuteSQL(sqlDel);
            }


            //开始上传图片
            foreach (ReportImage reportImage in importList)
            {
                //上传文件
                string errUpload = "";
                var ftp = GetFtp();
                ftp.Makedir(reportImage.Txml , out errUpload);
                ftp.Makedir(reportImage.Txml + "\\" + reportImage.Blh, out errUpload);
                ftp.Upload(reportImage.FileFullName,reportImage.Blh,reportImage.Txml,out errUpload);

                if (errUpload != "OK")
                {
                    MessageBox.Show($"上传图片发生错误,终止上传,文件名{reportImage.FileFullName},错误信息:" + errUpload);
                    return;
                }

                //先清空二代测序图片,再插入t_tx

                #region sql
                
                string sql =
                    $@"INSERT INTO [dbo].[T_TX]
                       ([F_BLH]
                       ,[F_TXM]
                       ,[F_TXSM]
                       ,[F_SFDY]
                       ,[F_TXLB])
                 VALUES
                       ('{reportImage
                        .Blh}'
                       ,'{reportImage.FileSafeName}'
                       ,'二代测序上传'
                       ,'1'
                       ,'' )";

                #endregion
                aa.ExecuteSQL(sql);
            }

            //上传成功后删除本地图片
            reportImages.ForEach(o=>File.Delete(o.FileFullName));

            MessageBox.Show("上传成功!");

            //刷新待上传列表
            GetImages();
        }

        private void btnOpenPicFolder_Click(object sender, EventArgs e)
        {
            if(Directory.Exists(folderPath))
                System.Diagnostics.Process.Start("ExpLorer", folderPath);
            else
            {
                MessageBox.Show($"文件夹:{folderPath}不存在!");
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            GetImages();
        }
    }

    public class ReportImage
    {
        [DisplayName("文件全名")]
        public string FileSafeName { get; set; }
        [DisplayName("文件名")]
        public string FileFullName { get; set; }
        [DisplayName("患者姓名")]
        public string Xm { get; set; }
        [DisplayName("分子编号")]
        public string Blh { get; set; }
        [DisplayName("图像序号")]
        public int Txxh { get; set; }
        [DisplayName("是否上传")]
        public bool IsUpload { get; set; } = true;
        public string Txml { get; set; }
    }
}