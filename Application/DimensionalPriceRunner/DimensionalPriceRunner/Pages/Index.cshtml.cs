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
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DimensionalPriceRunner.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<Result> Results { get; set; }

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

        public enum Airlines { Default, NotFound, SAS, BritishAirways }

        // Airline Links should be in 2:1 format
        public static readonly Dictionary<Airlines, string> AirlineDictionary = new Dictionary<Airlines, string>()
        {
            { Airlines.SAS, "https://upload.wikimedia.org/wikipedia/commons/thumb/3/33/Scandinavian_Airlines_logo.svg/1280px-Scandinavian_Airlines_logo.svg.png"},
            { Airlines.BritishAirways, "https://www.alternativeairlines.com/images/global/airlinelogos/ba_logo.gif"},
            { Airlines.Default, "http://betlosers.com/images/stories/flexicontent/leagueimages/m_fld34_NoImageAvailable.jpg" },
            { Airlines.NotFound, "dist/orangeCross.PNG" }
        };

        public void OnGet()
        {
            // Moves the Logo/Search html element more to the center of the screen, when first loading the page.
            SearchBarMarginTop = 20;
        }
        
        public void OnPost()
        {
            // Gets the URL search input from the user.
            var searchInput = Request.Form["search"];
            // Sets the searchBar inner value to the URL from input
            ViewData["search-input"] = searchInput;

            string selectedCurrency = Request.Form["selected-currency"];
            Currency activeCurrency;
            if (Enum.TryParse(selectedCurrency, out activeCurrency))
            {
                ActiveCurrency = activeCurrency;
            }

            string selectedLanguage = Request.Form["selected-language"];
            Language activeLanguage;
            if (Enum.TryParse(selectedLanguage, out activeLanguage))
            {
                ActiveLanguage = activeLanguage;
            }

            // Moves the Logo/Search html element to the top of the page, to make room for the result container.
            SearchBarMarginTop = 1;

            if (String.IsNullOrEmpty(searchInput))
            {
                return;
            }

            List<Task<Result>> tasks = new List<Task<Result>> {
                ProcessFlightSearch(searchInput, Location.USA, "PcWindowsChrome"),
                ProcessFlightSearch(searchInput, Location.SouthAfrica, "PcWindowsChrome"),
                ProcessFlightSearch(searchInput, Location.USA, "PhoneIOSSafari"),
                ProcessFlightSearch(searchInput, Location.SouthAfrica, "PhoneIOSSafari")
            };
            // Start all tasks 
            var allResults = Task.WhenAll(tasks);

            // Wait for all tasks
            allResults.Wait();
            Results = new List<Result>(allResults.Result.OrderBy(x => x.Ticket.Price));

            if (!Results.Any())
            {
                Results = null;
                NoResultStringHead = "We did not find any plane tickets";
                NoResultStringBody = "Please verify you entered a valid web address";
                NoResultImg = "https://image.flaticon.com/icons/png/512/885/885161.png";
            }



            



            //Finds and sets UserLocation based on their public IP
            string userIp = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            UserLocation = GetUserCountryByIp(userIp);

            //Finds and sets UserOS based on their User-Agent string
            string userAgent = Request.Headers["User-Agent"];
            UserOS = FindUserOS(userAgent);


        }

        public static string test { get; set; }

        public int SearchBarMarginTop { get; set; }
        public string NoResultImg { get; set; }

        public string NoResultStringHead { get; set; }
        public string NoResultStringBody { get; set; }

        public string UserLocation { get; set; }
        public string UserOS { get; set; }

        public string FindUserOS(string userAgent)
        {
            foreach (var key in OSUserAgentStrings.Keys)
            {
                if (userAgent.Contains(key))
                {
                    return OSUserAgentStrings[key];
                }
            }
            return "";
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

        //public void MakeTestResults()
        //{
        //    Result one = new Result("a" + Guid.NewGuid(),
        //        Airlines.SAS.ToString(),
        //        "Windows 8", "United States",
        //        new Ticket(ConvertToActiveCurrency(Currency.DKK, 1000),
        //            AirlineDictionary[Airlines.SAS],
        //            "3,5".Split(new[] { ',', '.' }),
        //            "Cityjet",
        //            "10.10 - 15.20",
        //            "5 t. 10 min.",
        //            "AMS - AAL",
        //            "1 mellemlanding (CPH)",
        //            "Returrejse",
        //            "https://www.google.com/"));


        //    Result two = new Result("a" + Guid.NewGuid(),
        //        Airlines.SAS.ToString(),
        //        "Windows 10", "United States",
        //        new Ticket(ConvertToActiveCurrency(Currency.DKK, 2000),
        //            AirlineDictionary[Airlines.BritishAirways],
        //            "5,0".Split(new[] { ',', '.' }),
        //            "Cityjet",
        //            "10.10 - 15.20",
        //            "5 t. 10 min.",
        //            "AMS - AAL",
        //            "1 mellemlanding (CPH)",
        //            "Returrejse",
        //            "https://www.google.com/"));

        //    Result three = new Result("a" + Guid.NewGuid(),
        //        Airlines.SAS.ToString(),
        //        "Windows 10", "United States",
        //        new Ticket(ConvertToActiveCurrency(Currency.DKK, 3000),
        //            AirlineDictionary[Airlines.SAS],
        //            "3,5".Split(new[] { ',', '.' }),
        //            "Cityjet",
        //            "10.10 - 15.20",
        //            "5 t. 10 min.",
        //            "AMS - AAL",
        //            "1 mellemlanding (CPH)",
        //            "Returrejse",
        //            "https://www.google.com/"));

        //    Result four = new Result("a" + Guid.NewGuid(),
        //        Airlines.SAS.ToString(),
        //        "Windows 8", "Denmark",
        //        new Ticket(ConvertToActiveCurrency(Currency.DKK, 4000),
        //            AirlineDictionary[Airlines.Default],
        //            "1,4".Split(new[] { ',', '.' }),
        //            "Cityjet",
        //            "10.10 - 15.20",
        //            "5 t. 10 min.",
        //            "AMS - AAL",
        //            "1 mellemlanding (CPH)",
        //            "Returrejse",
        //            "https://www.google.com/"));

        //    Result five = new Result("a" + Guid.NewGuid(),
        //        Airlines.SAS.ToString(),
        //        "Windows 10", "Denmark",
        //        new Ticket(ConvertToActiveCurrency(Currency.DKK, 5000),
        //            AirlineDictionary[Airlines.SAS],
        //            "4,9".Split(new[] { ',', '.' }),
        //            "Cityjet",
        //            "10.10 - 15.20",
        //            "5 t. 10 min.",
        //            "AMS - AAL",
        //            "1 mellemlanding (CPH)",
        //            "Returrejse",
        //            "https://www.google.com/"));



        //    Results = new List<Result>() { one, two, three, four, five };

        //}
    }
}

