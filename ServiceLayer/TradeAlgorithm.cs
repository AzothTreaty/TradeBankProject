using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TradeBank3.BackgroundListener;
using Microsoft.Extensions.Caching.Memory;

namespace TradeBank3.ServiceLayer
{
    public class TradeAgorithm : ITradeAlgorithm
    {
        private IHttpClientFactory _clientFactory;
        private readonly ILogger<TradeAgorithm> _logger;
        private IMemoryCache _cache;
        private const string cacheKey = "BaselineData";
        private const decimal baselineAmount = 1000m;

        public TradeAgorithm (ILogger<TradeAgorithm> logger, IHttpClientFactory clientFactory, IMemoryCache cache)
        {
            _clientFactory = clientFactory;
            _logger = logger;
            _cache = cache;
            cache.Remove(cacheKey);
        }

        public async Task<Models.Baseline> GetBaseline()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/trade/baseline");
            var client = _clientFactory.CreateClient("TradeBankProject");
            var response = await client.SendAsync(request);

            dynamic message = response.Content;

            Models.Baseline baseline = new Models.Baseline
            {
                recordId = message.recordId,
                originType = message.originType,
                originModifier = message.originModifier,
                usdModifier = message.usdModifier,
                gdpModifier = message.gbpModifier,
                createdTs = message.createdTs,
                version = message.version
            };

            return baseline;
        }

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

            /*_logger.LogInformation("Hello11 " + data.sgdToUsdBaseline);
            _logger.LogInformation("Hello12 " + data.usdToSgdBaseline);
            _logger.LogInformation("Hello13 " + data.sgdToGbpBaseline);
            _logger.LogInformation("Hello14 " + data.gbpToSgdBaseline);
            _logger.LogInformation("Hello15 " + data.usdToGbpBaseline);
            _logger.LogInformation("Hello16 " + data.gbpToUsdBaseline);*/

            //_cache.Set(cacheKey, data);
            return data;
        }
        public async Task<String> ShouldAcceptTrade(Models.UserInput userInput, BaselineData data)
        {
            //BaselineData data = _cache.Get<BaselineData>(cacheKey);

            if (!data.hasValues)//(data == null)
            {
                //_logger.LogInformation("==========================================================================Baseline doesnt exist yet");
                data = ComputeBaselinePPU(await GetBaseline());
                //return "No Trade Baseline";
            }

            //if (userInput.purchaseAmount <= baselineAmount)
            //    return "Too small purchase amounts";

            /*_logger.LogInformation("Hello1 " + data.sgdToUsdBaseline);
            _logger.LogInformation("Hello2 " + data.usdToSgdBaseline);
            _logger.LogInformation("Hello3 " + data.sgdToGbpBaseline);
            _logger.LogInformation("Hello4 " + data.gbpToSgdBaseline);
            _logger.LogInformation("Hello5 " + data.usdToGbpBaseline);
            _logger.LogInformation("Hello6 " + data.gbpToUsdBaseline);*/


            double applicableConversion = -1.0;
            if(userInput.sourceCurrency == "SGD")
            {
                if(userInput.purchaseCurrency == "USD")
                    applicableConversion = data.sgdToUsdBaseline;
                else if(userInput.purchaseCurrency == "GBP")
                    applicableConversion = data.sgdToGbpBaseline;
                else
                    _logger.LogInformation("==========================================================================Error Invalid Conversion");
            }
            else if (userInput.sourceCurrency == "USD")
            {
                if (userInput.purchaseCurrency == "SGD")
                    applicableConversion = data.usdToSgdBaseline;
                else if (userInput.purchaseCurrency == "GBP")
                    applicableConversion = data.usdToGbpBaseline;
                else
                    _logger.LogInformation("==========================================================================Error Invalid Conversion");
            }
            else if (userInput.sourceCurrency == "GBP")
            {
                if (userInput.purchaseCurrency == "SGD")
                    applicableConversion = data.gbpToSgdBaseline;
                else if (userInput.purchaseCurrency == "USD")
                    applicableConversion = data.gbpToUsdBaseline;
                else
                    _logger.LogInformation("==========================================================================Error Invalid Conversion");
            }
            else
                _logger.LogInformation("==========================================================================Error Invalid Conversion");

            if (((double)userInput.PPU > applicableConversion))
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
                        //_logger.LogInformation("==========================================================================Trade success");
                        return "Trade Success";

                    }
                    else
                    {
                        //_logger.LogInformation("==========================================================================Trade not ours");
                        //throw new HttpRequestException();
                        return "Trade already taken";
                    }
                }
                catch (Exception e)
                {
                    _logger.LogInformation($"message {e.Message}");
                    return "Trade API Exception";
                }
            }
            //_logger.LogInformation("==========================================================================Trade offer baseline is low");
            return "Trade offer baseline is low";
        }
    }
}
