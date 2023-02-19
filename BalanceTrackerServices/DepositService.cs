using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalanceTrackerServices
{

    public class DepositService
    {
        private readonly AccountBalance _accountBalance;

        public DepositService(AccountBalance accountBalance)
        {
            _accountBalance = accountBalance;
        }

        public DepositResult Deposit(string asset, decimal quantity)
        {
            if (!_accountBalance.Balances.ContainsKey(asset))
            {
                _accountBalance.Balances[asset] = 0;
            }

            _accountBalance.Balances[asset] += quantity;

            var success = true;
            var newBalance = new AccountBalance
            {
                Balances = _accountBalance.Balances
            };
            return new DepositResult
            {
                Success = success,
                Asset = asset,
                Quantity = quantity,
                NewBalance = newBalance
            };
        }
    }

}
