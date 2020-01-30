  using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TradeBank3.Models;
using TradeBank3.ServiceLayer;

namespace TradeBank3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IUserInput _userInput;
        private IHttpClientFactory _clientFactory;

        public HomeController(IUserInput userInput,ILogger<HomeController> logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _userInput = userInput;
            _clientFactory = clientFactory;

        }


        public async Task<IActionResult> Index()
        {

            try
            {
                Models.Registration registrationDetails = new Models.Registration
                { 
                    appName = "TradeBankEricTeam",
                    UniqueCode = "TradeBankEricTeam1235789"

                };
                var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/bank/register");
                var client = _clientFactory.CreateClient("TradeBankProject");
                var json = JsonConvert.SerializeObject(registrationDetails);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    throw new HttpRequestException();
                }

            }
            catch (Exception e)
            {

            }


            try
            {
                Models.UserInput userInput2 = new Models.UserInput
                {
                    requestType = "Buy",
                    tradeId = new Guid(),
                    sourceCurrency = "SGD",
                    PPU = 1.4m,
                    purchaseAmount = 2000,
                    purchaseCurrency = "USD"
                };
                var test = JsonConvert.SerializeObject(userInput2);

                //var record = JsonConvert.SerializeObject<Object>(message.Value);
                dynamic record = JsonConvert.DeserializeObject(test);

                if (record.tradeId != null)
                {
                    Models.UserInput userInput3 = new Models.UserInput
                    {
                        requestType = record.requestType,
                        tradeId = record.tradeId,
                        sourceCurrency = record.sourceCurrency,
                        PPU = record.PPU,
                        purchaseAmount = record.purchaseAmount,
                        purchaseCurrency = record.purchaseCurrency
                    };

                    await _userInput.AddUserInput(userInput3);  
                }

            }
            catch (Exception e)
            {

            }


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
