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
using Newtonsoft.Json.Linq;
using static DimensionalPriceRunner.Pages.IndexModel;

namespace DimensionalPriceRunner
{
    public class Program
    {
        public static Currency ActiveCurrency { get; set; }
        public enum Currency { DKK, USD, EUR, GBP }

        public static Language ActiveLanguage { get; set; }
        public enum Language { English, Dansk }

        public enum Location { USA, SouthAfrica }

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

        public static readonly Dictionary<Location, string> LocationIPDictionary = new Dictionary<Location, string>()
        {
            { Location.SouthAfrica, "94.245.93.167"},
            { Location.USA, "94.245.95.6"},
        };

        private static readonly HttpClient client = new HttpClient();


        public static async Task<Result> ProcessFlightSearch(string flightSearchUrl)
        {
            string uri = Uri.EscapeDataString(flightSearchUrl);

            var userAgent = "PcWindowsChrome";
            var location = Location.USA;
            var vpnUrl = LocationIPDictionary[location];

            var stringTask = client.GetStringAsync("http://" + vpnUrl + "/query/" + "%22" + uri + "%22" + "/" + userAgent + "/True");
            //var testResult = "|Price| 190 € |Duration| 18:15 - 09:05 |Airline| SAS |Rating| (3.2/10) |Time| 14h 50m (2 stops) |Leg| AAL - AMS - CDG";

            string json = await stringTask; // TODO REMOVE THIS AWAIT I THINK IT REMOVES ALL PARALELLISM
            JObject dictionary = JObject.Parse(json);
            var backendResult = dictionary["result"];

            return Result.BuildResult(backendResult.ToString(), "PLACEHOLDER UA", "");
        }


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
    }
}
