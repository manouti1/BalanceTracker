using Newtonsoft.Json.Linq;
using System.Configuration;

namespace BalanceTrackerServices
{
    public class ExchangeUploader
    {
        private string apiUrl;
        private List<string> assets;
        private Thread uploadThread;
        private bool isUploading;
        public ExchangeUploader(string apiUrl, List<string> assets)
        {
            this.apiUrl = apiUrl;
            this.assets = assets;
        }

        private ExchangeRate ParseExchangeRate(string data)
        {
            // Parse the exchange rate from the API response
            // In this case, the API returns the data in JSON format
            // You may need to adjust this code to parse data from a different format
            var exchangeRate = JObject.Parse(data);
            var baseCurrency = exchangeRate["base"].ToObject<String>();
            var rates = exchangeRate["rates"].ToObject<Dictionary<string, decimal>>();

            return new ExchangeRate
            {
                BaseCurrency = baseCurrency,
                Rates = rates
            };
        }

        private void UploadExchangeRate(string asset, Dictionary<string, decimal> rates)
        {
            // Upload the exchange rate to the system
            // In this case, I'm just printing the data to the console
            // You may need to adjust this code to upload the data to your system
             Console.WriteLine($"Uploading exchange rate: Base Asset={asset}");
            Console.WriteLine($"Exchange Rates:");
            rates.AsParallel()
                            .ForAll(x => Console.WriteLine($"{x.Key} : {x.Value}"));
        }

        private void Upload()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("apikey", "w6HXPYP7gkTlRRZzOM8kcWjBUcf6lX79");

            while (isUploading)
            {
                foreach (var asset in assets)
                {
                    var url = apiUrl + asset;

                    // Fetch the latest exchange rate for the current asset
                    var response = httpClient.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var data = response.Content.ReadAsStringAsync().Result;
                        var exchangeRates = ParseExchangeRate(data);

                        // Upload the exchange rate to the system
                        UploadExchangeRate(asset, exchangeRates.Rates);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to fetch exchange rate for {asset}: {response.StatusCode}");
                    }
                }

                // Wait for the next tick
                Thread.Sleep(1000);
            }
        }

        public void StartUploading()
        {
            if (uploadThread == null)
            {
                isUploading = true;
                uploadThread = new Thread(Upload);
                uploadThread.Start();
            }
        }

        public void StopUploading()
        {
            isUploading = false;
            if (uploadThread != null)
            {
                uploadThread.Join();
                uploadThread = null;
            }
        }
    }
}