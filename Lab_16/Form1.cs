using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_16
{
    public partial class Form1 : Form
    {
        private DataStorage data;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            openFileDlg.InitialDirectory = Application.StartupPath;
            if(openFileDlg.ShowDialog() == DialogResult.OK)
            {
                ShowData(openFileDlg.FileName);
            }
        }

        private void ShowData(String datapath)
        {
            try
            {
                data = DataStorage.DataCreator(datapath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("При загрузке данных что-то сломалось");
            }

            dgvRaw.DataSource = data.GetRawData();
            dgvRaw.ReadOnly = true;
            dgvAverage.DataSource = data.GetAverageData();
            dgvAverage.ReadOnly = true;
        }
    }
}
