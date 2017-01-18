using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OmniChannel.VideoChatConnector.APIs.Rentals
{
    public class RentalsList
    {
        public IEnumerable<Rental> Rentals { get; set; }
        public RentalsFilter Filters { get; set; }
    }
}