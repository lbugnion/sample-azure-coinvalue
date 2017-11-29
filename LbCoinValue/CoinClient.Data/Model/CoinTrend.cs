namespace CoinClient.Model
{
    public class CoinTrend
    {
        public const string SymbolBtc = "btc";
        public const string SymbolEth = "eth";

        public double CurrentValue { get; set; }

        public int Trend { get; set; }

        public string Symbol { get; set; }
    }
}