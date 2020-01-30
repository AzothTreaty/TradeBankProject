using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeBank3.ServiceLayer
{
    public interface ITradeAlgorithm
    {
        public void ComputeBaselinePPU(Models.Baseline baseline, BaselineData data);
        public void ShouldAcceptTrade(Models.UserInput userInput, BaselineData data);
    }
}
