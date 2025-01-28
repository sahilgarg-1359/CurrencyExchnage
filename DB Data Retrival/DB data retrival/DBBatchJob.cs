using DailyExchangeRateUpdater;

class DBBatchJob
{
    static async Task Main()
    {
        Console.WriteLine("Starting Batch Job");

        try
        {
            ExchangeRateUpdater updater = new ExchangeRateUpdater();

            Console.WriteLine("Fetching latest exchange rates");
            var rates = await updater.FetchLatestRatesAsync();
            await updater.SaveRatesToDatabaseAsync(rates, DateTime.UtcNow);
            Console.WriteLine("Exchange rates updated successfully.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nError: {e.Message}");
        }
    }
}