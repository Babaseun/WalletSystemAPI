﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WalletSystem.Services.Models;

namespace WalletSystem.Services.Data.Services
{
    public static class HttpClientService
    {
        public static async Task<Dictionary<string, decimal>> LoadCurrencies()
        {
            var client = new HttpClient();
            const string url = "http://data.fixer.io/api/latest?access_key=3073cb802b92bc621d2c3208dac74f6b";

            var response = await client.GetAsync(url);

            var json = await response.Content.ReadAsStringAsync();
            var myDeserializedClass = JsonConvert.DeserializeObject<Root>(json);


            return myDeserializedClass.Rates;
        }
    }


}

