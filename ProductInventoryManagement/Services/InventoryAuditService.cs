using ProductInventoryManagement.Models;
using ProductInventoryManagement.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductInventoryManagement.Services
{
    public class InventoryAuditService : IInventoryAuditService
    {
        private readonly IInventoryAuditRepository _auditRepository;

        public InventoryAuditService(IInventoryAuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }


        public async Task<InventoryAudit> GetAuditsAsync(Guid inventoryId)
        {
            return await _auditRepository.GetByIdAsync(inventoryId);
        }

        public async Task RecordTransactionAsync(Guid inventoryId, int quantity, string reason, string user)
        {
            var transaction = new InventoryAudit
            {
                AuditID = Guid.NewGuid(),   
                InventoryID = inventoryId,
                AdjustedQuantity = quantity,
                AdjustmentDate = DateTime.UtcNow,
                Reason = reason,
                User = user
            };
            await _auditRepository.AddAsync(transaction);
        }



    }
}
