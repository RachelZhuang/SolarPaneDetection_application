using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace SolarPaneDetection_application
{
    public partial class ProgressBar : Form
    {
        public ProgressBar()
        {
            InitializeComponent();
        }
        private delegate void SetPos(int ipos, string vinfo);//代理
        private void SetTextMesssage(int ipos, string vinfo)
        {
            if (this.InvokeRequired)
            {
                SetPos setpos = new SetPos(SetTextMesssage);
                this.Invoke(setpos, new object[] { ipos, vinfo });
            }
            else
            {
                //this.label1.Text = ipos.ToString() + "/100";
                //this.progressBar1.Value = Convert.ToInt32(ipos);
                this.textBox1.AppendText(vinfo);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string exe = @"D:/Anaconda/python.exe";
            string dosCommand = @"E:/Project/matlab_/mean_std_texture/histogram_contras.py";
            string output = DosCommandOutput.Execute(exe,dosCommand);
        }
       
    }
}
