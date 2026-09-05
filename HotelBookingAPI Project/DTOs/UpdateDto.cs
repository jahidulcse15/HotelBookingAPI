using System.ComponentModel.DataAnnotations;

namespace HotelBookingAPI_Project.DTOs
{
    public class UpdateDto
    {
        public int RoomNumber { get; set; }
        [Range(1, double.MaxValue)]
        public decimal PricePerNight { get; set; }
        public string RoomType { get; set; }
        public bool IsAvaiable { get; set; }
        public int HotelId { get; set; }
    }
}
