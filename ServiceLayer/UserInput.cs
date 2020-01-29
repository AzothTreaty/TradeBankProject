using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeBank3.BackgroundListener;
using TradeBank3.DBContext;

namespace TradeBank3.ServiceLayer
{
    public class UserInput : IUserInput
    {
        private TradeBankContext _context;

        public UserInput(TradeBankContext context)
        {
            _context = context;
        }

        public async Task<Boolean> AddUserInput(Models.UserInput userInput)
        {
            _context.UserInput.Add(userInput);
            await _context.SaveChangesAsync();
            return true;
        }

   
    }
}
