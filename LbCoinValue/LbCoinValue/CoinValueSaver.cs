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
        public const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=lbhackfest;AccountKey=PMYwcvXDTWcnoT3G4XbL4rLhLb2SIDRDOVCvmyP3uA08MXWNGNch86ozxeHuASz8Ket0TEmcqhNoopfIT1T7qw==;BlobEndpoint=https://lbhackfest.blob.core.windows.net/;QueueEndpoint=https://lbhackfest.queue.core.windows.net/;TableEndpoint=https://lbhackfest.table.core.windows.net/;FileEndpoint=https://lbhackfest.file.core.windows.net/;";
        public const string TableName = "coins";
        private const string Url = "https://coinmarketcap-nexuist.rhcloud.com/api/btc";

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
            var j = JObject.Parse(json);

            var price = j.GetValue("price")
                .ToObject<Dictionary<string, double>>()
                .FirstOrDefault(p => p.Key == "usd")
                .Value;

            if (price == 0)
            {
                log.Info("Something went wrong");
                return; // Do some logging here
            }

            var coin = new CoinEntity
            {
                Symbol = j.GetValue("symbol").ToString(),
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
