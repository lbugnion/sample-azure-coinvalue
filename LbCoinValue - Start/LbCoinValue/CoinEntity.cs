using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace LbCoinValue
{
    public class CoinEntity : TableEntity
    {
        public double PriceUsd { get; set; }

        public string Symbol { get; set; }

        public DateTime TimeOfReading { get; set; }
    }
}