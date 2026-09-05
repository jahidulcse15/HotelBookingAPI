namespace HotelBookingAPI_Project.DTOs
{
    public class BookingUpdateDto
    {
        public int RoomId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
    }
}
