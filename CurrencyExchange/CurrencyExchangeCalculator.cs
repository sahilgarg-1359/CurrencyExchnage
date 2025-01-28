using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

try
{
    Console.Write("Enter the first currency code");
    string firstCurrency = Console.ReadLine().ToUpper();

    Console.Write("Enter the second currency code");
    string secondCurrency = Console.ReadLine().ToUpper();


    Console.Write("Enter the amount you want to exchange ");
    double amount = Convert.ToDouble(Console.ReadLine());

    Console.Write("Do you want to fetch exchange rate by any specific date? ");
    string exchangeRate = Console.ReadLine().ToUpper();

    FetchExchangeRate converter = new FetchExchangeRate();
    string dato = "latest";

    if (exchangeRate == "YES")
    {
        Console.Write("Enter the Data in Formate yyyy-mm--dd: ");
        dato = Console.ReadLine();

    }

    Console.WriteLine("Fetch latest exchange rates-");
    await converter.LoadRatesAsync(dato);

    double result = converter.Convert(firstCurrency, secondCurrency, amount);

    Console.WriteLine($"\n{amount} {firstCurrency} is equal to {result:F2} {secondCurrency}.");
}
catch (Exception e)
{
    Console.WriteLine($"\nError: {e.Message}");
}
