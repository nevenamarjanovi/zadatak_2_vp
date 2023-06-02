using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManipulation
{
    public static class ImportedFileDatabase
    {
        private static Dictionary<long, ImportedFile> files = new Dictionary<long, ImportedFile>();
        private static object obj = new object();
        public static bool Add(ImportedFile file)
        {
            lock (obj)
            {
                if (files.ContainsKey(file.Id))
                {
                    return false;
                }
                files.Add(file.Id, file);
                return true;
            }
        }
        public static List<ImportedFile> Get()
        {
            return files.Values.ToList();
        }

    }
}
