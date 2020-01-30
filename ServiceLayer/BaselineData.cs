using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeBank3.ServiceLayer
{
    
    public class BaselineData
    {
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
