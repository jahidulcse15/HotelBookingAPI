namespace HotelBookingAPI_Project.DTOs
{
    public class BookingResponseDto
    {
        public int Id { get; set; }

        public string HotelName { get; set; } = string.Empty;
        public int RoomNumber { get; set; } 
        public decimal PricePerNight { get; set; }
        public decimal TotalPrice { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public DateTime? BookingDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
