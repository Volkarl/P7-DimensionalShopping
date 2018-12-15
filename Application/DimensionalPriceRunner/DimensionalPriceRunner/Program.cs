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

            var stringTask = client.GetStringAsync("http://94.245.95.6/query/" + "%22" + uri + "%22" + "/PcWindowsChrome/True");
            //var json = await stringTask;
            //JObject dictionary = JObject.Parse(json);
           // var backendResult = "Result: " + dictionary["result"];

            var testResult = "|Price| 190 € |Duration| 18:15 - 09:05 |Airline| SAS |Rating| (3.2/10) |Time| 14h 50m (2 stops) |Leg| AAL - AMS - CDG";


            Match price = Regex.Match(testResult, @"\|Price\|\s*([^\d]*)([\d]*)\s*([^\s\|]*)\s*");
            // Valuata is either in the first or the third match group, price is always in the second
            string currencySymbol = price.Groups[1].Value != "" ? price.Groups[1].Value : price.Groups[3].Value;
            Currency currency = Currency.USD;
            switch (currencySymbol)
            {
                case "DKK": currency = Currency.DKK; break;
                case "€": currency = Currency.EUR; break;
                case "£": currency = Currency.GBP; break;
                case "$": currency = Currency.USD; break;
                default: throw new NotSupportedException(currencySymbol); 
            }


            // TODO: CHANGE TESTRESULT TO BACKENDRESULT

            Match dur = Regex.Match(testResult, @"\|Duration\|\s*([\d]*):([\d]*)\s-\s([\d]*):([\d]*)\s*");
            // Match group 1: TakeoffHour, 2: TakeoffMinutes, 3: LandingHour, 4: LandingMinutes
            string flightDuration = !dur.Success ? "" : $"{dur.Groups[1].Value}:{dur.Groups[2].Value} - {dur.Groups[3].Value}:{dur.Groups[4].Value}";

            Match airlineMatch = Regex.Match(testResult, @"\|Airline\|\s*([^|]*)");
            string airline = GetMatchValue(airlineMatch, 1).Trim();

            Match ratingMatch = Regex.Match(testResult, @"\|Rating\|\s*\((\d*).(\d*)\/(\d*)\)\s*");
            string correctedRating = CorrectRating(ratingMatch);
            // Has to be divisible by 5 instead of the 10 that we get the value in

            Match timeStops = Regex.Match(testResult, @"\|Time\|\s*([\d]*h\s[\d]*m)\s*\((.*)\)");
            string time = GetMatchValue(timeStops, 1);
            string stops = GetMatchValue(timeStops, 2);

            Match airportMatch = Regex.Match(testResult, @"\|Leg\|\s*(.*)");
            string airports = GetMatchValue(airportMatch, 1);
            // Match the remaining characters

            string resultString = flightDuration + airline + correctedRating + time + stops + airports;


            IndexModel.test = resultString;
        }

        private static string CorrectRating(Match ratingMatch)
        {
            if (!ratingMatch.Success) return "";
            double res = 0;
            double.TryParse(GetMatchValue(ratingMatch, 1) + "." + GetMatchValue(ratingMatch, 2), out res);
            return (res / 2).ToString();
        }

        public static string GetMatchValue(Match match, int groupNo)
        {
            return match.Success ? match.Groups[groupNo].Value : "";
        }

        public static Result buildResult(Airlines airline, string os, string vpnLoc, Currency currency, decimal price, string rating,
            string flightDuration, string flightLength, string airports, string stops, string ticketType, string ticketUrl)
        {
            return
                new Result("a" + Guid.NewGuid(), // Not allowed to start on a number, so we select an arbitrary character
                airline.ToString(),
                os,
                vpnLoc,
                new Ticket(ConvertToActiveCurrency(currency, price),
                    AirlineDictionary[airline],
                    rating.Split(new[] { ',', '.' }), //TODO convert to int, divide by two, convert back?
                    "",
                    flightDuration,
                    flightLength,
                    airports,
                    stops,
                    ticketType,
                    ticketUrl));
        }

        public static decimal ConvertToActiveCurrency(Currency originCurrency, decimal price)
        {
            if (originCurrency == ActiveCurrency)
                return price;

            price = ConvertToUSD(originCurrency, price);

            switch (ActiveCurrency)
            {
                case Currency.DKK:
                    return Math.Round(price * 6.61244462m, 2);
                case Currency.USD:
                    return price;
                case Currency.EUR:
                    return Math.Round(price * 0.886194857m, 2);
                case Currency.GBP:
                    return Math.Round(price * 0.795780m, 2);
                default:
                    return price;
            }
        }

        private static decimal ConvertToUSD(Currency originCurrency, decimal price)
        {
            switch (originCurrency)
            {
                case Currency.DKK:
                    return Math.Round(price * 0.151343m, 2);
                case Currency.USD:
                    return price;
                case Currency.EUR:
                    return Math.Round(price * 1.12994m, 2);
                case Currency.GBP:
                    return Math.Round(price * 1.25684m, 2);
                default:
                    return price;
            }
        }

        public enum Airlines { Default, SAS, BritishAirways }

        // Airline Links should be in 2:1 format
        public static readonly Dictionary<Airlines, string> AirlineDictionary = new Dictionary<Airlines, string>()
        {
            { Airlines.SAS, "https://upload.wikimedia.org/wikipedia/commons/thumb/3/33/Scandinavian_Airlines_logo.svg/1280px-Scandinavian_Airlines_logo.svg.png"},
            { Airlines.BritishAirways, "https://www.alternativeairlines.com/images/global/airlinelogos/ba_logo.gif"},
            { Airlines.Default, "http://betlosers.com/images/stories/flexicontent/leagueimages/m_fld34_NoImageAvailable.jpg" }
        };

    }
}
