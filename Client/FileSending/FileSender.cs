using Client.FileInUseCheck;
using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace Client.FileSending
{
    public class FileSender : IFileSender
    {

        private readonly IXmlService proxy;
        private readonly IFileInUseChecker fileInUseChecker;
        private string folder;

        public FileSender(IXmlService proxy, IFileInUseChecker fileInUseChecker)
        {
            this.proxy = proxy;
            this.fileInUseChecker = fileInUseChecker;
            this.folder = ConfigurationManager.AppSettings["CSVFolder"];
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }


        public void SendFile(string filePath)
        {
            Dictionary<string, MemoryStream> result = proxy.SendFile(GetMemoryStream(filePath), Path.GetFileName(filePath));
            if (result != null)
            {
                foreach (var item in result)
                {
                    try
                    {
                        SaveCSV(item.Key, item.Value);

                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.Message);
                    }

                }
            }
            else
            {
                Console.WriteLine($"Nevalidna datoteka {filePath} ");
            }
        }

        private void SaveCSV(string name, MemoryStream memoryStream)
        {
            try
            {
                Console.WriteLine("Kreirana je datoteka: " + Path.Combine(folder, name));
                using (FileStream fileStream = new FileStream(Path.Combine(folder, name), FileMode.Create))
                {
                    memoryStream.Position = 0;

                    memoryStream.CopyTo(fileStream);
                }
            }
            finally
            {
                memoryStream.Dispose();
            }
        }

        private MemoryStream GetMemoryStream(string filePath)
        {
            MemoryStream ms = new MemoryStream();
            if (fileInUseChecker.IsFileInUse(filePath))
            {
                Console.WriteLine($"Cannot process the file {Path.GetFileName(filePath)}. It's being in use by another process or it has been deleted.");
                return ms;
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fileStream.CopyTo(ms);
                fileStream.Close();
            }
            return ms;
        }
    }
}
