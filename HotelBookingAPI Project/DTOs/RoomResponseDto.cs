namespace HotelBookingAPI_Project.DTOs
{
    public class RoomResponseDto
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public string RoomType { get; set; }
        public bool IsAvaiable { get; set; }
        public int HotelId { get; set; }
    }
}
