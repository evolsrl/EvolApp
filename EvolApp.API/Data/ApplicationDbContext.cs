using System.Collections.Generic;
using EvolApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EvolApp.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Empresa> Empresas { get; set; } // opcional si usás EF
    }
}
