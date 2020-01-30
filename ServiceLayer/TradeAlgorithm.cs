using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeBank3.ServiceLayer
{
    public class TradeAgorithm : ITradeAlgorithm
    {
        public void ComputeBaselinePPU(Models.Baseline baseline)
        {
            double sgdM = (double)baseline.originModifier;
            double usdM = (double)baseline.usdModifier;
            double gbpM = (double)baseline.gdpModifier;

            BaselineData.sgdToUsdBaseline = usdM / sgdM;
            BaselineData.usdToSgdBaseline = sgdM / usdM;
            BaselineData.sgdToGbpBaseline = gbpM / sgdM;
            BaselineData.gbpToSgdBaseline = sgdM / gbpM;
            BaselineData.usdToGbpBaseline = (gbpM / sgdM) * (sgdM / usdM);
            BaselineData.gbpToUsdBaseline = (usdM / sgdM) * (sgdM / gbpM);
            BaselineData.hasValues = true;
            BaselineData.StoreMe();
        }

        public void ShouldAcceptTrade(Models.UserInput userInput)
        {
            //check first if baselines have value
            if (!BaselineData.hasValues)
                return;



            double applicableConversion = -1.0;
            if(userInput.sourceCurrency == "SGD" && userInput.purchaseCurrency == "USD")
            {
                applicableConversion = BaselineData.sgdToUsdBaseline;
            }
            else if (userInput.sourceCurrency == "USD" && userInput.purchaseCurrency == "SGD")
            {
                applicableConversion = BaselineData.usdToSgdBaseline;
            }
            else if (userInput.sourceCurrency == "SGD" && userInput.purchaseCurrency == "GBP")
            {
                applicableConversion = BaselineData.sgdToGbpBaseline;
            }
            else if (userInput.sourceCurrency == "GBP" && userInput.purchaseCurrency == "SGD")
            {
                applicableConversion = BaselineData.gbpToSgdBaseline;
            }
            else if (userInput.sourceCurrency == "GBP" && userInput.purchaseCurrency == "USD")
            {
                applicableConversion = BaselineData.gbpToUsdBaseline;
            }
            else if (userInput.sourceCurrency == "USD" && userInput.purchaseCurrency == "GBP")
            {
                applicableConversion = BaselineData.usdToGbpBaseline;
            }

            if((double)userInput.PPU > applicableConversion)
            {
                //buy it
            }
        }
    }
}
