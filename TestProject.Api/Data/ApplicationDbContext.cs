using Microsoft.EntityFrameworkCore;

using TestProject.Api.Data.DomainDataMapping;
using TestProject.Api.Models;

namespace TestProject.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Fruit> Fruits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FruitTypeConfiguration());
        }
    }
}
