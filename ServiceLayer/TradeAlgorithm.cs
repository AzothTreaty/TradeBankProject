using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeBank3.ServiceLayer
{
    public class TradeAgorithm : ITradeAlgorithm
    {
        public BaselineData ComputeBaselinePPU(Models.Baseline baseline)
        {
            double sgdM = (double)baseline.originModifier;
            double usdM = (double)baseline.usdModifier;
            double gbpM = (double)baseline.gdpModifier;

            BaselineData data = new BaselineData
            {
                sgdToUsdBaseline = usdM / sgdM,
                usdToSgdBaseline = sgdM / usdM,
                sgdToGbpBaseline = gbpM / sgdM,
                gbpToSgdBaseline = sgdM / gbpM,
                usdToGbpBaseline = (gbpM / sgdM) * (sgdM / usdM),
                gbpToUsdBaseline = (usdM / sgdM) * (sgdM / gbpM),
                hasValues = true
            };

            return data;
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
