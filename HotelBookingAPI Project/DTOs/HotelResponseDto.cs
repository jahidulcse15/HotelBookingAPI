using System.ComponentModel.DataAnnotations;

namespace HotelBookingAPI_Project.DTOs
{
    public class HotelResponseDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage ="email id is requird")]
        [EmailAddress(ErrorMessage ="enter valid email address")]
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
