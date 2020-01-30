using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace TradeBank3.ServiceLayer
{
    
    public class BaselineData
    {
        public const double defaultValue = -1.0;
        public double sgdToUsdBaseline { get; set; }
        public double usdToSgdBaseline { get; set; }
        public double sgdToGbpBaseline { get; set; }
        public double gbpToSgdBaseline { get; set; }
        public double usdToGbpBaseline { get; set; }
        public double gbpToUsdBaseline { get; set; }
        public bool hasValues { get; set; }

        public BaselineData()
        {
            sgdToUsdBaseline = defaultValue;
            usdToSgdBaseline = defaultValue;
            sgdToGbpBaseline = defaultValue;
            gbpToSgdBaseline = defaultValue;
            usdToGbpBaseline = defaultValue;
            gbpToUsdBaseline = defaultValue;
            hasValues = false;
        }
    }
}
