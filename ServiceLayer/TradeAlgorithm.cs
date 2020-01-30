using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace TradeBank3.ServiceLayer
{
    public class TradeAgorithm : ITradeAlgorithm
    {
        private IHttpClientFactory _clientFactory;
        private readonly ILogger<BaselineListener> _logger;

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
        }

        public async void ShouldAcceptTrade(Models.UserInput userInput)
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
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/trade/"+userInput.tradeId);
                    var client = _clientFactory.CreateClient("TradeBankProject");
                    //var json = JsonConvert.SerializeObject(userInput);
                    //request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();
                        _logger.LogInformation("Trade success");

                    }
                    else
                    {
                        
                        _logger.LogInformation("Trade not ours");
                        throw new HttpRequestException();
                    }
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
