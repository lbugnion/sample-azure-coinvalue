using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoinClient.Data.Model
{
    public class CoinService : ICoinService
    {
        private const string Url = "https://lbcoinvalueapp.azurewebsites.net/api/CoinTrendGetter?code=/14FZhzxOdCKjGn9mn69SKHGaOM6JQ8naN2FCaU9O0ACyECCe1g3Iw==";

        public async Task<CoinTrend> GetTrend()
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(Url);

            var trend = JsonConvert.DeserializeObject<CoinTrend>(json);
            return trend;
        }
    }
}