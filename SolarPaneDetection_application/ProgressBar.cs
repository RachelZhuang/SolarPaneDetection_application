using System;
using System.Windows.Forms;

namespace SolarPaneDetection_application
{
    public partial class ProgressBar : Form
    {
        public ProgressBar()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string exe = @"D:/Anaconda/python.exe";
            string dosCommand = @"E:/Project/python_/sentinel/auto_downloading.py";
            string output = DosCommandOutput.Execute(exe,dosCommand);
        }
       
    }
}
