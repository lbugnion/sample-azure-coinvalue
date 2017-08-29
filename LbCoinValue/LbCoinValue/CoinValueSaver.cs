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
        public const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=lbfunctionsampl9985;AccountKey=4T2rnnQDwjpOHdEnf2tFey+iqmuxdtj/1uwiaHMLN6zU9+NrGGUCv8mtoSXpBgyX2kn+7P04OjLLHEebiOIJ1g==;BlobEndpoint=https://lbfunctionsampl9985.blob.core.windows.net/;QueueEndpoint=https://lbfunctionsampl9985.queue.core.windows.net/;TableEndpoint=https://lbfunctionsampl9985.table.core.windows.net/;FileEndpoint=https://lbfunctionsampl9985.file.core.windows.net/;";
        public const string TableName = "coins2";
        private const string Url = "https://coinmarketcap-nexuist.rhcloud.com/api/btc";

        [FunctionName("CoinValueSaver")]
        public static async Task Run([TimerTrigger("0 * */1 * * *")]TimerInfo myTimer, TraceWriter log)
        {
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
