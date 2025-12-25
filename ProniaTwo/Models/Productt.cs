using Microsoft.EntityFrameworkCore;
using ProniaTwo.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace ProniaTwo.Models;

public class Productt:BaseEntity
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    //[Required]
    //[MaxLength(200)]
    //public string ImagePath { get; set; }
    [Required]
    [Precision(10,2)]
    public decimal Price { get; set; }

    public Category?  Category { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public string MainImagePath { get; set; }
     public string HoverImagePath { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; } = [];

}

