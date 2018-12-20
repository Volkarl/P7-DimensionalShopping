using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DimensionalPriceRunner
{
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

        public Ticket(decimal price, string airline, string[] rating, string leg, string takeoffTime, 
            string flightLength, string airports, string stops, string ticketType, string ticketUrl)
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
}
