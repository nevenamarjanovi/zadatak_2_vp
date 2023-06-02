using Common;
using Common.Models;
using DBManipulation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Server
{
    internal class XmlService : IXmlService
    {
        delegate Dictionary<string, MemoryStream> SaveToCSV(List<Load> loads);
        SaveToCSV Save;
        public XmlService()
        {
            if (GetFileMode() == FileMode.Single)
                Save = new SaveToCSV(CSVHandler.SaveLoadsSingle);
            else
                Save = new SaveToCSV(CSVHandler.SaveLoadsMultiple);
        }
        private static FileMode GetFileMode()
        {
            string fileModeStr = ConfigurationManager.AppSettings["fileMode"];
            if (!Enum.TryParse(fileModeStr, out FileMode fileMode))
            {
                fileMode = FileMode.Single;
            }
            return fileMode;
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public Dictionary<string, MemoryStream> SendFile(MemoryStream ms, string fileName)
        {
            ConversionResult result = XMLConverter.ParseXML(ms);
            foreach (var audit in result.Audits)
            {
                AuditDatabase.Add(audit);
            }
            foreach (var load in result.Loads)
            {
                LoadDatabase.Add(load);
            }
            ImportedFileDatabase.Add(new ImportedFile { FileName = fileName });
            return Save(result.Loads);
        }

    }
}
