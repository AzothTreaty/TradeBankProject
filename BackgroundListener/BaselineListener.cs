using CAKafka.Domain.Models;
using CAKafka.Library.Subscriber.BackgroundListeners;
using Confluent.Kafka;
using MediatR;
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
        private BaselineData _baselineData;
        public BaselineListener(BaselineData baselineData, ITradeAlgorithm tradeAlgo, IUserInput userInput, ILogger<BaselineListener> logger, IOptions<KafkaOptions> options) : base(logger, options.Value, new List<string> { "TradeBaseline", "TradeOffer"})
        {
            _logger = logger;
            _userInput = userInput;
            _tradeAlgo = tradeAlgo;
            _baselineData = baselineData;
        }

        public override async Task ProcessingLogic(IConsumer<string, string> consumer, ConsumeResult<string, string> message)
        {
            try
            {
                dynamic kafkaMessage = JsonConvert.DeserializeObject(message.Value);

                if (kafkaMessage.RequestType != null)
                {
                    _logger.LogInformation("TradeOffer");
                    _tradeAlgo.ShouldAcceptTrade((Models.UserInput)kafkaMessage, _baselineData);
                    await _userInput.AddUserInput((Models.UserInput)kafkaMessage);
                }
                else if (kafkaMessage.RecordId != null)
                {
                    _logger.LogInformation("TradeBaseline");
                    _logger.LogInformation("TradeBaseline1");
                    _logger.LogInformation("Hello " + _baselineData.sgdToUsdBaseline);
                    _logger.LogInformation("Hello " + _baselineData.usdToSgdBaseline);
                    _logger.LogInformation("Hello " + _baselineData.sgdToGbpBaseline);
                    _logger.LogInformation("Hello " + _baselineData.gbpToSgdBaseline);
                    _logger.LogInformation("Hello " + _baselineData.usdToGbpBaseline);
                    _logger.LogInformation("Hello " + _baselineData.gbpToUsdBaseline);
                    _tradeAlgo.ComputeBaselinePPU((Models.Baseline)kafkaMessage, _baselineData);
                    _logger.LogInformation("Hello1 " + _baselineData.sgdToUsdBaseline);
                    _logger.LogInformation("Hello1 " + _baselineData.usdToSgdBaseline);
                    _logger.LogInformation("Hello1 " + _baselineData.sgdToGbpBaseline);
                    _logger.LogInformation("Hello1 " + _baselineData.gbpToSgdBaseline);
                    _logger.LogInformation("Hello1 " + _baselineData.usdToGbpBaseline);
                    _logger.LogInformation("Hello1 " + _baselineData.gbpToUsdBaseline);
                }
                else
                {
                    _logger.LogInformation("ERROR, wrong format");
                    _logger.LogInformation($"message {message.Value}");
                }

                consumer.Commit(message);
            }
            catch (Exception e)
            {

            }
        }
    }
}
