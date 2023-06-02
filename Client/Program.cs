using Client.FileInUseCheck;
using Client.FileSending;
using Client.Uploader;
using Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string folder = ConfigurationManager.AppSettings["XMLFolder"];
            string fileName = ConfigurationManager.AppSettings["XMLName"];
            if (folder == null && fileName == null)
            {
                Console.WriteLine("Invalid confugiration");
                Console.ReadLine();
                return;
            }
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            ChannelFactory<IXmlService> factory = new ChannelFactory<IXmlService>("XmlService");
            IXmlService proxy = factory.CreateChannel();
            using (EventUploader eventUploader = new EventUploader(new FileSender(proxy, new FileInUseCommonChecker()), folder, fileName))
            {
                eventUploader.Start();
                if (File.Exists(Path.Combine(folder, fileName)))
                {
                    eventUploader.SendFile(Path.Combine(folder, fileName));
                }
                Console.WriteLine("Client is running. Press any key to exit.");
                Console.ReadLine();
            }
        }
    }
}
