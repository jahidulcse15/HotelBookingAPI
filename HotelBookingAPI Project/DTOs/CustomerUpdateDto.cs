using System.ComponentModel.DataAnnotations;

namespace HotelBookingAPI_Project.DTOs
{
    public class CustomerUpdateDto
    {
        [Required(ErrorMessage = "Customer Name is Required.")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Enter a Valid Email Address.")]
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
