using MailSyncer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MailSyncer.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly IContactSyncService _contactSyncService;

        public ContactsController(IContactSyncService contactSyncService)
        {
            _contactSyncService = contactSyncService;
        }

        [HttpGet("sync")]
        public async Task<IActionResult> SyncContacts()
        {
            await _contactSyncService.SyncContactsAsync();
            return Ok(new { message = "Contacts synced successfully" });
        }
    }
}
