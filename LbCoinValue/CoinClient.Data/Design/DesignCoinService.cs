using System.Threading.Tasks;
using CoinClient.Data.Model;

namespace CoinClient.Design
{
    public class DesignCoinService : ICoinService
    {
        private const int OriginalTrend = 1;
        private const double OriginalValue = 345.6;
        private CoinTrend _trend;

        public Task<CoinTrend> GetTrend()
        {
            var tcs = new TaskCompletionSource<CoinTrend>();

            if (_trend == null)
            {
                _trend = new CoinTrend
                {
                    CurrentValue = OriginalValue,
                    Trend = OriginalTrend
                };
            }
            else
            {
                _trend.CurrentValue += 1000;
                _trend.Trend =
                    _trend.Trend == 0 ? 1
                    : _trend.Trend == 1 ? -1
                    : 0;
            }

            tcs.SetResult(_trend);
            return tcs.Task;
        }
    }
}