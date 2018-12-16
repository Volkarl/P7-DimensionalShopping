using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static DimensionalPriceRunner.Pages.IndexModel;
using static DimensionalPriceRunner.Program;

namespace DimensionalPriceRunner
{
    public class Result
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string OS { get; set; }
        public string VPNLocation { get; set; }
        public Ticket Ticket { get; set; }

        public Result(string id, string name, string os, string vpnLoc, Ticket ticket)
        {
            this.ID = id;
            this.Name = name;
            this.OS = os;
            this.VPNLocation = vpnLoc;
            this.Ticket = ticket;
        }

        public static Result BuildResult(string resultString, string userAgent, string vpnLoc)
        {
            Match priceCurrency = Regex.Match(resultString, @"\|Price\|[\\n\s]*([^\d]*)([\d\.]*)\s*([^\s\|]*)\s*");
            // Valuata is either in the first or the third match group, price is always in the second
            string currencySymbol = GetMatchValue(priceCurrency, 1) != "" ? GetMatchValue(priceCurrency, 1) : GetMatchValue(priceCurrency, 3);
            decimal price = 0;
            decimal.TryParse(GetMatchValue(priceCurrency, 2), out price);
            Currency currency = Currency.USD;
            switch (currencySymbol.Trim())
            {
                case "DKK": currency = Currency.DKK; break;
                case "€": currency = Currency.EUR; break;
                case "£": currency = Currency.GBP; break;
                case "$": currency = Currency.USD; break;
                case "": currency = Currency.USD; break; // If we didn't get any proper results from our crawling
                default: throw new NotSupportedException(currencySymbol);
            }

            Match dur = Regex.Match(resultString, @"\|Duration\|[\\n\s]*([\d]*):([\d]*)\s-\s([\d]*):([\d]*)\s*");
            // Match group 1: TakeoffHour, 2: TakeoffMinutes, 3: LandingHour, 4: LandingMinutes
            string flightDuration = !dur.Success ? "" : $"{dur.Groups[1].Value}:{dur.Groups[2].Value} - {dur.Groups[3].Value}:{dur.Groups[4].Value}";

            Match airlineMatch = Regex.Match(resultString, @"\|Airline\|[\\n\s]*([^|]*)");
            string airlineStr = GetMatchValue(airlineMatch, 1).Replace(" ", "");
            Airlines airline = Airlines.Default;
            Enum.TryParse(airlineStr, out airline);

            Match ratingMatch = Regex.Match(resultString, @"\|Rating\|[\\n\s]*\((\d*.\d*)\/(\d*)\)\s*");
            string[] correctedRating = CorrectRating(ratingMatch);
            // Has to be divisible by 5 instead of the 10 that we get the value in

            Match timeStops = Regex.Match(resultString, @"\|Time\|[\\n\s]*([\d]*[a-zA-Z]+\s[\d]*[a-zA-Z]+)\s*\((.*)\)");
            string time = GetMatchValue(timeStops, 1);
            string stops = GetMatchValue(timeStops, 2);

            Match airportMatch = Regex.Match(resultString, @"\|Leg\|[\\n\s]*(.*)");
            string airports = GetMatchValue(airportMatch, 1);
            // Match the remaining characters

            return BuildResult(airline, userAgent, vpnLoc, currency, price, correctedRating, flightDuration, 
                time, airports, stops, "", "");
        }

        private static string[] CorrectRating(Match ratingMatch)
        {
            if (!ratingMatch.Success) return new[] { "0", "0" };
            // We get data like: 3.2 / 10
            double res;
            double.TryParse(GetMatchValue(ratingMatch, 1).Replace(',', '.'), out res); // We capture 3.2 in a double
            res = res / 2; // It's halved to make it divisible by 5 instead of the 10, the example is now 1,6 / 5
            return res.ToString("N1").Split(new[] { '.', ',' }); // We split it into a string array with two arguments
        }

        private static string GetMatchValue(Match match, int groupNo)
        {
            return match.Success ? match.Groups[groupNo].Value : "";
        }

        public static Result BuildResult(Airlines airline, string os, string vpnLoc, Currency currency, decimal price, string[] rating,
            string flightDuration, string flightLength, string airports, string stops, string ticketType, string ticketUrl)
        {
            return
                new Result("a" + Guid.NewGuid(), // Not allowed to start on a number, so we select an arbitrary character
                airline.ToString(),
                os,
                vpnLoc,
                new Ticket(ConvertToActiveCurrency(currency, price),
                    AirlineDictionary[airline],
                    rating,
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
    }
}
