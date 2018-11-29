using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DimensionalPriceRunner.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<Result> Results { get; set; }
        public enum Valuta { DKK, USD, EUR }
        public Valuta ActiveValuta { get; set; }

        public enum Airlines { SAS, BritishAirways }

        // Airline Links should be in 2:1 format
        private static readonly Dictionary<Airlines, string> AirlineDictionary = new Dictionary<Airlines, string>()
        {
            { Airlines.SAS, "https://upload.wikimedia.org/wikipedia/commons/thumb/3/33/Scandinavian_Airlines_logo.svg/1280px-Scandinavian_Airlines_logo.svg.png"},
            { Airlines.BritishAirways, "https://www.alternativeairlines.com/images/global/airlinelogos/ba_logo.gif"}
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
            public int ID { get; set; }
            public string OS { get; set; }
            public string VPNLocation { get; set; }
            public Ticket Ticket { get; set; }

            public Result(int id, string os, string vpnLoc, Ticket ticket)
            {
                this.ID = id;
                this.OS = os;
                this.VPNLocation = vpnLoc;
                this.Ticket = ticket;
            }
        }



        public void OnGet()
        {
            ActiveValuta = Valuta.DKK;

            Result one = new Result(1, "Windows 7", "Denmark", new Ticket(ConvertToValuta(ActiveValuta, 1000), AirlineDictionary[Airlines.SAS]));
            Result two = new Result(2, "Windows 8", "USA", new Ticket(ConvertToValuta(ActiveValuta, 2000), AirlineDictionary[Airlines.BritishAirways]));
            Result three = new Result(3, "Windows 10", "Tyskland", new Ticket(ConvertToValuta(ActiveValuta, 3000), AirlineDictionary[Airlines.SAS]));
            Result four = new Result(4, "iOS", "USA", new Ticket(ConvertToValuta(ActiveValuta, 4000), AirlineDictionary[Airlines.BritishAirways]));

            Results = new List<Result>() { one, two, three, four };









        }

        // TODO: Is default Valuta always the same? This method asumes start valuta is always in USD
        private decimal ConvertToValuta(Valuta valuta, decimal price)
        {
            switch (valuta)
            {
                case Valuta.DKK:
                    return Math.Round(price * 6.61244462m, 2);
                case Valuta.USD:
                    return price;
                case Valuta.EUR:
                    return Math.Round(price * 0.886194857m, 2);
                default:
                    throw new Exception("Valuta not defined");
            }
        }


    }
}

