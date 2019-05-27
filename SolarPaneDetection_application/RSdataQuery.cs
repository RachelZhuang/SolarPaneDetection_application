using System;
using System.Data;
using System.Windows.Forms;

namespace SolarPaneDetection_application
{
    public partial class RSdataQuery : DevExpress.XtraEditors.XtraForm
    {
        public RSdataQuery()
        {
            InitializeComponent();
            dataGridView1.DataSource = MySqlHelper.GetDataSet(MySqlHelper.Conn, CommandType.Text, "select * from data_tif", null).Tables[0].DefaultView;
            
        }

        public Action<string> clickAddTiff;

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string selectDate = this.dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
            string selectLocation = this.comboBox1.SelectedItem.ToString();
            string SearchshpCmd = "SELECT * FROM data_tif WHERE filedate like '" + selectDate + "%' AND filelocation like '" + selectLocation + "%' ";
            dataGridView1.DataSource = MySqlHelper.GetDataSet(MySqlHelper.Conn, CommandType.Text, SearchshpCmd, null).Tables[0].DefaultView;       
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            int selectCellIndex = this.dataGridView1.SelectedRows[0].Cells[0].RowIndex;
            string selectFilePath = this.dataGridView1.Rows[selectCellIndex].Cells[2].Value.ToString();
            string selectFileName = this.dataGridView1.Rows[selectCellIndex].Cells[3].Value.ToString();
            selectFileName = selectFilePath + @"\" + selectFileName;
            this.dateTimePicker1.Text = this.dataGridView1.Rows[selectCellIndex].Cells[1].Value.ToString();
            if (clickAddTiff != null)//判断事件是否为空
             {
               clickAddTiff(selectFileName);//执行委托实例  
               this.Close();
             }
             
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}