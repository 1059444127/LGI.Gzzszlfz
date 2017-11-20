using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PathnetCAzgq
{
    public partial class Frm_XMZSYY_Login : Form
    {
        public Frm_XMZSYY_Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            pwd = txtpwd.Text.Trim();
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }
        string pwd = string.Empty;
        public string password
        {
            get { return pwd; }
        }
        public bool savepwd
        {
            get { return checkBox1.Checked; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
      
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void Frm_Login_FormClosing(object sender, FormClosingEventArgs e)
        {
          
        }

        private void Frm_Login_Load(object sender, EventArgs e)
        {
            txtpwd.Focus();
        }
    }
}
