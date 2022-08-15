using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using IdentityManagement.Model;

namespace IdentityManagement.Data
{
    public class IdentityManagementContext : DbContext
    {
        public IdentityManagementContext (DbContextOptions<IdentityManagementContext> options)
            : base(options)
        {
        }

        public DbSet<IdentityManagement.Model.Item> Item { get; set; } = default!;
    }
}
