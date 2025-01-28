using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
    
        try
        {
            Console.Write("Enter the source currency code: ");
            string fromCurrency = Console.ReadLine().ToUpper();

            Console.Write("Enter the target currency code: ");
            string toCurrency = Console.ReadLine().ToUpper();

            Console.Write("Enter the amount: ");
            double amount = Convert.ToDouble(Console.ReadLine());

            Console.Write("Do you want exchange rate by any Date: ");
            string answer = Console.ReadLine();

            FetchExchangeRate converter = new FetchExchangeRate();
            string date = "latest";
            if (answer == "YES")
            {
                Console.Write("Enter the Data in Formate yyyy-mm--dd: ");
                date = Console.ReadLine();

            }

            Console.WriteLine("Fetch latest exchange rates-");
            await converter.LoadRatesAsync(date);

            double result = converter.Convert(fromCurrency, toCurrency, amount);

            Console.WriteLine($"\n{amount} {fromCurrency} is equal to {result:F2} {toCurrency}.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nError: {e.Message}");
        }
    }
}
