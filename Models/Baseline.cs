using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeBank3.Models
{
    public class Baseline
    {
        public Guid baselineId { get; set; }
        public string recordId { get; set; }
        public string originType{ get; set; }
        public decimal originModifier{ get; set; }
        public decimal usdModifier { get; set; }
        public decimal gdpModifier { get; set; }
        public DateTime createdTs { get; set; }
        public int version { get; set; }
    }
}
