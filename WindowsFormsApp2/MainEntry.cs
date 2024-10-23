using Azure.Core;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Task.Factory.StartNew(() => fun());
        }
        private async void fun()
        {
            AzureClient azureClient = await AzureClient.InitializeAzureClientAsync(
    new AzureLocation("centraluseuap"),
    "PortalBVT");
            var groupList = await AzureClient.GetGroupLis();
            listBox1.Invoke(new Action(() => {
                listBox1.Items.Clear();
                foreach (var item in groupList)
                {
                    listBox1.Items.Add(item);
                }
            }));
        }

        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem.ToString() == "Waiting results...") return;
            listBox2.Items.Clear();
            //MessageBox.Show(listBox1.SelectedItem.ToString());
            try
            {
                if ((await AzureClient.GetCacheByGroup(listBox1.SelectedItem.ToString())).Count == 0)
                {
                    MessageBox.Show("There is no any caches in this group");
                }
                else
                {
                    foreach (var s in await AzureClient.GetCacheByGroup(listBox1.SelectedItem.ToString()))
                    {
                        Console.WriteLine(s);
                        listBox2.Items.Add(s);
                    }
                }
            }
            catch
            {

                MessageBox.Show("Deleting in processing, pls try later");
            }
        }


        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void ChangeJsonFiles_Click(object sender, EventArgs e)
        {
            JsonChange form1 = new JsonChange();
            this.Hide();
            form1.ShowDialog();
            this.Close();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            Azure_LoadTestCache c1 = new Azure_LoadTestCache();

            Task case1 = c1.Case1();
            Task case2 = c1.Case2();
            Task case3 = c1.Case3();
            MessageBox.Show("正在进行创建");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            PerformanceCacheCreate c1 = new PerformanceCacheCreate();
            Task case1 = c1.Case1();
            Task case2 = c1.Case2();
            Task case3 = c1.Case3();
            MessageBox.Show("正在进行创建");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Alt_File_change.FileChange();
            MessageBox.Show("修改完成！");
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            PerformanceFile_Modification performanceFile_Modification = new PerformanceFile_Modification();
            this.Hide();
            performanceFile_Modification.ShowDialog();
            this.Close();
        }
    }
}
