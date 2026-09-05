using HotelBookingAPI_Project.Models;

namespace HotelBookingAPI_Project.DTOs
{
    public class BookingCreateDto
    {
        public int RoomId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        //public DateTime? Bookingdate { get; set; } = DateTime.Now;
        //public string Status { get; set; } = "Confirmed";
    }
}
