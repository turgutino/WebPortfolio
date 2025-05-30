using Microsoft.EntityFrameworkCore;
using WebPortfolio.Models.Entities;

namespace WebPortfolio.Models.Data
{
    public class Context:DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Icon> Icons { get; set; }
        public DbSet<Menu> Menu { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(""); //Buraya connection string elave edin
        }

    }
}
