using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotelListingSystem.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public int? HotelId { get; set; }
        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }

        [NotMapped]
        public string HotelName { get; set; }

        public int? RoomId { get; set; }
        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        public int? HotelUserId { get; set; }
        [ForeignKey("HotelUserId")]
        public HotelUsers HotelUser { get; set; }

        public int NoOfRooms { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public decimal TotalCost { get; set; }
        public decimal TotalFees { get; set; }
        public bool? Booked { get; set; }
    }
}