using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelListingSystem.ViewModel
{
    public class ReservationListVM
    {
        public string HotelName { get; set; }
        public string RoomName { get; set; }
        public int RoomQuantity { get; set; }
        public DateTime? Checkin { get; set; }
        public DateTime? Checkout { get; set; }
        public decimal? Cost { get; set; }
    }
}