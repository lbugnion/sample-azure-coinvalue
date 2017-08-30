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
        [FunctionName("CoinValueSaver")]
        public static async Task Run([TimerTrigger("0 0 */1 * * *")]TimerInfo myTimer, TraceWriter log)
        {
            // Every hour: 0 0 */1 * * *
            // See https://codehollow.com/2017/02/azure-functions-time-trigger-cron-cheat-sheet/

            log.Info($"CoinValueSaver executed at: {DateTime.Now}");

        }
    }
}
