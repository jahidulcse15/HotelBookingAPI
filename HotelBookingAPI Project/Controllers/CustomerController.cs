using HotelBookingAPI_Project.Data;
using HotelBookingAPI_Project.DTOs;
using HotelBookingAPI_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI_Project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController:ControllerBase
    {
        private readonly ApplicationDbcontext _context;

        public CustomerController(ApplicationDbcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _context.Customers.
                            Select(c=>new CustomerResponseDto
                            {
                                Id=c.Id,
                                Name=c.Name,
                                Phone=c.Phone,
                                Email=c.Email,
                                Address=c.Address
                            }).ToListAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _context.Customers.
                           Select(c=>new CustomerResponseDto
                           {
                               Id = c.Id,
                               Name = c.Name,
                               Phone = c.Phone,
                               Email = c.Email,
                               Address = c.Address
                           }).FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerCreateDto customerCreateDto)
        {
            var customer = new Customer
            {
                Name= customerCreateDto.Name,
                Email= customerCreateDto.Email,
                Phone= customerCreateDto.Phone,
                Address= customerCreateDto.Address
            };
 
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,CustomerUpdateDto customerUpdateDto)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if(customer== null)
            {
                return NotFound();
            }

            customer.Name=customerUpdateDto.Name;
            customer.Email=customerUpdateDto.Email;
            customer.Phone=customerUpdateDto.Phone;
            customer.Address=customerUpdateDto.Address;

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null)
            {
                return BadRequest();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
