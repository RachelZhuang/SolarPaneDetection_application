using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SolarPaneDetection_application
{
    public partial class BuildingExtraction : Form
    {
        public BuildingExtraction()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string exe = @"D:\Anaconda\python.exe";
            string dosCommand = @"D:\Download\SolarPaneDetection_application-master\SolarPaneDetection_application-master\SolarPaneDetection_application\test.py";
            dosCommand = dosCommand + " " + comboBox1.Text;

            string output = DosCommandOutput.Execute(exe, dosCommand);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jpg文件|*.jpg|所有文件|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                comboBox1.Text = openFileDialog.FileName;

            }
        }
    }
}
