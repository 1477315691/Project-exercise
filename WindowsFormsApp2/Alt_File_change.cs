using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    internal class Alt_File_change
    {
        public static void FileChange()
        {

            string filePath = "C:\\Users\\SSA-User\\Desktop\\ALT-AzureLoadTestingPlan\\P1\\PerfTestPlan.jmx";
            string extractedValue = string.Empty;
            try
            {
                // 读取文件内容
                string content = File.ReadAllText(filePath);

                // 定义正则表达式以匹配指定格式的字符串
                string pattern = @"alt-eus2e-P1-(\d{4})\.redis\.cache\.windows\.net";

                // 查找第一个匹配的字符串
                Match match = Regex.Match(content, pattern);

                if (match.Success)
                {
                    // 提取并打印出匹配的部分
                    extractedValue = match.Groups[1].Value;
                }
                else
                {
                    Console.WriteLine("没有找到匹配的字符串");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生错误: {ex.Message}");
            }

            DateTime currentDate = DateTime.Now;
            string formattedDate = currentDate.ToString("MMdd");
            string oldValue = extractedValue + ".redis.cache.windows.net";
            string newValue = formattedDate + ".redis.cache.windows.net";

            for (int j = 1; j <= 5; j++)
            {
                //修改P文件
                string filePath_1 = "C:\\Users\\SSA-User\\Desktop\\ALT-AzureLoadTestingPlan\\P" + j + "\\redis-stresstestitr111-9462.properties";
                string filePath_2 = "C:\\Users\\SSA-User\\Desktop\\ALT-AzureLoadTestingPlan\\P" + j + "\\PerfTestPlan.jmx";

                ReplaceInFile(filePath_1, oldValue, newValue);
                ReplaceInFile(filePath_2, oldValue, newValue);
            }

            for (int j = 0; j <= 6; j++)
            {
                ////修改B-C文件
                ///"C:\Users\SSA-User\Desktop\ALT-AzureLoadTestingPlan\SC1\PerfTestPlan.jmx"
                string filePath_1 = "C:\\Users\\SSA-User\\Desktop\\ALT-AzureLoadTestingPlan\\BC" + j + "\\redis-stresstestitr111-9462.properties";
                string filePath_2 = "C:\\Users\\SSA-User\\Desktop\\ALT-AzureLoadTestingPlan\\BC" + j + "\\PerfTestPlan.jmx";
                //修改S-C文件
                //"C:\Users\SSA-User\Desktop\ALT-AzureLoadTestingPlan\BC0\PerfTestPlan.jmx"
                string filePath_3 = "C:\\Users\\SSA-User\\Desktop\\ALT-AzureLoadTestingPlan\\SC" + j + "\\redis-stresstestitr111-9462.properties";
                string filePath_4 = "C:\\Users\\SSA-User\\Desktop\\ALT-AzureLoadTestingPlan\\SC" + j + "\\PerfTestPlan.jmx";

                ReplaceInFile(filePath_1, oldValue, newValue);
                ReplaceInFile(filePath_2, oldValue, newValue);
                ReplaceInFile(filePath_3, oldValue, newValue);
                ReplaceInFile(filePath_4, oldValue, newValue);

            }
        }
        static void ReplaceInFile(string filePath, string oldValue, string newValue)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                // 对每一行进行处理
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].Contains(oldValue))
                    {

                        lines[i] = lines[i].Replace(oldValue, newValue);
                    }
                }

                // 使用 StreamWriter 重写整个文件
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        sw.WriteLine(lines[i]);
                    }
                }

                Console.WriteLine("文件内容修改完成。");
            }
            catch (Exception ex)
            {
                Console.WriteLine("操作文件出错: " + ex.Message);
            }
        }
        }
    }
