using Microsoft.AspNetCore.Identity;

namespace HotelBookingAPI_Project.Models
{
    public class User:IdentityUser
    {
        public int Id {  get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Customer";
    }
}
