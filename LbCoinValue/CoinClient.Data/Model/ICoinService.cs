using System.Threading.Tasks;

namespace CoinClient.Data.Model
{
    public interface ICoinService
    {
        Task<CoinTrend> GetTrend();
    }
}