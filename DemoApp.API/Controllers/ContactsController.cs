using DemoApp.API.Models.Domain;
using DemoApp.API.Models.DTO;
using DemoApp.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Serilog;
using System.Text;
using System.Text.Json;

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

            var response = await contactsRepository.getAllContactsAsync(sortBy, sortDirection);

            return Ok(response);
        }

        [HttpPost]
        [Route("addContact")]
        public async Task<IActionResult> createContact([FromBody]ContactDTO contactDetail)
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
                var isDuplicate = await contactsRepository.isduplicateContact(contactDomain);
                if(isDuplicate)
                {
                var Errorresponse = new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    ReasonPhrase = "Contact Already exists !"
                };
                return BadRequest(Errorresponse);
                }
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
                var json = JsonSerializer.Serialize(response);
                var successResponse = new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(json, Encoding.UTF8, "application/json"),
                    ReasonPhrase = "Contact created Successfully!"
                };
                return Ok(successResponse);
        }


        [HttpGet]
        [Route("getContactById/{id}")]
        public async Task<IActionResult> getContactById([FromRoute] int id)
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
                    var json = JsonSerializer.Serialize(response);
                    var successResponse = new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Content = new StringContent(json, Encoding.UTF8, "application/json"),
                        ReasonPhrase = "Contact data fetched"
                    };
                    return Ok(successResponse);
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


        [HttpGet]
        [Route("getContactByEmailAndPhoneNumber")]
        public async Task<IActionResult> getContactByEmailAndPhoneNumber([FromQuery]string email, [FromQuery] string phno)
        {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phno))
                {
                    var Errorresponse = new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        ReasonPhrase = "Both email and phone number are required"
                    };
                    return BadRequest(Errorresponse);
                }
                else
                {
                    var response = await contactsRepository.getContactByEmailAndPhoneNumberAsync(email, phno);
                    if(response !=null)
                    {
                        var json = JsonSerializer.Serialize(response);
                        var successResponse = new HttpResponseMessage
                        {
                            StatusCode = System.Net.HttpStatusCode.OK,
                            Content = new StringContent(json, Encoding.UTF8, "application/json"),
                            ReasonPhrase = "Contact data fetched"
                        };
                        return Ok(successResponse);
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
        }

        [HttpPut]
        [Route("updateContact/{Id}")]
        public async Task<IActionResult> updateContact([FromRoute]int Id,[FromBody] ContactListRespDTO contactDetail)
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

        [HttpDelete]
        [Route("deleteContact/{Id}")]
        public async Task<IActionResult> deleteContact([FromRoute] int Id)
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
    }
}
