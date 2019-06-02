using System;
using System.Windows.Forms;

namespace SolarPaneDetection_application
{
    public partial class PreProcess : Form
    {
        public PreProcess()
        {
            InitializeComponent();
        }  

        private void button1_Click(object sender, EventArgs e)
        {
            string exe = @"D:/Anaconda/python.exe";
            string dosCommand = @"E:/Project/matlab_/mean_std_texture/histogram_contras.py";
            string output = DosCommandOutput.Execute(exe, dosCommand);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
