using System.ComponentModel.DataAnnotations;

namespace HotelBookingAPI_Project.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "User Email is required.")]
        [EmailAddress(ErrorMessage = "Enter Valid Email.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password Must be between 8 to 20.")]
        public string Password { get; set; } = string.Empty;
    }
}

