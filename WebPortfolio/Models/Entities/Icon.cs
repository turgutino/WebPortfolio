using System.ComponentModel.DataAnnotations;

namespace WebPortfolio.Models.Entities
{
    public class Icon
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Icon CSS class is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Link is required.")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string Link { get; set; }
    }
}
