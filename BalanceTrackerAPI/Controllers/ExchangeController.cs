using BalanceTrackerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BalanceTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeController : ControllerBase
    {
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadExchangeRates(List<string> assets)
        {
            try
            {
                var apiUrl = "https://api.apilayer.com/exchangerates_data/latest?symbols=&base=";
                var uploader = new ExchangeUploader(apiUrl, assets);
                uploader.StartUploading();
                Thread.Sleep(10000);

                uploader.StopUploading();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("deposit")]
        public async Task<IActionResult> Deposit(string asset, decimal quantity)
        {
            // Perform validation on the asset and quantity parameters
            if (string.IsNullOrEmpty(asset))
            {
                return BadRequest("Asset cannot be null or empty.");
            }

            if (quantity <= 0)
            {
                return BadRequest("Quantity must be a positive number.");
            }
            var accountBalances = new AccountBalance();
            accountBalances.Balances.Add("USD", 1);
            DepositService depositService = new DepositService(accountBalances);
            var result = depositService.Deposit(asset, quantity);

            // Return a success response with the updated balance
            return Ok(new { message = "Deposit successful", balance = result.NewBalance });
        }

        [HttpPost]
        [Route("withdraw")]
        public async Task<IActionResult> WithDraw(string asset, decimal quantity)
        {
            // Perform validation on the asset and quantity parameters
            if (string.IsNullOrEmpty(asset))
            {
                return BadRequest("Asset cannot be null or empty.");
            }

            if (quantity <= 0)
            {
                return BadRequest("Quantity must be a positive number.");
            }
            var accountBalances = new AccountBalance();
            accountBalances.Balances.Add("USD", 100);
            WithdrawalService withdrawalService = new WithdrawalService();
            WithdrawalResult result = withdrawalService.Withdraw(asset.ToUpper(), quantity);
            var json = JsonConvert.SerializeObject(result);

            return Ok(json);

        }
    }
}
