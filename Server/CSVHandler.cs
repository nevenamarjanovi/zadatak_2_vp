using Common.Models;
using DBManipulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Server
{
    public static class CSVHandler
    {
        public static Dictionary<string, MemoryStream> SaveLoadsMultiple(List<Load> loads)
        {
            try
            {
                Dictionary<string, MemoryStream> result = new Dictionary<string, MemoryStream>();
                loads = loads.OrderBy(x => x.Timestamp).ToList();//sortiranje po datumu i vremenu
                DateTime? prevDate = null;
                List<Load> list = new List<Load>();
                foreach (var load in loads)
                {
                    if (prevDate == null)
                    {
                        list.Add(load);
                    }
                    else
                    {
                        if (prevDate.Value.Date == load.Timestamp.Date)
                        {
                            list.Add(load);
                        }
                        else
                        {
                            string fileName = "result_data_" + prevDate.Value.ToString("yyyy_MM_dd") + ".csv";
                            byte[] bytes = Encoding.UTF8.GetBytes(GenerateCSV(list));
                            MemoryStream memoryStream = new MemoryStream(bytes);
                            result.Add(fileName, memoryStream);
                            list.Clear();
                            list.Add(load);
                        }
                    }
                    prevDate = load.Timestamp;
                }
                if (list.Any())
                {
                    string fileName = "result_data_" + prevDate.Value.ToString("yyyy_MM_dd") + ".csv";
                    byte[] bytes = Encoding.UTF8.GetBytes(GenerateCSV(list));
                    MemoryStream memoryStream = new MemoryStream(bytes);
                    result.Add(fileName, memoryStream);
                    list.Clear();
                }
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static Dictionary<string, MemoryStream> SaveLoadsSingle(List<Load> loads)
        {
            try
            {
                Dictionary<string, MemoryStream> result = new Dictionary<string, MemoryStream>();
                ImportedFileDatabase.Add(new ImportedFile { FileName = "result_data.csv" });
                byte[] bytes = Encoding.UTF8.GetBytes(GenerateCSV(loads));
                MemoryStream memoryStream = new MemoryStream(bytes);
                result.Add("result_data.csv", memoryStream);
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static string GenerateCSV(List<Load> loads)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DATE;TIME;FORECAST_VALUE;MEASURED_VALUE");
            foreach (Load load in loads)
            {
                sb.AppendLine($"{load.Timestamp.ToString("yyyy-MM-dd;HH:mm")};{load.ForecastValue.ToString(CultureInfo.InvariantCulture)};{load.MeasuredValue.ToString(CultureInfo.InvariantCulture)}");
            }
            return sb.ToString();
        }
    }
}
