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
        public BaselineListener(IUserInput userInput, ILogger<BaselineListener> logger, IOptions<KafkaOptions> options) : base(logger, options.Value, new List<string> { "TradeBaseline", "TradeOffer"})
        {
            _logger = logger;
            _userInput = userInput;
           
        }

        public override async Task ProcessingLogic(IConsumer<string, string> consumer, ConsumeResult<string, string> message)
        {
            try
            {
                Models.UserInput userInput2 = new Models.UserInput
                {
                    requestType = "Buy",
                    tradeId = new Guid(),
                    sourceCurrency = "SGD",
                    PPU = 1.4m,
                    purchaseAmount = 1000,
                    purchaseCurrency = "USD"
                };
                var test = JsonConvert.SerializeObject(userInput2);

                //var record = JsonConvert.DeserializeObject<Object>(message.Value);
                Models.UserInput record = JsonConvert.DeserializeObject<Models.UserInput>(test);

                //code to caculate ppu
                await _userInput.AddUserInput((Models.UserInput)record);

                _logger.LogInformation($"message {message.Value}");

               
                
                consumer.Commit(message);
            }
            catch (Exception e)
            {

            }
        }
    }
}
