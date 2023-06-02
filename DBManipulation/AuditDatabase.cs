using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManipulation
{
    public static class AuditDatabase
    {
        private static Dictionary<long, Audit> audits = new Dictionary<long, Audit>();
        private static object obj = new object();
        public static bool Add(Audit audit)
        {
            lock (obj)
            {
                if (audits.ContainsKey(audit.Id))
                {
                    return false;
                }
                audits.Add(audit.Id, audit);
                return true;
            }
        }
        public static List<Audit> Get()
        {
            return audits.Values.ToList();
        }

    }
}
