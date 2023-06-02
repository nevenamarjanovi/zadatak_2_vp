using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common
{
    [ServiceContract]
    public interface IXmlService
    {
        [OperationContract]
        Dictionary<string, MemoryStream> SendFile(MemoryStream ms, string fileName);
    }
}
