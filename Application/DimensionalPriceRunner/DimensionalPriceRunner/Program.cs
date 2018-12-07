using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DimensionalPriceRunner.Pages;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DimensionalPriceRunner
{
    public class Program
    {
        public static Currency ActiveCurrency { get; set; }
        public enum Currency { DKK, USD, EUR }

        public static Language ActiveLanguage { get; set; }
        public enum Language { English, Danish }


        // flags used taken from here: http://flag-icon-css.lip.is/
        public static readonly Dictionary<Language, string> LanguageFlagDictionary = new Dictionary<Language, string>()
        {
            { Language.Danish, "https://lipis.github.io/flag-icon-css/flags/4x3/dk.svg"},
            { Language.English, "https://lipis.github.io/flag-icon-css/flags/4x3/gb.svg"}
        };


        // Symbols taken from here: https://www.flaticon.com/packs/currency-icons-fill/1
        public static readonly Dictionary<Currency, string> CurrencySymbolDictionary = new Dictionary<Currency, string>()
        {
            { Currency.DKK, "https://image.flaticon.com/icons/svg/32/32800.svg"},
            { Currency.USD, "https://image.flaticon.com/icons/svg/33/33002.svg"},
            { Currency.EUR, "https://image.flaticon.com/icons/svg/32/32719.svg"}
        };



        private static readonly HttpClient client = new HttpClient();

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static async Task ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            var stringTask = client.GetStringAsync("https://api.github.com/orgs/dotnet/repos");

            var msg = await stringTask;
            IndexModel.test = msg;
        }


    }
}
