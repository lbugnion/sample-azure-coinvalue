using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoinClient.Data.Model
{
    public interface ICoinService
    {
        Task<CoinTrend> GetTrend();
    }
}
