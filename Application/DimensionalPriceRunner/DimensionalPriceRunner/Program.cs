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


        public static async Task<Result> ProcessFlightSearch(string flightSearchUrl, Location location, string userAgent, int tryNo = 0)
        {
            string uri = Uri.EscapeDataString(flightSearchUrl);
            var vpnUrl = LocationIPDictionary[location];
            var queryUrl = "http://" + vpnUrl + "/query/" + "%22" + uri + "%22" + "/" + userAgent + "/True";

            return await QueryBackend(queryUrl, userAgent, location.ToString());
        }

        public static async Task<Result> QueryBackend(string queryUrl, string userAgent, string location, int tryNo = 0)
        {
            var stringTask = client.GetStringAsync(queryUrl);
            string json = await stringTask;
            JObject dictionary = JObject.Parse(json);
            var backendResult = dictionary["result"];

            if (backendResult.ToString() == String.Empty && tryNo < 3) // Max three tries
            {
                var otherTry = QueryBackend(queryUrl, userAgent, location, ++tryNo);
                otherTry.Wait();
                return otherTry.Result;
            }
            else return Result.BuildResult(backendResult.ToString(), userAgent, location);
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
