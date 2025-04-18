using Microsoft.AspNetCore.Mvc;
using UserRegistrationForm.Models;
using UserRegistrationForm.Repository;

namespace UserRegistrationForm.Controllers
{
    public class UserController : Controller
    {
        private readonly IContactRepository _contactRepository;

        public UserController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _contactRepository.GetAllContacts();
            return View(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Contact contact)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var id = await _contactRepository.AddContact(contact);
            return Json(new { success = true, contactId = id });
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _contactRepository.UpdateContact(contact);
            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _contactRepository.DeleteContact(id);
            return Json(new { success = true });
        }
    }
}
