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
            optionsBuilder.UseSqlServer("Data Source=TURGUT-SOFUYEV\\SQLEXPRESS;Initial Catalog=NewWebPortfolio2;Integrated Security=True;Trust Server Certificate=True;");
        }

    }
}
