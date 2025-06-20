using Microsoft.EntityFrameworkCore;
using ApiClienteDesafio.Models;

namespace ApiClienteDesafio.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ClientModel> Clients { get; set; }
        public DbSet<AddressModel> Addresses { get; set; }
        public DbSet<ContactModel> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientModel>().HasKey(c => c.ClientId);
            modelBuilder.Entity<AddressModel>().HasKey(a => a.AddressId);
            modelBuilder.Entity<ContactModel>().HasKey(c => c.ContactId);

            modelBuilder.Entity<ClientModel>()
                .HasOne(c => c.Address)
                .WithOne(a => a.Client)
                .HasForeignKey<AddressModel>(a => a.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientModel>()
                .HasOne(c => c.Contact)
                .WithOne(ct => ct.Client)
                .HasForeignKey<ContactModel>(ct => ct.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}