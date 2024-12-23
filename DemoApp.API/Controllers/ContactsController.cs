using DemoApp.API.Models.Domain;
using DemoApp.API.Models.DTO;
using DemoApp.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace DemoApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsRepository contactsRepository;

        public ContactsController(IContactsRepository contactsRepoitory)
        {
            this.contactsRepository = contactsRepoitory;
        }
       
        [HttpGet]
        [Route("getAllContacts")]
        public async Task<IActionResult> getAllContacts([FromQuery] string? sortBy,
            [FromQuery] string? sortDirection)
        {
            try
            {
                var response = await contactsRepository.getAllContactsAsync(sortBy, sortDirection);
                return Ok(response);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpPost]
        [Route("addContact")]
        public async Task<IActionResult> createContact([FromBody]ContactDTO contactDetail)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var contactDomain = new Contact
                {
                    FirstName = contactDetail.FirstName,
                    LastName = contactDetail.LastName,
                    Email = contactDetail.Email,
                    PhoneNumber = contactDetail.PhoneNumber,
                    Address = contactDetail.Address,
                    City = contactDetail.City,
                    State = contactDetail.State,
                    Country = contactDetail.Country,
                    PostalCode = contactDetail.PostalCode
                };
                await contactsRepository.createAsync(contactDomain);
                var response = new ContactListRespDTO  //Its not mandatory, but we can have control over response
                { 
                    Id = contactDomain.Id,
                    FirstName = contactDomain.FirstName,
                    LastName = contactDomain.LastName,
                    Email = contactDomain.Email,
                    PhoneNumber = contactDomain.PhoneNumber,
                    Address = contactDomain.Address,
                    City = contactDomain.City,
                    State = contactDomain.State,
                    Country = contactDomain.Country,
                    PostalCode = contactDomain.PostalCode
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet]
        [Route("getContactById/{id}")]
        public async Task<IActionResult> getContactById([FromRoute] int id)
        {
            try
            {
                if (id <=0)
                {
                    var Errorresponse = new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ReasonPhrase = "Invalid Id"
                    };
                    return BadRequest(Errorresponse);
                }
                else
                {
                    var response = await contactsRepository.getContactByIdAsync(id);
                    if (response != null)
                    {
                        return Ok(response);
                    }
                    else
                    {
                        var Errorresponse = new HttpResponseMessage
                        {
                            StatusCode = System.Net.HttpStatusCode.NotFound,
                            ReasonPhrase = "Resource Not Found"
                        };
                        return NotFound(Errorresponse);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet]
        [Route("getContactByEmailAndPhoneNumber")]
        public async Task<IActionResult> getContactByEmailAndPhoneNumber([FromQuery]string email, [FromQuery] string phno)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phno))
                {
                    return BadRequest("Both email and phone number are required.");
                }
                else
                {
                    var response = await contactsRepository.getContactByEmailAndPhoneNumberAsync(email, phno);
                    if(response !=null)
                    {
                        return Ok(response);
                    }
                    else
                    {
                        return NotFound("Contact Not Found");
                    }
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPut]
        [Route("updateContact/{Id}")]
        public async Task<IActionResult> updateContact([FromRoute]int Id,[FromBody] ContactListRespDTO contactDetail)
        {
            try
            {
                var contactDomain = new Contact
                {
                    Id = Id,
                    FirstName = contactDetail.FirstName,
                    LastName = contactDetail.LastName,
                    Email = contactDetail.Email,
                    PhoneNumber = contactDetail.PhoneNumber,
                    Address = contactDetail.Address,
                    City = contactDetail.City,
                    State = contactDetail.State,
                    Country = contactDetail.Country,
                    PostalCode = contactDetail.PostalCode
                };
                var contact = await contactsRepository.updateContactAsync(contactDomain);
                if (contact != null)
                {
                    var response = new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        ReasonPhrase = "Updated Successfully"
                    };
                    return Ok(response);
                }
                else
                {
                    var Errorresponse = new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        ReasonPhrase = "Resource Not Found"
                    };
                    return NotFound(Errorresponse);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("deleteContact/{Id}")]
        public async Task<IActionResult> deleteContact([FromRoute] int Id)
        {
            try
            {
                var contact = await contactsRepository.deleteContactAsync(Id);
                if (contact)
                {
                    var response = new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        ReasonPhrase = "Deleted Successfully"
                    };
                    return Ok(response);
                }
                else
                {
                    var Errorresponse = new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.NotFound,
                        ReasonPhrase = "Contact Not Found"
                    };
                    return NotFound(Errorresponse);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
