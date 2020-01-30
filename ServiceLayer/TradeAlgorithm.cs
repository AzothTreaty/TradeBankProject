using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeBank3.ServiceLayer
{
    public class TradeAgorithm : ITradeAlgorithm
    {
        public void ComputeBaselinePPU(Models.Baseline baseline, BaselineData data)
        {
            double sgdM = (double)baseline.originModifier;
            double usdM = (double)baseline.usdModifier;
            double gbpM = (double)baseline.gdpModifier;

            data.sgdToUsdBaseline = usdM / sgdM;
            data.usdToSgdBaseline = sgdM / usdM;
            data.sgdToGbpBaseline = gbpM / sgdM;
            data.gbpToSgdBaseline = sgdM / gbpM;
            data.usdToGbpBaseline = (gbpM / sgdM) * (sgdM / usdM);
            data.gbpToUsdBaseline = (usdM / sgdM) * (sgdM / gbpM);
            data.hasValues = true;
        }

        public void ShouldAcceptTrade(Models.UserInput userInput, BaselineData data)
        {
            //check first if baselines have value
            if (!data.hasValues)
                return;



            double applicableConversion = -1.0;
            if(userInput.sourceCurrency == "SGD" && userInput.purchaseCurrency == "USD")
            {
                applicableConversion = data.sgdToUsdBaseline;
            }
            else if (userInput.sourceCurrency == "USD" && userInput.purchaseCurrency == "SGD")
            {
                applicableConversion = data.usdToSgdBaseline;
            }
            else if (userInput.sourceCurrency == "SGD" && userInput.purchaseCurrency == "GBP")
            {
                applicableConversion = data.sgdToGbpBaseline;
            }
            else if (userInput.sourceCurrency == "GBP" && userInput.purchaseCurrency == "SGD")
            {
                applicableConversion = data.gbpToSgdBaseline;
            }
            else if (userInput.sourceCurrency == "GBP" && userInput.purchaseCurrency == "USD")
            {
                applicableConversion = data.gbpToUsdBaseline;
            }
            else if (userInput.sourceCurrency == "USD" && userInput.purchaseCurrency == "GBP")
            {
                applicableConversion = data.usdToGbpBaseline;
            }

            if((double)userInput.PPU > applicableConversion)
            {
                //buy it
            }
        }
    }
}
