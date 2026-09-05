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
            var room = await _dbcontext.Rooms.FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return BadRequest();
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
    }
}
