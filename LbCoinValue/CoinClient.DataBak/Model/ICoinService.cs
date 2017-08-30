using System.Threading.Tasks;

namespace CoinClient.Model
{
    public interface ICoinService
    {
        Task<CoinTrend> GetTrend();
    }
}