using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace DailyExchangeRateUpdater
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public string? CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }

    public class ExchangeRateDbContext : DbContext
    {
        public DbSet<ExchangeRate> ExchangeRates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Replace with your SQL Server connection string
            optionsBuilder.UseSqlServer("Server=your_server;Database=ExchangeRateDB;User Id=your_user;Password=your_password;");
        }
    }

    public class ExchangeRateUpdater
    {
        private const string FixerApiKey = "YOUR_FIXER_API_KEY"; // Replace with your Fixer.io API key
        private const string FixerApiUrl = "http://data.fixer.io/api/latest";

        public async Task<Dictionary<string, decimal>> FetchLatestRatesAsync()
        {
            string url = $"{FixerApiUrl}?access_key={FixerApiKey}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to fetch exchange rates. Status code: {response.StatusCode}");
                }

                string responseData = await response.Content.ReadAsStringAsync();
                dynamic json = JsonConvert.DeserializeObject(responseData);

                if (json.success != true)
                {
                    throw new Exception($"API Error: {json.error.type}");
                }

                // Deserialize rates into a dictionary
                return JsonConvert.DeserializeObject<Dictionary<string, decimal>>(JsonConvert.SerializeObject(json.rates));
            }
        }

        public async Task SaveRatesToDatabaseAsync(Dictionary<string, decimal> rates, DateTime date)
        {
            using (var context = new ExchangeRateDbContext())
            {
                // Create database if not exists
                await context.Database.EnsureCreatedAsync();

                foreach (var rate in rates)
                {
                    var exchangeRate = new ExchangeRate
                    {
                        CurrencyCode = rate.Key,
                        Rate = rate.Value,
                        Date = date
                    };

                    // Add to the database
                    await context.ExchangeRates.AddAsync(exchangeRate);
                }

                // Save changes
                await context.SaveChangesAsync();
            }
        }
    }
}
