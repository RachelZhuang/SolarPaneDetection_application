using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace SolarPaneDetection_application
{
    public partial class PreProcess : Form
    {
        public PreProcess()
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
            Thread fThread = new Thread(new ThreadStart(RunPythonScript));
            fThread.Start();
        }
        //调用python核心代码
        public void RunPythonScript()
        {

            Process p = new Process();

            string path = @"E:/Project/python_/sentinel/auto_downloading.py";//(因为我没放debug下，所以直接写的绝对路径,替换掉上面的路径了)
            p.StartInfo.FileName = @"D:/Anaconda/python.exe";//没有配环境变量的话，可以像我这样写python.exe的绝对路径。如果配了，直接写"python.exe"即可


            p.StartInfo.Arguments = path;

            p.StartInfo.UseShellExecute = false;

            p.StartInfo.RedirectStandardOutput = true;

            p.StartInfo.RedirectStandardInput = true;

            p.StartInfo.RedirectStandardError = true;

            p.StartInfo.CreateNoWindow = true;

            p.Start();
            p.BeginOutputReadLine();
            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);

            //Console.ReadLine();
            p.WaitForExit();
            p.Close();


        }
        //输出打印的信息
        public void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                AppendText(e.Data);
            }
        }
        public delegate void AppendTextCallback(string text);
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }
        public static int prev_pos = 0, cur_pos = 0;

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        public void AppendText(string text)
        {

            Console.WriteLine(text);     //此处在控制台输出.py文件print的结果   
            //从输出文本中提取数字           
            if (IsNumeric(text))
            {
                prev_pos = cur_pos;
                cur_pos = int.Parse(text);
            }
            text = text + Environment.NewLine;
            SetTextMesssage(cur_pos, text);

        }
    }
}
