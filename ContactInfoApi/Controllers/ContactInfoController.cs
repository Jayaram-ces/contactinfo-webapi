using ContactInfoApi.Model;
using ContactInfoApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactInfoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContactInfoController : ControllerBase
    {
        private IContactReposistory contactReposistory;

        public ContactInfoController(IContactReposistory _contactReposistory)
        {
            contactReposistory = _contactReposistory;
            
        }

        [HttpPost]
        [Route("AddContact")]
        public async Task<IActionResult> AddContact([FromBody] ContactInfo contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (contact?.EmailId == null)
                        return BadRequest("The email address is required");
                    var contactId = await contactReposistory.AddContact(contact);
                    if (contactId > 0)
                    {
                        return Ok("Contact added successfully");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {

                    return BadRequest();
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetContacts")]
        public async Task<IActionResult> GetContacts()
        {
            try
            {
                var contacts = await contactReposistory.GetContacts();
                if (contacts == null)
                {
                    return NotFound();
                }

                return Ok(contacts);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetContact")]
        public async Task<IActionResult> GetContact(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                var contact = await contactReposistory.GetContact(id);

                if (contact == null)
                {
                    return NotFound();
                }

                return Ok(contact);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("UpdateContact")]
        public async Task<IActionResult> UpdateContact(int? id, [FromBody] ContactInfo contact)
        {
            if(id == null)
            {
                return BadRequest("Please provide valid Id to update the contact.");
            }

            if (id != contact.Id)
            {
                return BadRequest("Contact was not found in database. Please check the given data.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await contactReposistory.UpdateContact(contact);

                    return Ok("Contact updated successfully");
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }

                    return BadRequest();
                }
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("DeleteContact")]
        public async Task<IActionResult> DeleteContact(int? id)
        {
            int result = 0;

            if (id == null)
            {
                return BadRequest();
            }

            try
            {
                result = await contactReposistory.DeleteContact(id);
                if (result == 0)
                {
                    return NotFound();
                }
                return Ok("Contact Deleted successfully");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
