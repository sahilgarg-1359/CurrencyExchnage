using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("[Controllers]")]
public class FetchExchangeRate : ControllerBase
{
    private const string FixerApiKey = "20b8a5d34f31e82e810157230e3a7d53";
    private const string BaseUrl = "http://data.fixer.io/api/";
    private Dictionary<string, double> exchangeRates;

    public async Task LoadRatesAsync(string date)
    {

        string url = $"{BaseUrl}{date}?access_key={FixerApiKey}";
        Console.WriteLine(url);

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Unable to fetch exhange rate: {response.StatusCode}");
            }

            string responseData = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(responseData);

            if (json.success != true)
            {
                throw new Exception($"API Error: {json.error.type}");
            }

            // Deserialize rates into a dictionary
            exchangeRates = JsonConvert.DeserializeObject<Dictionary<string, double>>(JsonConvert.SerializeObject(json.rates));
        }
    }

    public double Convert(string firstCurrency, string secondCurrency, double amount)
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


