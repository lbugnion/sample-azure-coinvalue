using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LbCoinValue
{
    public static class CoinValueSaver
    {
        private const string Symbol = "btc";
        private const string Url = "https://api.coinmarketcap.com/v1/ticker/";
        public const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=lbhackfest;AccountKey=PMYwcvXDTWcnoT3G4XbL4rLhLb2SIDRDOVCvmyP3uA08MXWNGNch86ozxeHuASz8Ket0TEmcqhNoopfIT1T7qw==;BlobEndpoint=https://lbhackfest.blob.core.windows.net/;QueueEndpoint=https://lbhackfest.queue.core.windows.net/;TableEndpoint=https://lbhackfest.table.core.windows.net/;FileEndpoint=https://lbhackfest.file.core.windows.net/;";
        public const string TableName = "coins";

        [FunctionName("CoinValueSaver")]
        public static async Task Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, TraceWriter log)
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

            var price = 0.0;

            try
            {
                var array = JArray.Parse(json);

                var priceString = array.Children<JObject>()
                    .FirstOrDefault(c => c.Property("symbol").Value.ToString().ToLower() == Symbol)?
                    .Property("price_usd").Value.ToString();

                if (priceString != null)
                {
                    double.TryParse(priceString, out price);
                }
            }
            catch
            {
                // Do nothing here for demo purposes
            }

            if (price < 0.1)
            {
                log.Info("Something went wrong");
                return; // Do some logging here
            }

            var coin = new CoinEntity
            {
                Symbol = Symbol,
                TimeOfReading = DateTime.Now,
                RowKey = "row" + DateTime.Now.Ticks,
                PartitionKey = "partition",
                PriceUsd = price
            };

            // Insert new value in table
            table.Execute(TableOperation.Insert(coin));

            // Send notification to devices
            const string uriAndroid = "https://api.mobile.azure.com/v0.1/apps/lbugnion/CoinValue.Android/push/notifications";
            const string uriUwp = "https://api.mobile.azure.com/v0.1/apps/lbugnion/CoinValue.UWP/push/notifications";

            const string notificationTemplate = "{\"notification_content\": "
                + "{\"name\": \"CoinValue\","
                + "\"title\":\"{0}\","
                + "\"body\": \"{1}\"}}";

            var notificationJson = string.Format(
                notificationTemplate,
                "New value saved",
                $"The current Bitcoin value is {price} U$");

            //var content = new StringContent(notificationJson);

            //var response = await client.PostAsync(uriAndroid, content);
            //if (response.StatusCode != System.Net.HttpStatusCode.OK)
            //{
            //    log.Error("Error posting the push notification to Android");
            //}

            //response = await client.PostAsync(uriUwp, content);
            //if (response.StatusCode != System.Net.HttpStatusCode.OK)
            //{
            //    log.Error("Error posting the push notification to UWP");
            //}
        }
    }

}