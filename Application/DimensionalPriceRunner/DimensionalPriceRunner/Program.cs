using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
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
        public enum Language { English, Dansk }


        // flags used taken from here: http://flag-icon-css.lip.is/
        public static readonly Dictionary<Language, string> LanguageFlagDictionary = new Dictionary<Language, string>()
        {
            { Language.Dansk, "https://lipis.github.io/flag-icon-css/flags/4x3/dk.svg"},
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
            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            client.Timeout = new TimeSpan(0, 0, 180);

            var stringTask = client.GetStringAsync("http://40.112.69.3/query/%22https%3A%2F%2Fwww.expedia.dk%2FFlights-Search%3Ftrip%3Doneway%26leg1%3Dfrom%253ALondon%252C%2520England%252C%2520Storbritannien%2520%28LON%29%252Cto%253AK%25C3%25B8benhavn%252C%2520Danmark%2520%28CPH%29%252Cdeparture%253A21%252F12%252F2018TANYT%26passengers%3Dadults%253A1%252Cchildren%253A0%252Cseniors%253A0%252Cinfantinlap%253AY%26options%3Dcabinclass%253Aeconomy%26mode%3Dsearch%26origref%3Dwww.expedia.dk%22/PcWindowsChrome/True/None");
            var msg = await stringTask;

            Match match = Regex.Match(msg, @"¤{Price}¤\\n([^¤]*)(?:\\n)?(?:¤|\Z)", RegexOptions.IgnoreCase);


            IndexModel.test = match.ToString();

        }


    }
}
