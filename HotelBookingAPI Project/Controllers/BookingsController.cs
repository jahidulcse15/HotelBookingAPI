using HotelBookingAPI_Project.Data;
using HotelBookingAPI_Project.DTOs;
using HotelBookingAPI_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace HotelBookingAPI_Project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController:ControllerBase
    {
        private readonly ApplicationDbcontext _context;
        
        public BookingsController(ApplicationDbcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _context.Bookings.
                         Include(r => r.Room).Include(c => c.Customer).
                         Select(b=>new BookingResponseDto
                         {
                             Id=b.Id,
                             HotelName=b.Room!.Hotel!.Name,
                             RoomNumber=b.Room.RoomNumber,
                             PricePerNight=b.Room.PricePerNight,
                             TotalPrice=Math.Round(b.Room.PricePerNight * (decimal)(b.CheckOutDate!.Value - b.CheckInDate!.Value).TotalDays,2),
                             NumberOfNights= (b.CheckOutDate!.Value - b.CheckInDate!.Value).Days,
                             CustomerName =b.Customer!.Name,
                             CheckInDate=b.CheckInDate,
                             CheckOutDate=b.CheckOutDate,
                             BookingDate=b.Bookingdate,
                             Status = b.Status
                         }).ToListAsync();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _context.Bookings.Include(r=>r.Room).Include(c=>c.Customer).
                          Select(b=>new BookingResponseDto
                          {
                              Id = b.Id,
                              HotelName = b.Room!.Hotel!.Name,
                              RoomNumber = b.Room.RoomNumber,
                              PricePerNight = b.Room.PricePerNight,
                              TotalPrice = Math.Round(b.Room.PricePerNight * (decimal)(b.CheckOutDate!.Value - b.CheckInDate!.Value).TotalDays, 2),
                              NumberOfNights = (b.CheckOutDate!.Value - b.CheckInDate!.Value).Days,
                              CustomerName = b.Customer!.Name,
                              CheckInDate = b.CheckInDate,
                              CheckOutDate = b.CheckOutDate,
                              BookingDate = b.Bookingdate,
                              Status=b.Status
                          }).FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null)
            {
                return NotFound("Booking Date Not Found.");
            }
            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> bookingCreate(BookingCreateDto bookingCreateDto)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == bookingCreateDto.RoomId);

            if (room == null)
            {
                return NotFound("Room not found.");
            }

            if (bookingCreateDto.CheckInDate < DateTime.Today)
            {
                return BadRequest("Invalid check in date time less then today.");
            }

            if (bookingCreateDto.CheckInDate >= bookingCreateDto.CheckOutDate)
            {
                return BadRequest("Check-in date must be before check-out date.");
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == bookingCreateDto.CustomerId);

            if (customer == null)
            {
                return NotFound("Customer not found.");
            }

            if (bookingCreateDto.CheckInDate >= bookingCreateDto.CheckOutDate)
            {
                return BadRequest("Check-out date must be after check-in date.");
            }

            var IsBooking = await _context.Bookings.AnyAsync(r => r.RoomId == bookingCreateDto.RoomId
                          &&  r.CheckOutDate > bookingCreateDto.CheckInDate
                          && r.CheckInDate < bookingCreateDto.CheckOutDate && r.Status == "Confirmed");

            if (IsBooking == true)
            {
                return Conflict("Room is not available for the selected dates.");
            }

            var booked = new Booking
            {
                RoomId = bookingCreateDto.RoomId,
                CustomerId = bookingCreateDto.CustomerId,
                Status = "Confirmed",
                CheckInDate = bookingCreateDto.CheckInDate,
                CheckOutDate = bookingCreateDto.CheckOutDate,
                Bookingdate = DateTime.Now

            };

            _context.Bookings.Add(booked);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = booked.Id }, booked);
        }


        [HttpGet("{id}/cancel")]
        public async Task<IActionResult> CalcelBook(int id)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound("Booking Not Found.");
            }

            if (booking.Status == "Calcel")
            {
                return BadRequest("Booking is already cancelled.");
            }

            booking.Status = "Cancel";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking Cancel Successfully." });
        }

        [HttpPut("{id}/update")]
        public async Task<IActionResult> Update(int id,BookingUpdateDto bookingUpdateDto)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(b=>b.Id==id);
            if (booking == null)
            {
                return BadRequest("Booking Not Found.");
            }

            if (bookingUpdateDto.CheckInDate < DateTime.Today)
            {
                return BadRequest("Invalid check in date time less then today.");
            }

            if (bookingUpdateDto.CheckInDate >= bookingUpdateDto.CheckOutDate)
            {
                return BadRequest("Check-in date must be before check-out date.");
            }

            if (booking.Status == "Cancel")
            {
                return BadRequest("Cancelled booking cannot be updated.");
            }

            if (bookingUpdateDto.CheckInDate >= bookingUpdateDto.CheckOutDate)
            {
                return BadRequest("Check-out date must be after check-in date.");
            }

            var booked=await _context.Bookings.AnyAsync(b=>b.CheckInDate>bookingUpdateDto.CheckOutDate
            && b.CheckOutDate>bookingUpdateDto.CheckInDate&&b.Status=="Confirmed");

            if (booked == true)
            {
                return Conflict("Room is not available for the selected dates.");
            }

            booking.RoomId = bookingUpdateDto.RoomId;
            booking.CustomerId= bookingUpdateDto.CustomerId;
            booking.CheckInDate= bookingUpdateDto.CheckInDate;
            booking.CheckOutDate=bookingUpdateDto.CheckOutDate;

            await _context.SaveChangesAsync();

            return Ok(booking);
        }


        [HttpGet("customer/{CustomerId}")]
        public async Task<IActionResult> GetBookingByCustomer(int CustomerId)
        {
            var booking = await _context.Bookings.Where(b => b.CustomerId == CustomerId).
                Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    HotelName = b.Room!.Hotel!.Name,
                    RoomNumber = b.Room.RoomNumber,
                    PricePerNight = b.Room.PricePerNight,
                    TotalPrice = Math.Round(b.Room.PricePerNight * (decimal)(b.CheckOutDate!.Value - b.CheckInDate!.Value).TotalDays, 2),
                    CustomerName = b.Customer!.Name,
                    CheckInDate = b.CheckInDate,
                    CheckOutDate = b.CheckOutDate,
                    BookingDate = b.Bookingdate,
                    Status = b.Status
                }).ToListAsync();

            return Ok(booking);
        }


        [HttpGet("Status/{status}")]
        public async Task<IActionResult> GetBookingByStatus(string status)
        {
            var book = await _context.Bookings.Where(b => b.Status.ToLower()==status.ToLower()).
                           Select(b => new BookingResponseDto
                           {
                               Id = b.Id,
                               HotelName = b.Room!.Hotel!.Name,
                               RoomNumber = b.Room.RoomNumber,
                               PricePerNight = b.Room.PricePerNight,
                               TotalPrice = Math.Round(b.Room.PricePerNight * (decimal)(b.CheckOutDate!.Value - b.CheckInDate!.Value).TotalDays, 2),
                               CustomerName = b.Customer!.Name,
                               CheckInDate = b.CheckInDate,
                               CheckOutDate = b.CheckOutDate,
                               BookingDate = b.Bookingdate,
                               Status = b.Status
                           }).ToListAsync();
            return Ok(book);
        }

        [HttpGet("search/date/{Date}")]
        public async Task<IActionResult> GetBookingByDate(DateTime Date)
        {
            var booking = await _context.Bookings.
                Include(b=>b.Customer).
                Include(b=>b.Room).
                Include(b=>b.Room!.Hotel)
                .Where(b => 
                b.CheckInDate <= Date&&b.CheckOutDate>Date&&b.Status=="Confirmed"
                ).Select(b=>new BookingResponseDto
                {
                    Id = b.Id,
                    HotelName = b.Room!.Hotel!.Name,
                    RoomNumber = b.Room.RoomNumber,
                    PricePerNight = b.Room.PricePerNight,
                    TotalPrice = b.Room.PricePerNight *
                         (decimal)(b.CheckOutDate!.Value - b.CheckInDate!.Value).TotalDays,
                    NumberOfNights = (b.CheckOutDate!.Value - b.CheckInDate!.Value).Days,
                    CustomerName = b.Customer!.Name,
                    CheckInDate = b.CheckInDate,
                    CheckOutDate = b.CheckOutDate,
                    BookingDate = b.Bookingdate,
                    Status = b.Status
                }).ToListAsync();

            return Ok(booking);
        }
    }
}
