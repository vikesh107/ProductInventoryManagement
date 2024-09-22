using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductInventoryManagement.Data;
using ProductInventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductInventoryManagement.Repositories
{
    public class InventoryAuditRepository : Repository<InventoryAudit>, IInventoryAuditRepository
    {
        private readonly AppDbContext _context;

        public InventoryAuditRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
