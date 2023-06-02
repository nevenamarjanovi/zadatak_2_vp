using Client.FileSending;
using System;
using System.IO;

namespace Client.Uploader
{
    public class EventUploader : IDisposable
    {
        private FileSystemWatcher inputFileWatcher;
        private readonly IFileSender fileSender;
        private readonly string folder;
        private readonly string fileName;

        public EventUploader(IFileSender fileSender, string folder, string fileName)
        {
            this.fileSender = fileSender;
            this.folder = folder;
            this.fileName = fileName;
        }

        private void SendFileEvent(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath;
            SendFile(filePath);
        }
        public void SendFile(string filePath)
        {
            try
            {
                fileSender.SendFile(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        internal void Start()
        {
            inputFileWatcher = new FileSystemWatcher(folder)
            {
                IncludeSubdirectories = false,
                InternalBufferSize = 32768,
                Filter = fileName,
                NotifyFilter = NotifyFilters.LastWrite,
                EnableRaisingEvents = true
            };
            inputFileWatcher.Created += SendFileEvent;
            inputFileWatcher.Changed += SendFileEvent;

        }

        public void Dispose()
        {
            inputFileWatcher.Dispose();
        }
    }
}
