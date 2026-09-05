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
            if (!ModelState.IsValid)
            {
                return BadRequest("Model State Invalid.");
            }

            var findUser = await _context.Customers.AnyAsync(c => c.Email.ToLower() == customerCreateDto.Email.ToLower());

            if (findUser)
            {
                return Conflict("This Email already Uses.Please Enter Another Email Id.");
            }

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
            if (!ModelState.IsValid)
            {
                return BadRequest("Model State Invalid");
            }
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if(customer== null)
            {
                return NotFound();
            }

            var findUser = await _context.Customers.AnyAsync(c => c.Email.ToList() == customerUpdateDto.Email.ToList() && id != c.Id);

            if (customer.Email.ToLower() == customerUpdateDto.Email.ToLower())
            {
                return Conflict("this email already exists.");
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

        [HttpGet("customer/search/NameEmail")]
        public async Task<IActionResult> GetCustomerByNameEmail([FromQuery]string? Name, [FromQuery]string? Email)
        {
            var customer = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Name))
            {
                customer = customer.Where(c => c.Name.ToLower() == Name.ToLower());
            }


            if (!string.IsNullOrWhiteSpace(Email))
            {
                customer = customer.Where(c => c.Email.ToLower() == Email.ToLower());
            }

            var Customer = await customer.Select(c => new CustomerResponseDto
            {
                Id=c.Id,
                Name=c.Name,
                Email=c.Email,
                Phone=c.Phone,
                Address=c.Address
            }).ToListAsync();

            return Ok(Customer);
        }
    }
}
