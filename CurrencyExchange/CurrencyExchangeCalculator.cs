using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public partial class Program
{
    private static Dictionary<string, double> exchangeRates;

    public static async Task Main()
    {

        try
        {
            Console.Write("Enter the source currency code: ");
            string fromCurrency = Console.ReadLine().ToUpper();

            Console.Write("Enter the target currency code: ");
            string toCurrency = Console.ReadLine()?.ToUpper();


            Console.Write("Enter the amount: ");
            double amount = Convert.ToDouble(Console.ReadLine());

            Console.Write("Do you want exchange rate by any specific Date: ");
            string answer = Console.ReadLine();
            string date = "latest";
            if (answer == "YES")
            {
                Console.Write("Enter the Data in Formate yyyy-mm--dd: ");
                date = Console.ReadLine();

            }

            Console.WriteLine("Fetch latest exchange rates-");
            FetchExchangeRate converter = new FetchExchangeRate();

            var exchangeRates = await converter.LoadRatesAsync(date);

            double result = ConvertCurrency(fromCurrency, toCurrency, amount);

            Console.WriteLine($"\n{amount} {fromCurrency} is equal to {result:F2} {toCurrency}.");
        }
        catch (Exception e)
        {
            Console.WriteLine($"\nError: {e.Message}");
        }
    }

    public static double ConvertCurrency(string firstCurrency, string secondCurrency, double amount)
    {
        if (!exchangeRates.ContainsKey(firstCurrency))
        {
            throw new ArgumentException($"Invalid currency code: {firstCurrency}");
        }
        if (!exchangeRates.ContainsKey(secondCurrency))
        {
            throw new ArgumentException($"Invalid currency code: {secondCurrency}");
        }

        // Convert from the source currency to EUR, then to the target currency
        double amountInBase = amount / exchangeRates[firstCurrency];
        double convertedAmount = amountInBase * exchangeRates[secondCurrency];
        return convertedAmount;
    }

}
