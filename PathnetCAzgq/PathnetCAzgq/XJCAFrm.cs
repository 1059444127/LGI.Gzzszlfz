using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PathnetCAzgq
{
    public partial class XJCAFrm : Form
    {
         string m_strSIGNXML = "";
         string m_strMsg = "";
         string wy = "";
        public String StrSIGNXML
        {
            get { return m_strSIGNXML; }
        }
        public String StrMsg
        {
            get { return m_strMsg; }
        }
        public XJCAFrm(string bgyw)
        {
            wy = bgyw;
            InitializeComponent();
        }
        private void XJCAFrm_Load(object sender, EventArgs e)
        {
            m_strSIGNXML = "";
            m_strMsg = "";
            try
            {
                axXJFormSealX1.XJCASowSignInSvr(wy, out m_strSIGNXML);
                if (m_strSIGNXML != null)
                {
                    this.DialogResult = DialogResult.Yes;
                }
                else
                {
                    m_strSIGNXML = "";
                    m_strMsg = axXJFormSealX1.XJCAError;
                    this.DialogResult = DialogResult.No;
                }
            }
            catch
            {
                this.DialogResult = DialogResult.No;
            }
            this.Close();
        }


    }
}
