using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSyncer.Application.Interfaces
{
    public interface IContactSyncService
    {
        Task SyncContactsAsync();
    }
}
