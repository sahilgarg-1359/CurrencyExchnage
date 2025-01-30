using Microsoft.EntityFrameworkCore;
using ExchangeAPIservice;
using ExchangeRateInfo;

namespace DailyExchangeRateUpdater
{
    public class ExchangeRateDbContext : DbContext
    {

        public DbSet<ExchangeRate> ExchangeRate { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Replace with your SQL Server connection string
            optionsBuilder.UseSqlServer(AppSettingConfig.Server_Name, AppSettingConfig.Database_Name, AppSettingConfig.User_Id,AppSettingConfig.Password);
        }
    

    //public class ExchangeRateUpdater
    //{
    //    private const string FixerApiKey = "YOUR_FIXER_API_KEY"; // Replace with your Fixer.io API key
    //    private const string FixerApiUrl = "http://data.fixer.io/api/latest";

    //    public async Task<Dictionary<string, decimal>> FetchLatestRatesAsync()
    //    {
    //        string url = $"{FixerApiUrl}?access_key={FixerApiKey}";

    //        using (HttpClient client = new HttpClient())
    //        {
    //            HttpResponseMessage response = await client.GetAsync(url);

    //            if (!response.IsSuccessStatusCode)
    //            {
    //                throw new Exception($"Failed to fetch exchange rates. Status code: {response.StatusCode}");
    //            }

    //            string responseData = await response.Content.ReadAsStringAsync();
    //            dynamic json = JsonConvert.DeserializeObject(responseData);

    //            if (json.success != true)
    //            {
    //                throw new Exception($"API Error: {json.error.type}");
    //            }

    //            // Deserialize rates into a dictionary
    //            return JsonConvert.DeserializeObject<Dictionary<string, decimal>>(JsonConvert.SerializeObject(json.rates));
    //        }
    //    }

        public async Task SaveRatesToDatabaseAsync(Dictionary<string, decimal> rates, DateTime date)
        {
            using var context = new ExchangeRateDbContext();

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
                await context.ExchangeRate.AddAsync(exchangeRate);
            }

            // Save changes
            await context.SaveChangesAsync();
        }
    }
}
