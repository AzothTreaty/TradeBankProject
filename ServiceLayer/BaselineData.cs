using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace TradeBank3.ServiceLayer
{
    
    public class BaselineData
    {
        public static void StoreMe()
        {
            XmlDocument xmlFile = new XmlDocument();

            xmlFile.LoadXml("BaselineData.xml");

            XmlNode rootNode = xmlFile.SelectSingleNode("baseline");
            rootNode.AppendChild(xmlFile.CreateNode(XmlNodeType.Element, "sgdToUsdBaseline", "TradeBank3.ServiceLayer")).InnerText = "" + defaultValue;

            xmlFile.Save("BookStore.xml");
        }
        public const double defaultValue = -1.0;
        public static double sgdToUsdBaseline = defaultValue;
        public static double usdToSgdBaseline = defaultValue;
        public static double sgdToGbpBaseline = defaultValue;
        public static double gbpToSgdBaseline = defaultValue;
        public static double usdToGbpBaseline = defaultValue;
        public static double gbpToUsdBaseline = defaultValue;
        public static bool hasValues = false;
    }
}
