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
        [FunctionName("CoinValueSaver")]
        public static async Task Run(
            //[TimerTrigger("*/5 * * * * *")] // For debug only, every 5 seconds
            [TimerTrigger("0 0 */1 * * *")] // For production, every 1 hour
            TimerInfo myTimer,
            TraceWriter log)
        {
            // Every hour: 0 0 */1 * * *
            // See https://codehollow.com/2017/02/azure-functions-time-trigger-cron-cheat-sheet/

            log.Info($"CoinValueSaver executed at: {DateTime.Now}");
        }
    }
}