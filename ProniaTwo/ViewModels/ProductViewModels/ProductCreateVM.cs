using Microsoft.EntityFrameworkCore;
using ProniaTwo.Models;
using System.ComponentModel.DataAnnotations;

namespace ProniaTwo.ViewModels.ProductViewModels;

public class ProductCreateVM
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    //[Required]
    //[MaxLength(200)]
    //public string ImagePath { get; set; }
    [Required]
    [Precision(10, 2)]
    public decimal Price { get; set; }

  
    [Required]
    public int CategoryId { get; set; }
    public IFormFile MainImage { get; set; }
    public IFormFile HoverImage { get; set; }
    public List<IFormFile>? Images{ get; set; }


}
