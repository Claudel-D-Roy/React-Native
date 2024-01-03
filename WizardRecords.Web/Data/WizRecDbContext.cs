using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WizardRecords.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WizardRecords.Api.Data;
using WizardRecords.Api.Data.Entities;
using System.Reflection.Emit;

namespace WizardRecords.Api {
    public class WizRecDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid> {
        public DbSet<Album> Albums { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Client { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public WizRecDbContext(DbContextOptions<WizRecDbContext> options) :
            base(options) {}

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

        

            builder.LoadSeed();
        }
    }
}
