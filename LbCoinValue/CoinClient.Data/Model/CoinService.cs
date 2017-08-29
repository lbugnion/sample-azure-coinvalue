using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoinClient.Data.Model
{
    public class CoinService : ICoinService
    {
        private const string Url = "https://lbcoinvalue.azurewebsites.net/api/CoinTrendGetter?code=LZ9GFEPbM5LAxKLp/fnPNI8M1T0ZJ6LwY9a55Cs6XZYfBMrlatcSMQ==";

        public async Task<CoinTrend> GetTrend()
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(Url);

            // Workaround for weird formatting
            json = json
                .Replace("\\", string.Empty);

            json = json.Substring(1);
            json = json.Substring(0, json.Length - 1);

            var trend = JsonConvert.DeserializeObject<CoinTrend>(json);
            return trend;
        }
    }
}
