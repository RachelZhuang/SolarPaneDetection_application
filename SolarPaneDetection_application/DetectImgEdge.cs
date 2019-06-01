using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenCvSharp;

namespace SolarPaneDetection_application
{
    public partial class DetectImgEdge : Form
    {
         public DetectImgEdge()
        {
            InitializeComponent();
        }
        public DetectImgEdge(IList<string> uavFilelist)
        {
            InitializeComponent();
            comboBox1.DataSource = uavFilelist;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = comboBox1.Text;
            //Mat src = new Mat(path, ImreadModes.Grayscale);

            //Mat dst = new Mat();

            //Cv2.Canny(src, dst, 50, 200);
            //using (new Window("src image", src))
            //using (new Window("dst image", dst))
            //{
            //    Cv2.WaitKey();
            //}
            string exe = @"C:/Users/Administrator/AppData/Local/Programs/Python/Python37/python.exe";
            string dosCommand = @"C:/Users/Administrator/PycharmProjects/test/test.py";
            dosCommand = dosCommand + " " + path;
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
