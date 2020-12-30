using ContactInfoApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactInfoApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactInfoController : ControllerBase
    {
        private readonly ContactInfoContext _context;

        public ContactInfoController(ContactInfoContext context)
        {
            _context = context;
            if (!_context.Contacts.Any())
            {
                AddTestData();
            }
        }

        private void AddTestData()
        {
            var contact1 = new ContactInfo() { Id = 1, FirstName = "Willam", LastName = "Shakes", MobileNumber = "+4142353246", EmailId = "Willam@outlook.com" };
            var contact2 = new ContactInfo() { Id = 2, FirstName = "Johnny", LastName = "Deep", MobileNumber = "+9842353246", EmailId = "Johnny@outlook.com" };
            var contact3 = new ContactInfo() { Id = 3, FirstName = "Vin", LastName = "Disel", MobileNumber = "+3142353246", EmailId = "Vin@outlook.com" };
            var contact4 = new ContactInfo() { Id = 4, FirstName = "Robert", LastName = "Downey", MobileNumber = "+7142353246", EmailId = "Robert@outlook.com" };
            var contact5 = new ContactInfo() { Id = 5, FirstName = "Will", LastName = "Buyers", MobileNumber = "+9142353246", EmailId = "Will@outlook.com" };
            _context.Add(contact1);
            _context.Add(contact2);
            _context.Add(contact3);
            _context.Add(contact4);
            _context.Add(contact5);
            _context.SaveChanges();

        }
        // GET: contactinfo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactInfo>>> GetContacts()
        {
            return await _context.Contacts.ToListAsync();
        }

        // GET: contactinfo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactInfo>> GetContact(long id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        // PUT: contactinfo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(long id, ContactInfo contact)
        {
            if (id != contact.Id)
            {
                return BadRequest();
            }

            _context.Entry(contact).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return Created("UpdateContact", "Contact updated Successfully");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: contactinfo
        [HttpPost]
        public async Task<ActionResult<ContactInfo>> PostContact(ContactInfo contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetContact", new { id = contact.Id }, contact);
        }

        // DELETE: contactinfo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactInfo>> DeleteContact(long id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return Created("DeleteContact", "Contact Deleted Successfully"); ;
        }

        private bool ContactExists(long id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
