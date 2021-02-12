using ContactInfoApi.Model;
using ContactInfoApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactInfoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ContactInfoController : ControllerBase
    {
        private IContactRepository _contactService;
        private readonly ILogger<ContactInfoController> _logger;
        public ContactInfoController(ILogger<ContactInfoController> logger, IContactRepository contactRepository)
        {
            _logger = logger;
            _contactService = contactRepository;
            
        }

        /// <summary>
        /// Add contact to the directory
        /// </summary>
        /// <param name="contactInfoModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddContact([FromBody] ContactInfoModel contactInfoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            try
            {
                await _contactService.AddAsync(contactInfoModel);
                return Ok("Contact added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Adding contact to directary failed.");
                _logger.LogCritical(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get contacts from the directory
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContactInfoModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetContacts()
        {
            try
            {
                var result = await _contactService.GetContactsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Retrieving contacts failed.");
                _logger.LogCritical(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Get contact by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ContactInfoModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetContact([FromRoute] int id)
        {
            try
            {
                var contact = await _contactService.GetContactAsync(id);

                if (contact == null)
                {
                    return NotFound("Not found in the directory.");
                }

                return Ok(contact);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to retrieve the contact.");
                _logger.LogCritical(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Update contact in the directory
        /// </summary>
        /// <param name="contactInfoModel"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(SerializableError), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateContact([FromBody] ContactInfoModel contactInfoModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _contactService.UpdateAsync(contactInfoModel);
                return Ok("Contact updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to update the contact.");
                _logger.LogCritical(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Delete contact in the directory
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(ContactInfoModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteContact([FromRoute] int id)
        {
            try
            {
                await _contactService.DeleteAsync(id);
                return Ok("Contact Deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Failed to delete the contact.");
                _logger.LogCritical(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
