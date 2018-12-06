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
            //ActiveCurrency = Currency.USD;
            //ActiveLanguage = Language.English;
            SearchBarMarginTop = 25;

        }


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


            //ActiveCurrency = (Currency)Enum.Parse(typeof(Currency), te);

            //var te = Request.Form["selected-currency"];
            //ActiveCurrency = (Currency)Enum.Parse(typeof(Currency), te);


            SearchBarMarginTop = 1;

            //var curr = Request.Form["selected-currency"];
            //var content = (Currency)Enum.Parse(typeof(Currency), curr);
            //ActiveCurrency = content;

            if (searchInput == "https://www.google.dk/")
            {

                Result one = new Result("a" + Guid.NewGuid(),
                    Airlines.SAS.ToString(),
                    "Windows 7", "Denmark",
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
                    "Windows 7", "Denmark",
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
                    "Windows 7", "Denmark",
                    new Ticket(ConvertToCurrency(ActiveCurrency, 3000),
                        AirlineDictionary[Airlines.Default],
                        "0,4".Split(new[] { ',', '.' }),
                        "Cityjet",
                        "10.10 - 15.20",
                        "5 t. 10 min.",
                        "AMS - AAL",
                        "1 mellemlanding (CPH)",
                        "Returrejse",
                        "https://www.google.com/"));

                Result four = new Result("a" + Guid.NewGuid(),
                    Airlines.SAS.ToString(),
                    "Windows 7", "Denmark",
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
                    "Windows 7", "Denmark",
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



                Results = new List<Result>() {one, two, three, four, five};



            }
            else
            {
                S = "We did not find any plane tickets";
                S2 = "please verify you entered a valid web address";
                NoResultImg = "https://image.flaticon.com/icons/png/512/885/885161.png";


                //Program.ProcessRepositories().Wait();

                //S2 = test;

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




    }
}

