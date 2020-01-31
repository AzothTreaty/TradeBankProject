using CAKafka.Domain.Models;
using CAKafka.Library.Subscriber.BackgroundListeners;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeBank3.ServiceLayer;

namespace TradeBank3.BackgroundListener
{
    public class BaselineListener : KafkaBackgroundConsumer<string>
    {
        private readonly ILogger<BaselineListener> _logger;
        private IUserInput _userInput;
        private ITradeAlgorithm _tradeAlgo;
        private IMemoryCache _cache;
        private BaselineData baselineData;
        public BaselineListener(ITradeAlgorithm tradeAlgo, IUserInput userInput, ILogger<BaselineListener> logger, IOptions<KafkaOptions> options) : base(logger, options.Value, new List<string> { "TradeBaseline", "TradeOffer"})
        {
            _logger = logger;
            _userInput = userInput;
            _tradeAlgo = tradeAlgo;
            //_cache = cache;
            baselineData = new BaselineData();
        }

        public override async Task ProcessingLogic(IConsumer<string, string> consumer, ConsumeResult<string, string> message)
    {
            dynamic kafkaMessage = JsonConvert.DeserializeObject(message.Value);

            //_logger.LogInformation($"message {message.Value}");

            if (kafkaMessage.tradeId != null)
            {
                //_logger.LogInformation("==========================================================================TradeOffer");

                Models.UserInput userInput = new Models.UserInput
                {
                    tradeId = kafkaMessage.tradeId,
                    sourceCurrency = kafkaMessage.paymentCurrency,
                    PPU = kafkaMessage.pricePerUnit,
                    purchaseAmount = kafkaMessage.requestedAmount,
                    purchaseCurrency = kafkaMessage.requestedCurrency,
                    status = "Received",
                    timestampCreated = DateTime.Now
                };

                String status = await _tradeAlgo.ShouldAcceptTrade(userInput, baselineData);
                userInput.status = status;

                var gg = await _userInput.AddUserInput(userInput);
            }
            else if (kafkaMessage.RecordId != null)
            {
                //_logger.LogInformation("==========================================================================TradeBaseline");
                /*_logger.LogInformation("Hello1 " + baselineData.sgdToUsdBaseline);
                _logger.LogInformation("Hello2 " + baselineData.usdToSgdBaseline);
                _logger.LogInformation("Hello3 " + baselineData.sgdToGbpBaseline);
                _logger.LogInformation("Hello4 " + baselineData.gbpToSgdBaseline);
                _logger.LogInformation("Hello5 " + baselineData.usdToGbpBaseline);
                _logger.LogInformation("Hello6 " + baselineData.gbpToUsdBaseline);*/

                Models.Baseline baseline = new Models.Baseline
                {
                    recordId = kafkaMessage.RecordId,
                    originType = kafkaMessage.OriginType,
                    originModifier = kafkaMessage.OriginModifier,
                    usdModifier = kafkaMessage.UsdModifier,
                    gdpModifier = kafkaMessage.GbpModifier,
                    createdTs = kafkaMessage.CreatedTs,
                    version = kafkaMessage.Version
                };

                baselineData = _tradeAlgo.ComputeBaselinePPU(baseline);
                /*_logger.LogInformation("Hello11 " + baselineData.sgdToUsdBaseline);
                _logger.LogInformation("Hello12 " + baselineData.usdToSgdBaseline);
                _logger.LogInformation("Hello13 " + baselineData.sgdToGbpBaseline);
                _logger.LogInformation("Hello14 " + baselineData.gbpToSgdBaseline);
                _logger.LogInformation("Hello15 " + baselineData.usdToGbpBaseline);
                _logger.LogInformation("Hello16 " + baselineData.gbpToUsdBaseline);*/
            }
            else
            {
                _logger.LogInformation("==========================================================================ERROR, wrong format");
                _logger.LogInformation($"message {message.Value}");
            }

            consumer.Commit(message);
        }
    }
}
