using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoinClient.Model
{
    public class CoinService : ICoinService
    {
        private const string Url = "https://lbcoinvalueapp.azurewebsites.net/api/get/symbol/{symbol}?code=tFJxlkcewOVd41wrH9yqPSbGOtwZeXVc6YSfEC3v97ND3nb3EBK/bA==";

        public async Task<CoinTrend> GetTrend(string symbol = CoinTrend.SymbolBtc)
        {
            var client = new HttpClient();
            var json = await client.GetStringAsync(Url.Replace("{symbol}", symbol));

            var trend = JsonConvert.DeserializeObject<CoinTrend>(json);
            return trend;
        }
    }
}