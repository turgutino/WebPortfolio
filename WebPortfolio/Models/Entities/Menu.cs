using System.ComponentModel.DataAnnotations;

namespace WebPortfolio.Models.Entities
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }

        public string ProfilPhoto { get; set; }

        public string Name { get; set; }
        public string Position { get; set; }

        public string Description { get; set; }

        public string Contact { get; set; }
    }
}
