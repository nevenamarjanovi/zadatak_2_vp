using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ImportedFile
    {
        private static long IdGenerator = 1;
        public ImportedFile()
        {
            Id = IdGenerator++;
        }

        public long Id { get; private set; }
        public string FileName { get; set; }
    }
}
