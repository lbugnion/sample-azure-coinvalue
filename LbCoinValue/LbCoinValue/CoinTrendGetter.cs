using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using System;
using CoinClient;

namespace LbCoinValue
{
    public static class CoinTrendGetter
    {
        private const int StandardCount = 10;

        [FunctionName("CoinTrendGetter")]
        public static async Task<HttpResponseMessage> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"CoinTrendGetter executed at: {DateTime.Now}");

            // Create account, client and table
            var account = CloudStorageAccount.Parse(CoinValueSaver.ConnectionString);
            var tableClient = account.CreateCloudTableClient();
            var table = tableClient.GetTableReference(CoinValueSaver.TableName);
            await table.CreateIfNotExistsAsync();

            // Get the last 10 coin
            var coinsQuery = table.CreateQuery<CoinEntity>().ToList();
            var count = coinsQuery.Count;

            var trend = new CoinTrend();

            if (count == 0)
            {
                log.Info("No coins found");
            }
            else
            {
                var selectedCount = (count < StandardCount) ? count : StandardCount;
                var coinValues = coinsQuery
                    .Skip(count - selectedCount)
                    .Select(c => c.PriceUsd)
                    .ToArray();

                // Prepare the data for analysis
                var currentIndex = 1;
                var indexes = coinValues.Select(c => (double)currentIndex++).ToArray();

                // Calculate the linear regression
                double rSquared, yIntercept, slope;
                Stats.CalculateLinearRegression(indexes, coinValues, 0, selectedCount, out rSquared, out yIntercept, out slope);

                // Prepare the result
                trend.CurrentValue = coinValues.Last();
                trend.Trend = (slope > 0.05) ? 1 // Positive trend
                    : (slope < -0.05) ? -1       // Negative trend
                    : 0;                         // Flat trend
            }

            return req.CreateResponse(HttpStatusCode.OK, trend, "application/json");
        }
    }
}
