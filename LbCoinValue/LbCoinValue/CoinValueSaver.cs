using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace LbCoinValue
{
    public static class CoinValueSaver
    {
        public const string ConnectionString = "CONNECTION STRING HERE";
        public const string TableName = "coins";
        private const string Url = "https://api.coinmarketcap.com/v1/ticker/";
        private const string Symbol = "btc";

        [FunctionName("CoinValueSaver")]
        public static async Task Run([TimerTrigger("0 0 */1 * * *")]TimerInfo myTimer, TraceWriter log)
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
        }
    }
}
