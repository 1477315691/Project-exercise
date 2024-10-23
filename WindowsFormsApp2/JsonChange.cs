using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp2
{
    public partial class JsonChange : Form
    {
        string date;
        public JsonChange()
        {
            InitializeComponent();
            DateTime currentDate = DateTime.Now;
            int month = currentDate.Month;
            int day = currentDate.Day;
            date = "" + month + day;
        }
        public void DetailsChange(string filePath)
        {
            try
            {
                var random = RandomList.GenerateRandomLetters(2);
                string[] lines = File.ReadAllLines(filePath);
                string region = "";
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains("East US 2 EUAP"))
                    {
                        region = "eus2euap";
                    }
                    if (lines[i].Contains("Central US EUAP"))
                    {
                        region = "cuseuap";
                    }
                    if (lines[i].Contains("StorageAccountName"))
                    {
                        lines[i] = "  \"StorageAccountName\": \"san" + region + date + random + "\",";
                    }
                    if (lines[i].Contains("VNetName"))
                    {
                        lines[i] = "  \"VNetName\": \"vnet" + region + date + random + "\",";
                    }

                    File.WriteAllLines(filePath, lines);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }
        private void SelecJsonChangeBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] filePaths = openFileDialog.FileNames;
                    foreach (string file in filePaths)
                    {
                        DetailsChange(file);
                    }
                    MessageBox.Show("success!");
                }
            }
        }

        private void AllJsonChangeBtn_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string folderPath = folderDialog.SelectedPath;
                    List<string> txtFiles = Directory.GetFiles(folderPath, "*.json", SearchOption.AllDirectories).ToList();

                    foreach (string file in txtFiles)
                    {
                        DetailsChange(file);
                    }
                    MessageBox.Show("success!");
                }
            }
        }

        private void ChageRegion_Click(object sender, EventArgs e)
        {
            if (this.RegionCcb.SelectedItem == "")
            {
                MessageBox.Show("Region is not null");
            }
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Multiselect = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] filePaths = openFileDialog.FileNames;
                    foreach (string file in filePaths)
                    {
                        string[] lines = File.ReadAllLines(file);
                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (lines[i].Contains("Region"))
                            {
                                lines[i] = "  \"Region\": \"" + this.RegionCcb.SelectedItem + "\",";
                            }
                        }
                        File.WriteAllLines(file, lines);
                    }
                    MessageBox.Show("success!");
                }
            }
        }

        private void returnBtn_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            this.Hide();
            f1.ShowDialog();
        }
    }
}
