using HotelBookingAPI_Project.Data;
using HotelBookingAPI_Project.DTOs;
using HotelBookingAPI_Project.Models;
using HotelBookingAPI_Project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly ApplicationDbcontext _context;
        private readonly TokenServices _tokenServices;

        public AuthController(ApplicationDbcontext context, TokenServices tokenServices)
        {
            _context = context;
            _tokenServices = tokenServices;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);

            if (user != null)
            {
                return BadRequest(new {message= "Email is already registered." });
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var createUser = new User
            {
                Name=registerDto.Name,
                Email=registerDto.Email,
                PasswordHash=passwordHash,
                Role="Customer"
            };

            _context.Users.Add(createUser);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Registration Successfully."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            var PasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

            if (PasswordValid==false)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            var token = _tokenServices.CreateToken(user);

            var login = new LoginResponseDto
            {
                Name=user.Name,
                Email= user.Email,
                Role=user.Role,
                Token=token
            };

            return Ok(login);
        }
    }
}
