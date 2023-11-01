using Kontact_Keeper_Pro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kontact_Keeper_Pro.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        // DbSets
        public virtual DbSet<Contact> Contacts { get; set; } = default!;
        public virtual DbSet<Category> Categories { get; set; } = default!;


    }
}