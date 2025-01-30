using ExchangeAPIservice;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("[Controllers]")]
public class FetchExchangeRate : ControllerBase
{

    public async Task<Dictionary<string, decimal>> LoadRatesAsync(string date)
    {
        string url = $"{AppSettingConfig.Base_Url}{date}?access_key={AppSettingConfig.Api_Key}";
        Console.WriteLine(url);

        using HttpClient client = new();

        HttpResponseMessage response = await client.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Unable to fetch exhange rate: {response.StatusCode}");
        }

        string responseData = await response.Content.ReadAsStringAsync();
        dynamic json = JsonConvert.DeserializeObject(responseData);

        if (json?.success != true)
        {
            throw new Exception($"API Error: {json?.error.type}");
        }


        // Deserialize rates into a dictionary
        return JsonConvert.DeserializeObject<Dictionary<string, double>>(JsonConvert.SerializeObject(json?.rates));
    }
}


