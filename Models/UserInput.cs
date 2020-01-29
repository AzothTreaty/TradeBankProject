using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeBank3.Models
{
    public class UserInput
    {
        [Key]
        public Guid UserInputId { get; set; }

        public string requestType { get; set; }
        public Guid tradeId { get; set; }
        public string sourceCurrency { get; set; }
        public decimal PPU { get; set; }
        public decimal purchaseAmount { get; set; }
        public string purchaseCurrency { get; set; }


    }
}
