using MailSyncer.Application.Dtos;

namespace MailSyncer.Application.Interfaces
{
    public interface IContactSyncService
    {
        Task<SyncResponseDto> SyncContactsAsync();
        Task<SyncResponseDto> CleanContactsAsync();
    }
}