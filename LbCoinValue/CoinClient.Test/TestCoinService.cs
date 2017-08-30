using CoinClient.Data.Model;
using System.Threading.Tasks;

namespace CoinClient.Test
{
    public class TestCoinService : ICoinService
    {
        private double _testValue;
        private int _trend;

        public TestCoinService(double testValue, int trend)
        {
            _testValue = testValue;
            _trend = trend;
        }

        public Task<CoinTrend> GetTrend()
        {
            var tcs = new TaskCompletionSource<CoinTrend>();

            tcs.SetResult(new CoinTrend
            {
                CurrentValue = _testValue,
                Trend = _trend
            });

            return tcs.Task;
        }
    }
}