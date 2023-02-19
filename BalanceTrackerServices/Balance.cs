using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceTrackerServices
{
    public class Balance
    {
        public Balance(List<Currency> Currencies) { this.Currencies = Currencies; }
        public List<Currency> Currencies { get; set; }
    }
}
