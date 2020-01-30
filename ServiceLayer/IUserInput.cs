using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeBank3.BackgroundListener;

namespace TradeBank3.ServiceLayer
{
    

    public interface IUserInput
    {
        Task<Boolean> AddUserInput(Models.UserInput userInput);
        List<Models.UserInput> GetUserInput();
    }


}
