using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Lg.Pdf2Jpg;

namespace BatchPdf2Jpeg
{
    public partial class Form1 : Form
    {
        private string[] _fileNames;
        BackgroundWorker asyncConvertWorker = new BackgroundWorker();
        private Converer.Definition _definition;
        private string _outPath;

        public Form1()
        {
            InitializeComponent();


            asyncConvertWorker.DoWork += AsyncConvertWorkerDoWork;

            asyncConvertWorker.RunWorkerCompleted += (sender, args) =>
            {
                lblWorkStatus.Text = "转换完成!";
            };
        }

        private void btnSelectPdfs_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "PDF(*.pdf)|*.pdf";
            d.Multiselect = true;
            var r = d.ShowDialog();
            if (r == DialogResult.Cancel) return;
            var fileNames = d.FileNames;
            MessageBox.Show("点确定开始转换,转换时程序可能会假死,请不要关闭.");

            if (d.FileNames != null)
            {
                _definition = (Converer.Definition)int.Parse(cmbQuality.Text);
                _fileNames = fileNames;
                _outPath = txtOutPath.Text;
                lblWorkStatus.Text = "转换中,请不要操作..";
                asyncConvertWorker.RunWorkerAsync();
            }

            MessageBox.Show("转换完成!");
        }


        private void btnSelectDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            var r = d.ShowDialog();
            if (r == DialogResult.Cancel) return;

            //获取所有文件
            List<string> fileNames = new List<string>();

            GetPdfFileName(d.SelectedPath, fileNames);

            MessageBox.Show($"共[{fileNames.Count}]个文件,点确定开始转换,转换时程序可能会假死,请不要关闭.");

            _definition = (Converer.Definition)int.Parse(cmbQuality.Text);
            _fileNames = fileNames.ToArray();
            _outPath = txtOutPath.Text;
            lblWorkStatus.Text = "转换中,请不要操作..";
            asyncConvertWorker.RunWorkerAsync();

      //      MessageBox.Show("转换完成!");
        }

        private void AsyncConvertWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            Convert();
        }

        private static void GetPdfFileName(string dir, List<string> fileNames)
        {
            foreach (string fileName in Directory.GetFiles(dir))
            {
                if (Path.GetExtension(fileName).ToUpper() == ".PDF")
                {
                    fileNames.Add(fileName);
                }
            }
            foreach (string directory in Directory.GetDirectories(dir))
            {
                GetPdfFileName(directory, fileNames);
            }
        }


        private void Convert()
        {
            for (int i = 0; i < _fileNames.Length; i++)
            {
                var outPath = _outPath;
                if (string.IsNullOrEmpty(_outPath.Trim()) && _fileNames.Length > 0)
                    outPath = _fileNames[i].Substring(0, _fileNames[i].LastIndexOf(@"\") + 1);
                string fileName = _fileNames[i];
                string safeName = Path.GetFileNameWithoutExtension(_fileNames[i]);
                Converer.ConvertPDF2Image(fileName, outPath, safeName, 1, 100, ImageFormat.Jpeg, _definition, true);

               // asyncConvertWorker.ReportProgress((int)(i/_fileNames.Length));
            }
        }

        private void btnSelectOutPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            var r = d.ShowDialog();
            if (r == DialogResult.Cancel) return;

            txtOutPath.Text = d.SelectedPath;
        }
    }
}