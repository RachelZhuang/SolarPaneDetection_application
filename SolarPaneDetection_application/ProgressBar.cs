﻿using System;
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
<<<<<<< HEAD
            string exe = @"D:/Anaconda/python.exe";
            string dosCommand = @"E:/Project/matlab_/mean_std_texture/histogram_contras.py";
=======
            string exe = @"C:/Users/Administrator/AppData/Local/Programs/Python/Python37/python.exe";
            string dosCommand = @"C:/Users/Administrator/PycharmProjects/test/test.py";
>>>>>>> 62c1442b6c41e9b830dcca4782b308d13a1658af
            string output = DosCommandOutput.Execute(exe,dosCommand);
        }
       
    }
}
