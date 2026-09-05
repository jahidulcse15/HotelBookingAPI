using HotelBookingAPI_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingAPI_Project.DTOs
{
    public class BookingCreateDto
    {
        [Required(ErrorMessage ="Booking Id is requird.")]
        [Range(1,int.MaxValue)]
        public int RoomId { get; set; }
        [Required(ErrorMessage ="customer is is required")]
        [Range(1,int.MaxValue)]
        public int CustomerId { get; set; }
        [Required(ErrorMessage ="checkin date is required")]
        public DateTime? CheckInDate { get; set; }
        [Required(ErrorMessage ="checkout date is required")]
        public DateTime? CheckOutDate { get; set; }
        //public DateTime? Bookingdate { get; set; } = DateTime.Now;
        //public string Status { get; set; } = "Confirmed";
    }
}
