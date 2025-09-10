using BaseProject.API.Security.Authorization;
using BaseProject.Domain.Constants.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [PermissionAuthorize("CustomerController")]
    public class CustomerController : ControllerBase
    {
        // In-memory sample data
        private static List<Customer> _customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Alice", Email = "alice@example.com", CreatedOn = DateTime.UtcNow },
            new Customer { Id = 2, Name = "Bob", Email = "bob@example.com", CreatedOn = DateTime.UtcNow }
        };

        // GET: api/customer
        [HttpGet]
        [PermissionAuthorizeAction(PermissionActionName.List)]
        public ActionResult<List<Customer>> GetAll()
        {
            return Ok(_customers);
        }

        // GET: api/customer/1
        [HttpGet("{id}")]
        [PermissionAuthorizeAction(PermissionActionName.List)]
        public ActionResult<Customer> Get(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        // POST: api/customer
        [HttpPost]
        [PermissionAuthorizeAction(PermissionActionName.Create)]
        public ActionResult<Customer> Create(Customer customer)
        {
            customer.Id = _customers.Any() ? _customers.Max(c => c.Id) + 1 : 1;
            customer.CreatedOn = DateTime.UtcNow;
            _customers.Add(customer);
            return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
        }

        // PUT: api/customer/1
        [HttpPut("{id}")]
        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        public ActionResult Update(int id, Customer updatedCustomer)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null) return NotFound();

            customer.Name = updatedCustomer.Name;
            customer.Email = updatedCustomer.Email;
            return NoContent();
        }

        // DELETE: api/customer/1
        [HttpDelete("{id}")]
        [PermissionAuthorizeAction(PermissionActionName.Delete)]
        public ActionResult Delete(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            if (customer == null) return NotFound();
            _customers.Remove(customer);
            return NoContent();
        }
    }

    // Simple model
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
