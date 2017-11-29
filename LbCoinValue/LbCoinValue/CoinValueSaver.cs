using CoinClient.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LbCoinValue
{
    public static class CoinValueSaver
    {
        private const string Url = "https://api.coinmarketcap.com/v1/ticker/";
        public const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=lbhackfest;AccountKey=PMYwcvXDTWcnoT3G4XbL4rLhLb2SIDRDOVCvmyP3uA08MXWNGNch86ozxeHuASz8Ket0TEmcqhNoopfIT1T7qw==;BlobEndpoint=https://lbhackfest.blob.core.windows.net/;QueueEndpoint=https://lbhackfest.queue.core.windows.net/;TableEndpoint=https://lbhackfest.table.core.windows.net/;FileEndpoint=https://lbhackfest.file.core.windows.net/;";
        public const string TableName = "coins";

        [FunctionName("CoinValueSaver")]
        public static async Task Run(
            //[TimerTrigger("*/5 * * * * *")] // For debug only, every 5 minutes
            [TimerTrigger("0 0 */1 * * *")] // For production, every 1 hour
            TimerInfo myTimer,
            TraceWriter log)
        {
            // Every hour: 0 0 */1 * * *
            // See https://codehollow.com/2017/02/azure-functions-time-trigger-cron-cheat-sheet/

            log.Info($"CoinValueSaver executed at: {DateTime.Now}");

            // Create account, client and table
            var account = CloudStorageAccount.Parse(ConnectionString);
            var tableClient = account.CreateCloudTableClient();
            var table = tableClient.GetTableReference(TableName);
            await table.CreateIfNotExistsAsync();

            // Get coin value (JSON)
            var client = new HttpClient();
            var json = await client.GetStringAsync(Url);

            var priceBtc = 0.0;
            var priceEth = 0.0;

            try
            {
                var array = JArray.Parse(json);

                var priceString = array.Children<JObject>()
                    .FirstOrDefault(c => c.Property("symbol").Value.ToString().ToLower() == CoinTrend.SymbolBtc)?
                    .Property("price_usd").Value.ToString();

                if (priceString != null)
                {
                    double.TryParse(priceString, out priceBtc);
                }

                priceString = array.Children<JObject>()
                    .FirstOrDefault(c => c.Property("symbol").Value.ToString().ToLower() == CoinTrend.SymbolEth)?
                    .Property("price_usd").Value.ToString();

                if (priceString != null)
                {
                    double.TryParse(priceString, out priceEth);
                }
            }
            catch
            {
                // Do nothing here for demo purposes
            }

            if (priceBtc < 0.1
                || priceEth < 0.1)
            {
                log.Info("Something went wrong");
                return; // Do some logging here
            }

            var coinBtc = new CoinEntity
            {
                Symbol = CoinTrend.SymbolBtc,
                TimeOfReading = DateTime.Now,
                RowKey = "rowBtc" + DateTime.Now.Ticks,
                PartitionKey = "partition",
                PriceUsd = priceBtc
            };

            // Insert new value in table
            table.Execute(TableOperation.Insert(coinBtc));

            var coinEth = new CoinEntity
            {
                Symbol = CoinTrend.SymbolEth,
                TimeOfReading = DateTime.Now,
                RowKey = "rowEth" + DateTime.Now.Ticks,
                PartitionKey = "partition",
                PriceUsd = priceEth
            };

            // Insert new value in table
            table.Execute(TableOperation.Insert(coinEth));

            // Send notification to devices
            const string uriAndroid = "https://api.mobile.azure.com/v0.1/apps/lbugnion/CoinValue.Android/push/notifications";
            const string uriUwp = "https://api.mobile.azure.com/v0.1/apps/lbugnion/CoinValue.UWP/push/notifications";
            const string ApiToken = "b19f8321553f324acc49544b5179c44f9e738680";

            var notification = $"{{\"notification_content\":{{\"name\":\"CoinValue\",\"title\":\"New values saved\",\"body\": \"Btc: {priceBtc} U$ | Eth: {priceEth} U$\"}}}}";

            var request1 = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriAndroid),
                Method = HttpMethod.Post,
                Content = new StringContent(
                    notification,
                    Encoding.UTF8,
                    "application/json")
            };

            request1.Headers.Add("X-API-Token", ApiToken);

            var response = await client.SendAsync(request1);
            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                log.Error("Error posting the push notification to Android");
            }

            var request2 = new HttpRequestMessage()
            {
                RequestUri = new Uri(uriUwp),
                Method = HttpMethod.Post,
                Content = new StringContent(
                    notification,
                    Encoding.UTF8,
                    "application/json")
            };

            request2.Headers.Add("X-API-Token", ApiToken);

            response = await client.SendAsync(request2);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                log.Error("Error posting the push notification to UWP");
            }
        }
    }
}