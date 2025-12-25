using ProniaTwo.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaTwo.Models
{
    public class Shipping :BaseEntity
    {
       
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? Icon { get; set; } 

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
