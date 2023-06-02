using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Load
    {
        private static long IdGenerator = 1;
        public Load()
        {
            Id = IdGenerator++;
        }

        public long Id { get; private set; }
        public DateTime Timestamp { get; set; }
        public float ForecastValue { get; set; }
        public float MeasuredValue { get; set; }
    }
}
