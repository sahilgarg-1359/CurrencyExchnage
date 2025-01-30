using DailyExchangeRateUpdater;
using ExchangeRateInfo;
class DBBatchJob
{
     public static async Task Main()
    {
        Console.WriteLine("Starting Batch Job");

        try
        {
            //ExchangeRateInfo users = await apiService.FetchUsersAsync();

            //SaveUsersToDatabase(users);

            Console.WriteLine("Data successfully saved to Azure SQL Database!");

            FetchExchangeRate exchangeRate = new FetchExchangeRate();
            ExchangeRateDbContext updater = new();

            Console.WriteLine("Fetching latest exchange rates");

            var rates = await exchangeRate.LoadRatesAsync(null);
            await updater.SaveRatesToDatabaseAsync(rates, DateTime.UtcNow);
            Console.WriteLine("Exchange rates updated successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nError: {e.Message}");
        }
    }
}