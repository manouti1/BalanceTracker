using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceTrackerServices
{
    public class DepositResult
    {
        public bool Success { get; set; }
        public string Asset { get; set; }
        public decimal Quantity { get; set; }
        public AccountBalance NewBalance { get; set; }
    }
}
