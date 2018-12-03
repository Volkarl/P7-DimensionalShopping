using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace DimensionalPriceRunner.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<Result> Results { get; set; }
        public enum Currency { DKK, USD, EUR }
        public Currency ActiveCurrency { get; set; }



        public enum Airlines { Default, SAS, BritishAirways, test }

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

            public Ticket(decimal price, string airline)
            {
                this.Price = price;
                this.Airline = airline;
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
            ActiveCurrency = Currency.DKK;






        }



        public void OnPost()
        {
            var searchInput = Request.Form["search"];
            ViewData["search-input"] = searchInput;

            if (searchInput == "test@mail.com")
            {


                Result one = new Result("a" + Guid.NewGuid(),
                    Airlines.SAS.ToString(),
                    "Windows 7", "Denmark",
                    new Ticket(ConvertToCurrency(ActiveCurrency, 1000), AirlineDictionary[Airlines.SAS]));

                Result two = new Result("a" + Guid.NewGuid(),
                    Airlines.BritishAirways.ToString(),
                    "Windows 8", "USA",
                    new Ticket(ConvertToCurrency(ActiveCurrency, 2000), AirlineDictionary[Airlines.BritishAirways]));

                Result three = new Result("a" + Guid.NewGuid(),
                    Airlines.SAS.ToString(),
                    "Windows 10", "Tyskland",
                    new Ticket(ConvertToCurrency(ActiveCurrency, 3000), AirlineDictionary[Airlines.SAS]));

                Result four = new Result("a" + Guid.NewGuid(),
                    Airlines.BritishAirways.ToString(),
                    "iOS", "USA",
                    new Ticket(ConvertToCurrency(ActiveCurrency, 4000), AirlineDictionary[Airlines.BritishAirways]));


                Result one2 = new Result("a" + Guid.NewGuid(),
                    Airlines.SAS.ToString(),
                    "Windows 7", "Denmark",
                    new Ticket(ConvertToCurrency(ActiveCurrency, 5000), AirlineDictionary[Airlines.SAS]));

                Result two2 = new Result("a" + Guid.NewGuid(),
                    Airlines.BritishAirways.ToString(),
                    "Windows 8", "USA",
                    new Ticket(ConvertToCurrency(ActiveCurrency, 6000), AirlineDictionary[Airlines.BritishAirways]));

                Result three2 = new Result("a" + Guid.NewGuid(),
                    Airlines.SAS.ToString(),
                    "Windows 10", "Tyskland",
                    new Ticket(ConvertToCurrency(ActiveCurrency, 7000), AirlineDictionary[Airlines.SAS]));

                Result four2 = new Result("a" + Guid.NewGuid(),
                    Airlines.BritishAirways.ToString(),
                    "iOS", "USA",
                    new Ticket(ConvertToCurrency(ActiveCurrency, 8000), AirlineDictionary[Airlines.BritishAirways]));


                Results = new List<Result>() {one, two, three, four, one2, two2, three2, four2};



            }
            else
            {
                
            }

            

        }







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

