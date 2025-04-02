using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms; // 用于处理动态 JSON 对象


namespace WindowsFormsApp2.ChangeExcelfile
{
    public class ChangeFile
    {
        string filePath = "C:\\Users\\User\\Desktop\\Performance结果.txt"; // 替换为你的文件路径
        string ExcelPath = "C:\\Users\\User\\Desktop\\performance_result_111.xlsx";
        int startRow = 3; // M3 -> 3          // 起始列
        int startColumn = 13; // M -> 13
        public List<string> OutputData { get; private set; } = new List<string>();
        public void ProcessFile()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 设置 Excel 包的许可证上下文，EPPlus 要求设置此项

            try
            {
                // 读取文件的所有行
                string[] lines = File.ReadAllLines(filePath);

                // 用于存储当前 JSON 字符串
                string currentJson = string.Empty;
                string currentFileName = string.Empty;

                foreach (string line in lines)
                {
                    // 如果行以 "Results from: " 开头，表示新的一段开始
                    if (line.StartsWith("Results from:"))
                    {
                        // 如果当前 JSON 非空，则输出
                        if (!string.IsNullOrEmpty(currentJson))
                        {
                            OutputData.AddRange(CalMea.ProcessJson(currentJson, currentFileName, ExcelPath, ref startRow, startColumn));
                            OutputData.Add("数据已成功插入到 Excel 文件中！");
                        }

                        // 提取新的文件名并重置 JSON 数据
                        currentFileName = line.Substring("???: ".Length).Trim(); // 去除前后空白字符
                        currentJson = string.Empty; // 重置当前 JSON
                        if (currentFileName.Contains("results-P1"))
                        {
                            startRow = 3;
                        }
                        if (currentFileName.Contains("results-SC0"))
                        {
                            startRow = 12;
                        }
                        if (currentFileName.Contains("results-BC0"))
                        {
                            startRow = 23;
                        }
                    }
                    else
                    {
                        // 将行添加到当前 JSON 数据中
                        currentJson += line + Environment.NewLine;
                    }
                }

                // 最后一个 JSON 也需要输出
                if (!string.IsNullOrEmpty(currentJson))
                {
                    OutputData.AddRange(CalMea.ProcessJson(currentJson, currentFileName, ExcelPath, ref startRow, startColumn));
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("读取文件时出错: " + ex.Message);
                OutputData.Add("读取文件时出错: " + ex.Message);
            }

        }
       
    }
}
