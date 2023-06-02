using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ConversionResult
    {
        public List<Load> Loads = new List<Load>();
        public List<Audit> Audits = new List<Audit>();
    }
}
