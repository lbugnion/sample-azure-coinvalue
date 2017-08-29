using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace LbCoinValue
{
    public class CoinEntity : TableEntity
    {
        public string Symbol { get; set; }
        public DateTime TimeOfReading { get; set; }
        public double PriceUsd { get; set; }
    }
}
