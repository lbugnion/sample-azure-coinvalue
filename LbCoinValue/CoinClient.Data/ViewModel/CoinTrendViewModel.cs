using CoinClient.Model;

namespace CoinClient.ViewModel
{
    public class CoinTrendViewModel
    {
        public CoinTrend Model { get; }

        public CoinTrendViewModel(CoinTrend model)
        {
            Model = model;
        }

        public bool IsDownTrendVisible
        {
            get
            {
                return Model != null && Model.Trend < 0;
            }
        }

        public bool IsFlatTrendVisible
        {
            get
            {
                return Model?.Trend == 0;
            }
        }

        public bool IsUpTrendVisible
        {
            get
            {
                return Model != null && Model.Trend > 0;
            }
        }
    }
}