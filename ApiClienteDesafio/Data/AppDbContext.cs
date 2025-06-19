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
    }
}