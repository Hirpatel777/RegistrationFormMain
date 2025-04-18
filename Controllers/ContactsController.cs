using Microsoft.AspNetCore.Mvc;
using UserRegistrationForm.Models;
using UserRegistrationForm.Repository;
  
namespace UserRegistrationForm.Controllers
{
    // Controllers/ContactsController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;

        public ContactsController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        
        // GET: api/Contacts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            return Ok(await _contactRepository.GetAllContacts());
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _contactRepository.GetContactById(id);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        // POST: api/Contacts
        [HttpPost]
        public async Task<ActionResult<Contact>> PostContact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _contactRepository.AddContact(contact);
            contact.ContactId = id;

            return CreatedAtAction("GetContact", new { id = contact.ContactId }, contact);
        }

        // PUT: api/Contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContact(int id, Contact contact)
        {
            if (id != contact.ContactId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _contactRepository.UpdateContact(contact);

            return NoContent();
        }

        // DELETE: api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            await _contactRepository.DeleteContact(id);
            return NoContent();
        }

        // GET: api/Contacts/States
        [HttpGet("States")]
        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {
            return Ok(await _contactRepository.GetAllStates());
        }

        // GET: api/Contacts/Cities?stateId=1
        [HttpGet("Cities")]
        public async Task<ActionResult<IEnumerable<City>>> GetCities(int stateId)
        {
            return Ok(await _contactRepository.GetCitiesByState(stateId));
        }
    }
}
