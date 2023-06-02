using System;
using System.ServiceModel;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost svc = new ServiceHost(typeof(XmlService)))
            {
                svc.Open();
                Console.WriteLine("Pritisnite [Enter] za zaustavljanje servisa.");
                Console.ReadLine();
            }

        }
    }
}
