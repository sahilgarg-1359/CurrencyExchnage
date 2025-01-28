using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        Console.WriteLine("Currency Converter (Using Fixer.io API)");
        Console.WriteLine("Supported currencies: USD, EUR, GBP, INR, JPY, AUD, CAD, CNY\n");

        try
        {
            Console.Write("Enter the source currency code: ");
            string fromCurrency = Console.ReadLine().ToUpper();

            Console.Write("Enter the target currency code: ");
            string toCurrency = Console.ReadLine().ToUpper();

            Console.Write("Enter the amount in the source currency: ");
            double amount = Convert.ToDouble(Console.ReadLine());

            Console.Write("Do you want exchange Rate by any Date: ");
            string answer = Console.ReadLine();

            FetchExchangeRate converter = new FetchExchangeRate();

            if (answer == "YES")
            {
                Console.Write("enter the Data in Formate yyyy-mm--dd: ");
                string date = Console.ReadLine();

                if (string.IsNullOrEmpty(date))
                    date = "latest";
            }

            Console.WriteLine("Fetching latest exchange rates...");
            await converter.LoadRatesAsync();

            double result = converter.Convert(fromCurrency, toCurrency, amount);

            Console.WriteLine($"\n{amount} {fromCurrency} is equal to {result:F2} {toCurrency}.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nError: {e.Message}");
        }
    }
}
