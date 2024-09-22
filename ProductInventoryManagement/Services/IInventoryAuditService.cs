using ProductInventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductInventoryManagement.Services
{
    public interface IInventoryAuditService
    {
        Task<InventoryAudit> GetAuditsAsync(Guid inventoryId);

        Task RecordTransactionAsync(Guid inventoryId, int quantity, string reason, string user);
    }
}
