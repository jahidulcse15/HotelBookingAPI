using HotelBookingAPI_Project.Data;
using HotelBookingAPI_Project.DTOs;
using HotelBookingAPI_Project.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelsController:ControllerBase
    {
        private readonly ApplicationDbcontext _context;

        public HotelsController(ApplicationDbcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GettAll()
        {
            var hotels = await _context.Hotels.
                         Select(h => new HotelResponseDto
                         {
                             Id=h.Id,
                             Name=h.Name,
                             Email=h.Email,
                             Phone=h.Phone,
                             City=h.City,
                             Description=h.Description
                         }).ToListAsync();
            return Ok(hotels);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id == null)
            {
                return NotFound("Id is Null,Please enter valid Id.");
            }
            var hotel = await _context.Hotels.
                        Select(h=>new HotelResponseDto
                        {
                            Id=h.Id,
                            Name=h.Name,
                            Email=h.Email,
                            Phone=h.Phone,
                            City=h.City,
                            Description=h.Description
                        }).FirstOrDefaultAsync(x => x.Id == id);
            if (hotel == null)
            {
                return NotFound($"Id {id} does not exists.");
            }
            return Ok(hotel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel(HotelCreateDto hotelCreateDto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest("Mode is not Valid.");
            }

            var findUser = await _context.Hotels.AnyAsync(h => h.Email.ToLower() == hotelCreateDto.Email.ToLower());

            if (findUser)
            {
                return Conflict("this email already exists.");
            }

            var hotel = new Hotel
            {
                Name= hotelCreateDto.Name,
                Email= hotelCreateDto.Email,
                Phone= hotelCreateDto.Phone,
                Address= hotelCreateDto.Address,
                Description= hotelCreateDto.Description,
                City= hotelCreateDto.City
            };
            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new {id=hotel.Id},hotel);
        }

        [HttpPut("id")]
        public async Task<IActionResult> Update(int id,HotelUpdateDto hotelUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model State in Invalid.");
            }

            var findUser = await _context.Hotels.AnyAsync(c => c.Email.ToLower() == hotelUpdateDto.Email.ToLower()&&id!=c.Id);

            if (findUser)
            {
                return Conflict("this email already exists.");
            }

            var hotel=await _context.Hotels.FirstOrDefaultAsync(x => x.Id == id);

            if (hotel == null)
            {
                return NotFound("Hotel Not Found.");
            }

            hotel.Name= hotelUpdateDto.Name;
            hotel.Email= hotelUpdateDto.Email;
            hotel.Phone= hotelUpdateDto.Phone;
            hotel.City= hotelUpdateDto.City;
            hotel.Description= hotelUpdateDto.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.Id == id);
            if (hotel == null)
            {
                return NotFound($"Id {id} is not Found.");
            }
            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpGet("hotel/{HotelId}")]
        public async Task<IActionResult> GetHotelById(int HotelId)
        {
            var hotel = await _context.Rooms.Where(b => b.HotelId == HotelId).
                Select(b => new RoomResponseDto
                {
                    Id=b.Id,
                    RoomNumber=b.RoomNumber,
                    PricePerNight=b.PricePerNight,
                    RoomType=b.RoomType,
                    HotelName=b.Hotel!.Name,
                    HotelId=b.HotelId,
                    IsAvaiable=b.IsAvaiable
                }).ToListAsync();
            return Ok(hotel);
        }

        [HttpGet("hotel/search/{City}")]
        public async Task<IActionResult> GetHotelByCity(string City)
        {
            var hotel = await _context.Hotels.Where(h => h.City.ToLower() == City.ToLower()).
                Select(h=>new HotelResponseDto
                {
                    Id=h.Id,
                    Name=h.Name,
                    Email=h.Email,
                    Phone=h.Phone,
                    City=h.City,
                    Address=h.Address,
                    Description=h.Description
                }).ToListAsync();

            return Ok(hotel);
        }


        [HttpGet("hotel/search/nameCity")]
        public async Task<IActionResult> GetHotelByNameCity([FromQuery] string? Name, [FromQuery]string? City)
        {
            var hotel = _context.Hotels.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Name))
            {
                hotel = hotel.Where(h => h.Name.ToLower() == Name.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(City))
            {
                hotel = hotel.Where(h => h.City.ToLower() == City.ToLower());
            }

            var hotelroom =await hotel.Select(h =>new HotelResponseDto
            {
                Id = h.Id,
                Name = h.Name,
                Email = h.Email,
                Phone = h.Phone,
                City = h.City,
                Address = h.Address,
                Description = h.Description
            }).ToListAsync();

            return Ok(hotelroom);
        }
    }
}
