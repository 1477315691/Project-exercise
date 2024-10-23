using Azure;
using Azure.Core;
using Azure.ResourceManager.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Performance_GetKey : Form
    {
        string path = "";
        string date = "";
        string select_type = "";
        public Performance_GetKey()
        {
            InitializeComponent();
        }

        private void FileBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog.FileName;
                    this.FileBtn.Text = openFileDialog.FileName;
                }
            }
        }
        public async Task GetKeysAsync()
        {
            //string group = this.groupText.Text;
            string group = "Cache-FunctionalRun-neverdeletedPatching";
            AzureClient azureClient = await AzureClient.InitializeAzureClientAsync(new AzureLocation("centraluseuap"), group);
            RedisCollection cachecollection = azureClient.RedisCollection;
            AsyncPageable<RedisResource> redisResourcesAsyncPageable = cachecollection.GetAllAsync();
            await foreach (var cache in redisResourcesAsyncPageable)
            {
                if (cache.Data.Name.Contains(date))
                {

                    if (select_type == "1")
                    {
                        //Primarykey
                        File.AppendAllText(path, "" + cache.Data.HostName + ":6379,password=" + cache.GetKeys().Value.PrimaryKey + ",ssl=False\n ");
                        File.AppendAllText(path, "" + cache.Data.HostName + ":6380,password=" + cache.GetKeys().Value.PrimaryKey + ",ssl=True\n ");
                    }
                    if (select_type == "2")
                    {
                        ///插入数据
                        if (cache.Data.Name.Contains("Verifyperformance-P"))
                        {
                            File.AppendAllText(path, "redis-fill.exe -h " + cache.Data.HostName + " -a " + cache.GetKeys().Value.PrimaryKey + " -m 1\n");
                            File.AppendAllText(path, Environment.NewLine);
                        }
                        else
                            File.AppendAllText(path, "redis-fill.exe -h " + cache.Data.HostName + " -a " + cache.GetKeys().Value.PrimaryKey + " -m 0.5\n");
                        File.AppendAllText(path, Environment.NewLine);
                    }
                    if (select_type == "3")
                    {
                        //Performance
                        File.AppendAllText(path, cache.Data.HostName + " " + cache.GetKeys().Value.PrimaryKey + "\n");
                    }
                }
            }

            //对文件内容进行排序

            string filePath = path;
            string[] lines = File.ReadAllLines(filePath);

            // 将内容解析为排序的列表
            var sortedLines = SortLines(lines);

            // 如果需要写回到文件
            File.WriteAllLines(filePath, sortedLines);


            MessageBox.Show("输出成功!");

        }
        private void PrintBtn_Click(object sender, EventArgs e)
        {
            if (this.GroupTitle.Text == "") MessageBox.Show("Rescource group is not null!");
            date = this.DateText.Text.Trim();
            select_type = this.comboBox1.Text;
            MessageBox.Show("正在输出，请稍等...");
            GetKeysAsync();
        }

        private static List<string> SortLines(string[] lines)
        {
            var sortedLines = new List<string>(lines);

            sortedLines.Sort((x, y) =>
            {
                int GetOrder(string s)
                {
                    var match = Regex.Match(s, @"(P|C|S)(\d+)");
                    if (!match.Success)
                        return int.MaxValue;

                    var prefix = match.Groups[1].Value;
                    var number = int.Parse(match.Groups[2].Value);

                    if (prefix == "P") return number;
                    if (prefix == "C") return 100 + number; // BC的范围从100开始
                    if (prefix == "S") return 200 + number; // SC的范围从200开始

                    return int.MaxValue;
                }

                return GetOrder(x).CompareTo(GetOrder(y));
            });

            return sortedLines;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
