using HotelBookingAPI_Project.Data;
using HotelBookingAPI_Project.DTOs;
using HotelBookingAPI_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI_Project.Controllers
{
    [Authorize(Roles ="Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController:ControllerBase
    {
        private readonly ApplicationDbcontext _dbcontext;

        public RoomsController(ApplicationDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _dbcontext.Rooms.
                        Select(h=>new RoomResponseDto
                        {
                            Id=h.Id,
                            HotelId=h.Id,
                            PricePerNight=h.PricePerNight,
                            RoomNumber=h.RoomNumber,
                            RoomType=h.RoomType,
                            IsAvaiable=h.IsAvaiable,
                            HotelName=h.Hotel!.Name

                        }).ToListAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _dbcontext.Rooms.
                     Select(h=>new RoomResponseDto
                     {
                         Id = h.Id,
                         HotelId = h.Id,
                         PricePerNight = h.PricePerNight,
                         RoomNumber = h.RoomNumber,
                         RoomType = h.RoomType,
                         IsAvaiable = h.IsAvaiable,
                     }).FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> RoomCreate(CreateRoomDto createRoomDto)
        {

            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            var findRoom = await _dbcontext.Rooms.AnyAsync(r => r.
            HotelId == createRoomDto.HotelId && r.RoomNumber == createRoomDto.RoomNumber);

            if (findRoom)
            {
                return Conflict("this room number already exusts in this hotel.");
            }

            var room = new Room
            {
                RoomNumber= createRoomDto.RoomNumber,
                RoomType= createRoomDto.RoomType,
                PricePerNight= createRoomDto.PricePerNight,
                IsAvaiable= createRoomDto.IsAvaiable,
                HotelId= createRoomDto.HotelId
            };

            if (room == null)
            {
                return BadRequest();
            }

           await _dbcontext.Rooms.AddAsync(room);
           await _dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room); ;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,UpdateDto updateDto)
        {
            var findRoom = await _dbcontext.Rooms.AnyAsync(r => r.
            HotelId == updateDto.HotelId && r.RoomNumber == updateDto.RoomNumber&&id!=r.Id);

            if (findRoom)
            {
                return Conflict("this room number already exusts in this hotel.");
            }
            var room = await _dbcontext.Rooms.FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return BadRequest("Invalid Hotel Id.");
            }

            room.RoomNumber = updateDto.RoomNumber;
            room.PricePerNight = updateDto.RoomNumber;
            room.HotelId = updateDto.HotelId;
            room.RoomType = updateDto.RoomType;
            room.IsAvaiable = updateDto.IsAvaiable;

            _dbcontext.Rooms.Update(room);

            await _dbcontext.SaveChangesAsync();

            return Ok("Update Successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _dbcontext.Rooms.Include(r => r.Hotel).FirstOrDefaultAsync(r => r.Id == id);

            if (room == null) { return NotFound(); }

            _dbcontext.Rooms.Remove(room);

            await _dbcontext.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("room/{IsAvailable}")]
        public async Task<IActionResult> RoomIsAvailable(bool IsAvailable)
        {
            var room = await _dbcontext.Rooms.Where(r => r.IsAvaiable == IsAvailable).
                Select(r => new RoomResponseDto
                {
                   Id=r.Id,
                   RoomNumber=r.RoomNumber,
                   RoomType=r.RoomType,
                   PricePerNight=r.PricePerNight,
                   IsAvaiable=r.IsAvaiable,
                   HotelId=r.Hotel!.Id,
                   HotelName=r.Hotel!.Name

                }).ToListAsync();

            return Ok(room);
        }


        [HttpGet("room/{RoomId}/available")]
        public async Task<IActionResult> GetAvaiableRoomById(int RoomId)
        {
            var room = await _dbcontext.Rooms.Where(b => b.Hotel!.Id == RoomId && b.IsAvaiable == true).
                Select(r => new RoomResponseDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    RoomType = r.RoomType,
                    PricePerNight = r.PricePerNight,
                    IsAvaiable = r.IsAvaiable,
                    HotelId = r.Hotel!.Id,
                    HotelName = r.Hotel!.Name
                }).ToListAsync();
            
            return Ok(room);
        }


        [HttpGet("type/{RoomType}")]
        public async Task<IActionResult> GetRoomByRoomType(string RoomType)
        {
            var book = await _dbcontext.Rooms.Where(r => r.RoomType.ToLower()==(RoomType.ToLower())).
                Select(r => new RoomResponseDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    RoomType = r.RoomType,
                    PricePerNight = r.PricePerNight,
                    IsAvaiable = r.IsAvaiable,
                    HotelId = r.Hotel!.Id,
                    HotelName = r.Hotel!.Name
                }).ToListAsync();
            return Ok(book);
        }


        [HttpGet("room/search/{Price}")]
        public async Task<IActionResult>GetRoomByPrice(decimal Price)
        {
            var room = await _dbcontext.Rooms.OrderByDescending(r => r.PricePerNight).
                Where(r => r.PricePerNight <= Price).
                Select(r => new RoomResponseDto
                {
                    Id = r.Id,
                    RoomNumber = r.RoomNumber,
                    RoomType = r.RoomType,
                    PricePerNight = r.PricePerNight,
                    IsAvaiable = r.IsAvaiable,
                    HotelId = r.Hotel!.Id,
                    HotelName = r.Hotel!.Name
                }).ToListAsync();

            return Ok(room);
        }

        [HttpGet("search/price/roomType")]
        public async Task<IActionResult> GetRoomByPriceRoomType([FromQuery]decimal? Price,[FromQuery]string? RoomType, [FromQuery] bool? IsAvailable)
        {
            var room = _dbcontext.Rooms.AsQueryable();

            if (!string.IsNullOrEmpty(RoomType))
            {
                room=room.Where(r => r.RoomType.ToLower() == RoomType);
            }

            if (IsAvailable.HasValue)
            {
                room = room.Where(r => r.IsAvaiable == IsAvailable);
            }

            if (Price.HasValue)
            {
                room=room.Where(r => r.PricePerNight <= Price);
            }

            var rooms = await room.Select(r => new RoomResponseDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                RoomType = r.RoomType,
                PricePerNight = r.PricePerNight,
                IsAvaiable = r.IsAvaiable,
                HotelId = r.Hotel!.Id,
                HotelName = r.Hotel!.Name
            }).ToListAsync();

            return Ok(rooms);
        }


        [HttpGet("search/Range")]
        public async Task<IActionResult> GetRoomByRange([FromQuery] decimal? MaxPrice, [FromQuery] decimal?MinPrice ,[FromQuery] string? RoomType, [FromQuery] bool? IsAvailable)
        {
            var room = _dbcontext.Rooms.AsQueryable();

            if (!string.IsNullOrEmpty(RoomType))
            {
                room = room.Where(r => r.RoomType.ToLower() == RoomType);
            }

            if (MinPrice.HasValue)
            {
                room = room.Where(r => r.PricePerNight >= MinPrice);
            }

            if (IsAvailable.HasValue)
            {
                room = room.Where(r => r.IsAvaiable == IsAvailable);
            }

            if (MaxPrice.HasValue)
            {
                room = room.Where(r => r.PricePerNight <= MaxPrice);
            }

            var rooms = await room.Select(r => new RoomResponseDto
            {
                Id = r.Id,
                RoomNumber = r.RoomNumber,
                RoomType = r.RoomType,
                PricePerNight = r.PricePerNight,
                IsAvaiable = r.IsAvaiable,
                HotelId = r.Hotel!.Id,
                HotelName = r.Hotel!.Name
            }).ToListAsync();

            return Ok(rooms);
        }
    }
}
