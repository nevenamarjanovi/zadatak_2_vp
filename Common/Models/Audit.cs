using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Audit
    {
        private static long IdGenerator = 1;
        public Audit()
        {
            Id = IdGenerator++;
        }

        public long Id { get; private set; }
        public DateTime Timestamp { get; set; }
        public MessageType MessageType { get; set; }
        public string Message { get; set; }
    }
}
