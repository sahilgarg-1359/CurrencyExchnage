using System;

namespace ExchangeRateInfo
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public string? CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}

