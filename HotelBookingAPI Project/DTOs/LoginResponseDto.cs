using System.ComponentModel.DataAnnotations;

namespace HotelBookingAPI_Project.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        [Required(ErrorMessage = "User Name is required.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "User Email is required.")]
        [EmailAddress(ErrorMessage = "Enter Valid Email.")]
        public string Email { get; set; } = string.Empty;
        public string Role {  get; set; } = string.Empty;
    }
}
