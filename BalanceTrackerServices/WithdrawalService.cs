using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceTrackerServices
{

    public class WithdrawalService
    {
        private static readonly Dictionary<string, decimal> assets = new Dictionary<string, decimal>
    {
        { "EUR", 500m },
        { "USD", 1000m },
        { "GBP", 200m },
        { "JPY", 30000m }
    };

        private static readonly Dictionary<string, decimal> exchangeRates = new Dictionary<string, decimal>
    {
        { "EUR", 1m },
        { "USD", 1.22m },
        { "GBP", 0.86m },
        { "JPY", 130.77m }
    };
        public WithdrawalResult Withdraw(string asset,decimal quantity)
        {
            if (!assets.ContainsKey(asset) || assets[asset] < quantity)
            {
                // asset not present or balance not enough, try to convert to other available currencies
                decimal amount = quantity;
                List<(string, decimal)> usedAssets = new List<(string, decimal)>();
                foreach (var kvp in assets)
                {
                    if (kvp.Key != asset && exchangeRates.ContainsKey(kvp.Key))
                    {
                        decimal convertedAmount = amount * exchangeRates[kvp.Key] / exchangeRates[asset];
                        if (kvp.Value >= convertedAmount)
                        {
                            usedAssets.Add((kvp.Key, convertedAmount));
                            amount -= convertedAmount;
                            if (amount <= 0) break;
                        }
                    }
                }
                if (amount > 0)
                {
                    return new WithdrawalResult(false, usedAssets); // insufficient balance in any currency
                }
                // perform the withdrawals from the balance
                assets[asset] -= quantity;
                foreach (var (usedAsset, usedQuantity) in usedAssets)
                {
                    assets[usedAsset] -= usedQuantity;
                }
                return new WithdrawalResult(true, usedAssets);
            }
            else
            {
                // perform the withdrawal from the balance
                assets[asset] -= quantity;
                return new WithdrawalResult(true, new List<(string, decimal)> { (asset, quantity) });
            }
        }
    }

}
