using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;

namespace WindowsFormsApp2.ChangeExcelfile
{
    public class CalMea
    {
        public static void InsertDataToExcel(string filePath, string sheetName, int Row, int Column, double data)
        {
            // 创建文件信息对象
            FileInfo fileInfo = new FileInfo(filePath);

            // 打开 Excel 文件
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                // 获取或创建工作表
                ExcelWorksheet worksheet;
                if (package.Workbook.Worksheets[sheetName] != null)
                {
                    worksheet = package.Workbook.Worksheets[sheetName];
                }
                else
                {
                    worksheet = package.Workbook.Worksheets.Add(sheetName);
                }

                // 将数据插入到指定单元格
                worksheet.Cells[Row, Column].Value = data;

                // 保存 Excel 文件
                package.Save();
            }
            //Console.WriteLine("数据已成功插入到 Excel 文件中！");
        }
        public static List<string> ProcessJson(string json, string fileName, string excelPath, ref int startRow, int startColumn)
        {
            List<string> outputData = new List<string>();
            outputData.Add("文件名: " + fileName);
            JObject jsonObject = JsonConvert.DeserializeObject<JObject>(json);

            // 提取相关数据
            double getsRPS = jsonObject["Gets RPS"].Value<double>();
            double totalDuration = jsonObject["Total duration"].Value<double>();
            double getsp50 = jsonObject["Gets p50.00"].Value<double>();
            double getsp99 = jsonObject["Gets p99.00"].Value<double>();
            double getsp999 = jsonObject["Gets p99.90"].Value<double>();
            double getsp9999 = jsonObject["Gets p99.99"].Value<double>();

            // 输出结果
            Console.WriteLine($"Gets RPS = {getsRPS}");
            Console.WriteLine($"Total Duration = {totalDuration}");
            Console.WriteLine($"Gets p50.00 = {getsp50}");
            Console.WriteLine($"Gets p99.00 = {getsp99}");
            Console.WriteLine($"Gets p99.90 = {getsp999}");
            Console.WriteLine($"Gets p99.99 = {getsp9999}");

            // 定义要写入的数据
            double[] valuesToInsert = new double[]
            {
            getsRPS,
            totalDuration,
            getsp50,
            getsp99,
            getsp999,
            getsp9999
            };

            // 循环写入数据到对应的单元格
            for (int i = 0; i < valuesToInsert.Length; i++)
            {
                InsertDataToExcel(excelPath, "20241008", startRow, startColumn + i, valuesToInsert[i]);
            }

            startRow++; // 增加行数以准备写入下一组数据
            return outputData;
            
        }
    }
}

