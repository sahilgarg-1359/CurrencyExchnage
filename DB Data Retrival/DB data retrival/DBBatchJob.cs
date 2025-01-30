using DailyExchangeRateUpdater;

class DBBatchJob
{
     public static async Task Main()
    {
        Console.WriteLine("Starting Batch Job");

        try
        {
            FetchExchangeRate exchangeRate = new();
            ExchangeRateUpdater updater = new();

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