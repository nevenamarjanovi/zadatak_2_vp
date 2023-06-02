using Common.Enum;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Server
{
    public class XMLConverter
    {
        public static ConversionResult ParseXML(MemoryStream memoryStream)
        {
            ConversionResult result = new ConversionResult();
            memoryStream.Position = 0;
            IEnumerable<XElement> rowElements;
            try
            {
                XDocument xdoc = XDocument.Load(memoryStream);
                rowElements = xdoc.Descendants("row");
            }
            catch (Exception e)
            {
                result.Audits.Add(new Audit { Message = e.Message, Timestamp = DateTime.Now, MessageType = MessageType.Error });
                return result;
            }
            finally
            {
                memoryStream.Dispose();
            }
            foreach (XElement rowElement in rowElements)
            {
                string timestampValue = rowElement.Element("TIME_STAMP").Value;
                string forecastValue = rowElement.Element("FORECAST_VALUE")?.Value ?? "";
                string measuredValue = rowElement.Element("MEASURED_VALUE")?.Value ?? "";
                bool validLoad = true;
                if (!DateTime.TryParse(timestampValue, out DateTime timestamp))
                {
                    result.Audits.Add(new Audit { Message = "Invalid datetime", Timestamp = DateTime.Now, MessageType = MessageType.Error });
                    validLoad = false;
                }
                if (!float.TryParse(forecastValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float forecast))
                {
                    result.Audits.Add(new Audit { Message = "Invalid forecast value", Timestamp = DateTime.Now, MessageType = MessageType.Error });
                    validLoad = false;
                }
                if (!float.TryParse(measuredValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float measured))
                {
                    result.Audits.Add(new Audit { Message = "Invalid measured value", Timestamp = DateTime.Now, MessageType = MessageType.Error });
                    validLoad = false;
                }
                if (validLoad)
                    result.Loads.Add(new Load { ForecastValue = forecast, MeasuredValue = measured, Timestamp = timestamp });

            }
            return result;
        }


    }
}
