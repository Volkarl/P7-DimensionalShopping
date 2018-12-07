using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using static DimensionalPriceRunner.Program;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DimensionalPriceRunner.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<Result> Results { get; set; }



        public static string test { get; set; }


        public enum Airlines { Default, SAS, BritishAirways }

        // Airline Links should be in 2:1 format
        private static readonly Dictionary<Airlines, string> AirlineDictionary = new Dictionary<Airlines, string>()
        {
            { Airlines.SAS, "https://upload.wikimedia.org/wikipedia/commons/thumb/3/33/Scandinavian_Airlines_logo.svg/1280px-Scandinavian_Airlines_logo.svg.png"},
            { Airlines.BritishAirways, "https://www.alternativeairlines.com/images/global/airlinelogos/ba_logo.gif"},

            { Airlines.Default, "http://betlosers.com/images/stories/flexicontent/leagueimages/m_fld34_NoImageAvailable.jpg" } 
            // tag højde for sporg? http://betlosers.com/images/stories/flexicontent/leagueimages/m_fld34_NoImageAvailable.jpg
            // https://premium.wpmudev.org/blog/wp-content/uploads/2012/06/no.image_.600x300.png
        };


        private static readonly Dictionary<string, string> OSUserAgentStrings = new Dictionary<string, string>()
        {
            { "Windows NT 5.1", "Windows XP" },
            { "Windows NT 6.0", "Windows Vista" },
            { "Windows NT 6.1", "Windows 7" },
            { "Windows NT 6.2", "Windows 8" },
            { "Windows NT 10.0", "Windows 10" },
            { "Linux", "Linux" },
            { "Mac OS", "iOS" },
            { "Android", "Android" }           
        };

        public readonly Dictionary<string, string> OSIconDictionary = new Dictionary<string, string>()
        {
            { "Windows XP", "fa-windows" },
            { "Windows Vista", "fa-windows" },
            { "Windows 7", "fa-windows" },
            { "Windows 8", "fa-windows" },
            { "Windows 10", "fa-windows" },
            { "Linux", "fa-linux"},
            { "iOS", "fa-apple" },
            { "Android", "fa-android" }
        };




        public class Ticket
        {
            public decimal Price { get; set; }
            public string Airline { get; set; }
            public string[] Rating { get; set; }
            public string Leg { get; set; }
            public string TakeoffTime { get; set; }
            public string FlightLength { get; set; }
            public string Airports { get; set; }
            public string Stops { get; set; }
            public string TicketType { get; set; }
            public string TicketUrl { get; set; }

            public Ticket(decimal price, string airline, string[] rating, string leg, string takeoffTime, string flightLength, string airports, string stops, string ticketType, string ticketUrl)
            {
                this.Price = price;
                this.Airline = airline;
                this.Rating = rating;
                this.Leg = leg;
                this.TakeoffTime = takeoffTime;
                this.FlightLength = flightLength;
                this.Airports = airports;
                this.Stops = stops;
                this.TicketType = ticketType;
                this.TicketUrl = ticketUrl;
            }
        }

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
        }
        

        public void OnGet()
        {
            // Moves the Logo/Search html element more to the center of the screen, when first loading the page.
            SearchBarMarginTop = 25;
        }






        public string UserLocation { get; set; }

        public void OnPost()
        {
            var searchInput = Request.Form["search"];
            ViewData["search-input"] = searchInput;


            



            string te = Request.Form["test"];
            if (te != null)
            {
                ActiveCurrency = (Currency)Enum.Parse(typeof(Currency), te);
            }

            string te2 = Request.Form["test2"];
            if (te2 != null)
            {
                ActiveLanguage = (Language)Enum.Parse(typeof(Language), te2);
            }

            // Moves the Logo/Search html element to the top of the page, to make room for the result container.
            SearchBarMarginTop = 1;


            if (searchInput == "https://www.google.dk/")
            {
                MakeTestResults();


                UserLocation = GetUserCountryByIp("162.210.211.225");
                //UserLocation = GetUserCountryByIp(HttpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());

                FindUserMinMaxPrice();
            }
            else
            {
                S = "We did not find any plane tickets";
                S2 = "please verify you entered a valid web address";
                NoResultImg = "https://image.flaticon.com/icons/png/512/885/885161.png";


                Program.ProcessRepositories().Wait();

                S2 = test;

            }

            

        }

        public int SearchBarMarginTop { get; set; }
        public string NoResultImg { get; set; }

        public string S { get; set; }
        public string S2 { get; set; }








        // TODO: Is default Valuta always the same? This method asumes start valuta is always in USD
        private decimal ConvertToCurrency(Currency currency, decimal price)
        {
            switch (currency)
            {
                case Currency.DKK:
                    return Math.Round(price * 6.61244462m, 2);
                case Currency.USD:
                    return price;
                case Currency.EUR:
                    return Math.Round(price * 0.886194857m, 2);
                default:
                    throw new Exception("Currency not defined");
            }
        }








        public string UserOS { get; set; }
        public decimal UserMinPrice { get; set; }
        public decimal UserMaxPrice { get; set; }

        // Sets the lowest and highest price for the user's current system (OS and IP Location) -> Taken from the user agent string
        public void FindUserMinMaxPrice()
        {
            string userOS = Request.Headers["User-Agent"];

            foreach (var key in OSUserAgentStrings.Keys)
            {
                if (userOS.Contains(key))
                {
                    UserOS = OSUserAgentStrings[key];
                    break;
                }
            }

            if (UserOS != "" && UserLocation != "")
            {
                UserMinPrice = Results.Aggregate((minItem, nextItem) => (minItem.Ticket.Price < nextItem.Ticket.Price && (minItem.OS == UserOS) && (minItem.VPNLocation == UserLocation)) ? minItem : nextItem).Ticket.Price;

                UserMaxPrice = Results.Where(r => r.OS == UserOS && r.VPNLocation == UserLocation).Max(r => r.Ticket.Price);

                //UserMaxPrice = Results.Aggregate((maxItem, nextItem) => (maxItem.Ticket.Price > nextItem.Ticket.Price && (maxItem.OS == UserOS) && (maxItem.VPNLocation == UserLocation)) ? maxItem : nextItem).Ticket.Price;
            }
        }





        public class IpInfo
        {

            [JsonProperty("ip")]
            public string Ip { get; set; }

            [JsonProperty("hostname")]
            public string Hostname { get; set; }

            [JsonProperty("city")]
            public string City { get; set; }

            [JsonProperty("region")]
            public string Region { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            [JsonProperty("loc")]
            public string Loc { get; set; }

            [JsonProperty("org")]
            public string Org { get; set; }

            [JsonProperty("postal")]
            public string Postal { get; set; }
        }

        public static string GetUserCountryByIp(string ip)
        {
            IpInfo ipInfo = new IpInfo();
            try
            {
                string info = new WebClient().DownloadString("http://ipinfo.io/" + ip);
                ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
            }
            catch (Exception)
            {
                ipInfo.Country = null;
            }

            return ipInfo.Country;
        }









        public void MakeTestResults()
        {
            Result one = new Result("a" + Guid.NewGuid(),
                Airlines.SAS.ToString(),
                "Windows 8", "United States",
                new Ticket(ConvertToCurrency(ActiveCurrency, 1000),
                    AirlineDictionary[Airlines.SAS],
                    "3,5".Split(new[] {',', '.'}),
                    "Cityjet",
                    "10.10 - 15.20",
                    "5 t. 10 min.",
                    "AMS - AAL",
                    "1 mellemlanding (CPH)",
                    "Returrejse",
                    "https://www.google.com/"));


            Result two = new Result("a" + Guid.NewGuid(),
                Airlines.SAS.ToString(),
                "Windows 10", "United States",
                new Ticket(ConvertToCurrency(ActiveCurrency, 2000),
                    AirlineDictionary[Airlines.BritishAirways],
                    "5,0".Split(new[] { ',', '.' }),
                    "Cityjet",
                    "10.10 - 15.20",
                    "5 t. 10 min.",
                    "AMS - AAL",
                    "1 mellemlanding (CPH)",
                    "Returrejse",
                    "https://www.google.com/"));

            Result three = new Result("a" + Guid.NewGuid(),
                Airlines.SAS.ToString(),
                "Windows 10", "United States",
                new Ticket(ConvertToCurrency(ActiveCurrency, 3000),
                    AirlineDictionary[Airlines.SAS],
                    "3,5".Split(new[] { ',', '.' }),
                    "Cityjet",
                    "10.10 - 15.20",
                    "5 t. 10 min.",
                    "AMS - AAL",
                    "1 mellemlanding (CPH)",
                    "Returrejse",
                    "https://www.google.com/"));

            Result four = new Result("a" + Guid.NewGuid(),
                Airlines.SAS.ToString(),
                "Windows 8", "Denmark",
                new Ticket(ConvertToCurrency(ActiveCurrency, 4000),
                    AirlineDictionary[Airlines.Default],
                    "1,4".Split(new[] { ',', '.' }),
                    "Cityjet",
                    "10.10 - 15.20",
                    "5 t. 10 min.",
                    "AMS - AAL",
                    "1 mellemlanding (CPH)",
                    "Returrejse",
                    "https://www.google.com/"));

            Result five = new Result("a" + Guid.NewGuid(),
                Airlines.SAS.ToString(),
                "Windows 10", "Denmark",
                new Ticket(ConvertToCurrency(ActiveCurrency, 5000),
                    AirlineDictionary[Airlines.SAS],
                    "4,9".Split(new[] { ',', '.' }),
                    "Cityjet",
                    "10.10 - 15.20",
                    "5 t. 10 min.",
                    "AMS - AAL",
                    "1 mellemlanding (CPH)",
                    "Returrejse",
                    "https://www.google.com/"));



            Results = new List<Result>() { one, two, three, four, five };

        }








    }
}

