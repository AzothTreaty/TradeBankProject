using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TradeBank3.Models
{
    public class BaselinePPU
    {
        [Key]
        public Guid PaselinePPUID { get; set; }

        public Guid UserInputId { get; set; }

        [ForeignKey("UserInputId")]
        public virtual UserInput userInput { get; set; }

        public string sourceCurrency { get; set; }

        public string purchaseCurrency { get; set; }

        public decimal PPU { get; set; }
    }
}
