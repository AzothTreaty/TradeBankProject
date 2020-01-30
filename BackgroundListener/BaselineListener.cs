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
        public BaselineListener(ITradeAlgorithm tradeAlgo, IUserInput userInput, ILogger<BaselineListener> logger, IOptions<KafkaOptions> options) : base(logger, options.Value, new List<string> { "TradeBaseline", "TradeOffer"})
        {
            _logger = logger;
            _userInput = userInput;
            _tradeAlgo = tradeAlgo;
        }

        public override async Task ProcessingLogic(IConsumer<string, string> consumer, ConsumeResult<string, string> message)
        {
            try
            {
                dynamic kafkaMessage = JsonConvert.DeserializeObject(message.Value);

                if (kafkaMessage.RequestType != null)
                {
                    _logger.LogInformation("TradeOffer");
                    _tradeAlgo.ShouldAcceptTrade((Models.UserInput)kafkaMessage);
                    await _userInput.AddUserInput((Models.UserInput)kafkaMessage);
                }
                else if (kafkaMessage.RecordId != null)
                {
                    _logger.LogInformation("TradeBaseline");
                    _logger.LogInformation("Hello " + BaselineData.sgdToUsdBaseline);
                    _logger.LogInformation("Hello " + BaselineData.usdToSgdBaseline);
                    _logger.LogInformation("Hello " + BaselineData.sgdToGbpBaseline);
                    _logger.LogInformation("Hello " + BaselineData.gbpToSgdBaseline);
                    _logger.LogInformation("Hello " + BaselineData.usdToGbpBaseline);
                    _logger.LogInformation("Hello " + BaselineData.gbpToUsdBaseline);
                    _tradeAlgo.ComputeBaselinePPU((Models.Baseline)kafkaMessage);
                    _logger.LogInformation("Hello1 " + BaselineData.sgdToUsdBaseline);
                    _logger.LogInformation("Hello1 " + BaselineData.usdToSgdBaseline);
                    _logger.LogInformation("Hello1 " + BaselineData.sgdToGbpBaseline);
                    _logger.LogInformation("Hello1 " + BaselineData.gbpToSgdBaseline);
                    _logger.LogInformation("Hello1 " + BaselineData.usdToGbpBaseline);
                    _logger.LogInformation("Hello1 " + BaselineData.gbpToUsdBaseline);
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
