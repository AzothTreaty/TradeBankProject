using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeBank3.ServiceLayer
{
    public interface ITradeAlgorithm
    {
        public BaselineData ComputeBaselinePPU(Models.Baseline baseline);
        public Task<String> ShouldAcceptTrade(Models.UserInput userInput, BaselineData data);
    }
}
