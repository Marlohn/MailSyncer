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

        [HttpPost("sync")]
        public async Task<IActionResult> SyncContacts()
        {
            var syncResponse = await _contactSyncService.SyncContactsAsync();

            if (syncResponse == null || syncResponse.SyncedContacts == 0)
            {
                return NoContent();
            }

            return Ok(syncResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            var syncResponse = await _contactSyncService.GetContactsAsync();

            if (syncResponse == null || syncResponse.SyncedContacts == 0)
            {
                return NoContent();
            }

            return Ok(syncResponse);
        }

        [HttpDelete]
        public async Task<IActionResult> CleanContacts()
        {
            var syncResponse = await _contactSyncService.CleanContactsAsync();

            if (syncResponse == null || syncResponse.SyncedContacts == 0)
            {
                return NoContent();
            }

            return Ok(syncResponse);
        }
    }
}