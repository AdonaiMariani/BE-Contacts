using AutoMapper;
using BE_Contacts.Models.DTO;
using BE_Contacts.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BE_Contacts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ContactController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IContactRepository _contactRepository;

        public ContactController(IMapper mapper, IContactRepository contactRepository)
        {
            _mapper = mapper;
            _contactRepository = contactRepository;
        }

        private int GetAuthenticatedUserId()
        {
            // Obtener el ID del usuario autenticado desde los Claims
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        // GET: api/Contact
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            try
            {
                int authenticatedUserId = GetAuthenticatedUserId();
                var listContacts = await _contactRepository.GetAllContactsAsync(authenticatedUserId);
                return Ok(listContacts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Contact/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact(int id)
        {
            try
            {
                int authenticatedUserId = GetAuthenticatedUserId();
                var contact = await _contactRepository.GetContactByIdAsync(id, authenticatedUserId);
                return Ok(contact);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/Contact
        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] ContactDTO newContactDTO)
        {
            try
            {
                int authenticatedUserId = GetAuthenticatedUserId();
                var newContact = await _contactRepository.AddContactAsync(newContactDTO, authenticatedUserId);
                return CreatedAtAction(nameof(GetContact), new { id = newContact.Id }, newContact);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/Contact/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactDTO updatedContactDTO)
        {
            try
            {
                if (id != updatedContactDTO.Id)
                {
                    return BadRequest("Contact ID mismatch");
                }

                int authenticatedUserId = GetAuthenticatedUserId();
                await _contactRepository.UpdateContactAsync(updatedContactDTO, authenticatedUserId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PATCH: api/Contact/{id}/favorite
        [HttpPatch("{id}/favorite")]
        public async Task<IActionResult> ToggleFavorite(int id, [FromBody] bool isFavorite)
        {
            try
            {
                int authenticatedUserId = GetAuthenticatedUserId();
                await _contactRepository.ToggleFavoriteAsync(id, authenticatedUserId, isFavorite);
                var updatedContact = await _contactRepository.GetContactByIdAsync(id, authenticatedUserId);
                return Ok(updatedContact);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Contact/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            try
            {
                int authenticatedUserId = GetAuthenticatedUserId();
                await _contactRepository.DeleteContactAsync(id, authenticatedUserId);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}


//using AutoMapper;
//using BE_Contacts.Models.DTO;
//using BE_Contacts.Repository.Interfaces;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using System.Security.Claims;

//namespace BE_Contacts.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize]
//    public class ContactController : ControllerBase
//    {
//        private readonly IMapper _mapper;
//        private readonly IContactRepository _contactRepository;

//        public ContactController(IMapper mapper, IContactRepository contactRepository)
//        {
//            _mapper = mapper;
//            _contactRepository = contactRepository;
//        }

//        private int GetAuthenticatedUserId()
//        {
//            // Obtener el ID del usuario autenticado desde los Claims
//            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
//        }

//        // GET: api/Contact
//        [HttpGet]
//        public async Task<IActionResult> GetContacts(int userId)
//        {
//            try
//            {
//                int authenticatedUserId = GetAuthenticatedUserId();
//                var listContacts = await _contactRepository.GetAllContactsAsync(userId);
//                return Ok(listContacts);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        // GET: api/Contact/{id}
//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetContact(int id, int userId)
//        {
//            try
//            {
//                int authenticatedUserId = GetAuthenticatedUserId();
//                var contact = await _contactRepository.GetContactByIdAsync(id, userId);
//                return Ok(contact);
//            }
//            catch (KeyNotFoundException)
//            {
//                return NotFound();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        // POST: api/Contact
//        [HttpPost]
//        public async Task<IActionResult> AddContact([FromBody] ContactDTO newContactDTO, int userId)
//        {
//            try
//            {
//                int authenticatedUserId = GetAuthenticatedUserId();
//                var newContact = await _contactRepository.AddContactAsync(newContactDTO, userId);
//                return CreatedAtAction(nameof(GetContact), new { id = newContact.Id, userId = userId }, newContact);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        // PUT: api/Contact/{id}
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactDTO updatedContactDTO, int userId)
//        {
//            try
//            {
//                if (id != updatedContactDTO.Id)
//                {
//                    return BadRequest("Contact ID mismatch");
//                }
//                int authenticatedUserId = GetAuthenticatedUserId();
//                await _contactRepository.UpdateContactAsync(updatedContactDTO, userId);
//                return NoContent();
//            }
//            catch (KeyNotFoundException)
//            {
//                return NotFound();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        // PATCH: api/Contact/{id}/favorite
//        [HttpPatch("{id}/favorite")]
//        public async Task<IActionResult> ToggleFavorite(int id, int userId, [FromBody] bool isFavorite)
//        {
//            try
//            {
//                int authenticatedUserId = GetAuthenticatedUserId();
//                await _contactRepository.ToggleFavoriteAsync(id, userId, isFavorite);
//                var updatedContact = await _contactRepository.GetContactByIdAsync(id, userId);
//                return Ok(updatedContact);
//            }
//            catch (KeyNotFoundException)
//            {
//                return NotFound();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        // DELETE: api/Contact/{id}
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteContact(int id, int userId)
//        {
//            try
//            {
//                int authenticatedUserId = GetAuthenticatedUserId();
//                await _contactRepository.DeleteContactAsync(id, userId);
//                return NoContent();
//            }
//            catch (KeyNotFoundException)
//            {
//                return NotFound();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}

//using AutoMapper;
//using BE_Contacts.Models;
//using BE_Contacts.Models.DTO;
//using BE_Contacts.Repository.Interfaces;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;

//namespace BE_Contacts.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ContactController : ControllerBase
//    {

//        private readonly IMapper _mapper;
//        private readonly IContactRepository _contactRepository;

//        public ContactController(IMapper mapper, IContactRepository contactRepository)
//        {
//            _mapper = mapper;
//            _contactRepository = contactRepository;
//        }

//        [HttpGet]

//        public async Task<IActionResult> Get()
//        {
//            try
//            {
//                var listContacts = await _contactRepository.GetAllContacts();

//                var listContactsDTO = _mapper.Map<IEnumerable<ContactDTO>>(listContacts);

//                return Ok(listContactsDTO);
//            }
//            catch(Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        // GET: api/Contact/{id}
//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetContact(int id)
//        {
//            try
//            {
//                var contact = await _contactRepository.GetContactByIdAsync(id);
//                if (contact == null)
//                {
//                    return NotFound();
//                }

//                var contactDTO = _mapper.Map<ContactDTO>(contact);

//                return Ok(contactDTO);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        // POST: api/Contact
//        [HttpPost]
//        public async Task<IActionResult> AddContact([FromBody] ContactDTO newContactDTO)
//        {
//            try
//            {
//                //var newContact = _mapper.Map<Contact>(newContactDTO);

//                var newContact = await _contactRepository.AddContactAsync(newContactDTO);  

//                //var newContactItemDTO = _mapper.Map<ContactDTO>(newContact);

//                return CreatedAtAction(nameof(GetContact), new { id = newContactDTO.Id }, newContactDTO);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        // PUT: api/Contact/{id}
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactDTO updatedContactDTO)
//        {
//            try
//            {


//                if (id != updatedContactDTO.Id)
//                {
//                    return BadRequest("Contact ID mismatch");
//                }

//                // Verificar si el contacto ya existe
//                var existingContact = await _contactRepository.GetContactByIdAsync(id);

//                if (existingContact == null)
//                {
//                    return NotFound();
//                }
//                // Mapear updatedContactDTO a Contact para la actualización
//                //var updatedContact = _mapper.Map<Contact>(updatedContactDTO);

//                // Actualizar el contacto existente con los datos mapeados
//                await _contactRepository.UpdateContactAsync(updatedContactDTO);

//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        // PATCH: api/Contact/{id}/favorite
//        [HttpPatch("{id}/favorite")]
//        public async Task<IActionResult> ToggleFavorite(int id, [FromBody] bool isFavorite)
//        {
//            try
//            {

//                var existingContact = await _contactRepository.GetContactByIdAsync(id);
//                if (existingContact == null)
//                {
//                    return NotFound();
//                }

//                // Llamar al método del repositorio para actualizar el estado de favorito
//                await _contactRepository.ToggleFavoriteAsync(id, isFavorite);

//                // Obtener el contacto actualizado y mapearlo a DTO
//                var updatedContact = await _contactRepository.GetContactByIdAsync(id);
//                var contactDTO = _mapper.Map<ContactDTO>(updatedContact);

//                return Ok(contactDTO);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }



//        // DELETE: api/Contact/{id}
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteContact(int id)
//        {
//            try
//            {
//                var contact = await _contactRepository.GetContactByIdAsync(id);
//                if (contact == null)
//                {
//                    return NotFound();
//                }

//                await _contactRepository.DeleteContactAsync(id);

//                return NoContent();
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}
