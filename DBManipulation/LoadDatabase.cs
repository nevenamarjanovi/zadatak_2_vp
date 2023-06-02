using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManipulation
{
    public static class LoadDatabase
    {
        private static Dictionary<long, Load> loads = new Dictionary<long, Load>();
        private static object obj = new object();
        public static bool Add(Load load)
        {
            lock (obj)
            {
                if (loads.ContainsKey(load.Id))
                {
                    return false;
                }
                if (loads.Values.Any(x => x.Timestamp == load.Timestamp))
                    return false;
                loads.Add(load.Id, load);
                return true;
            }
        }
        public static List<Load> Get()
        {
            return loads.Values.ToList();
        }

    }
}
