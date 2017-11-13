using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace File2Base64
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //textBox2.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;

            var r = openFileDialog1.ShowDialog();
            if (r != DialogResult.OK)
                return;

            string path = openFileDialog1.FileName; 
            FileStream filestream = new FileStream(path, FileMode.Open);


            byte[] bt = new byte[filestream.Length];

            //调用read读取方法  
            filestream.Read(bt, 0, bt.Length);
            var base64Str = Convert.ToBase64String(bt, Base64FormattingOptions.None);
            filestream.Close();

            textBox2.Text = base64Str;
            textBox2.Focus();
            textBox2.SelectAll();
        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();
            var r = d.ShowDialog();
            if (r != DialogResult.OK) return;

            var base64 = textBox2.Text;
            var contents = Convert.FromBase64String(base64);
            var outPath = d.FileName;


            using (var fs = new FileStream(outPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(contents, 0, contents.Length);
                fs.Flush();
            }

            var mr = MessageBox.Show("转换成功,是否打开文件?","提示", MessageBoxButtons.YesNo);
            if (mr == DialogResult.Yes)
                Process.Start(d.FileName);
        }
    }
}
