using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceTrackerServices
{
    public class WithdrawalResult
    {
        public bool Success { get; set; }
        public List<(string asset, decimal quantity)> UsedAssets { get; set; }

        public WithdrawalResult(bool success, List<(string asset, decimal quantity)> usedAssets)
        {
            Success = success;
            UsedAssets = usedAssets;
        }
    }

}
