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
        public enum Currency { DKK, USD, EUR, GBP }

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
            { Currency.EUR, "https://image.flaticon.com/icons/svg/32/32719.svg"},
            { Currency.GBP, "https://image.flaticon.com/icons/svg/33/33917.svg"}
        };



        private static readonly HttpClient client = new HttpClient();

        public static void Main(string[] args)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = new TimeSpan(0, 0, 180);

            //client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();


        public static async Task ProcessFlightSearch(string flightSearchUrl)
        {
            string uri = Uri.EscapeDataString(flightSearchUrl);

            var stringTask = client.GetStringAsync("http://94.245.93.167/query/" + "%22" + uri + "%22" + "/PcWindowsChrome/True");
            var msg = await stringTask;

            //Match match = Regex.Match(msg, @"---Price---\\n([^\\n]*)(?:\\n)?(?:---|\Z)");

            //if (match.Success)
            //{
            //    IndexModel.test = match.Groups[1].Value;
            //}
            //else
            //{
            //    IndexModel.test = msg;
            //}

            IndexModel.test = msg;

            // TryParse on valuta etc.

        }


    }
}
