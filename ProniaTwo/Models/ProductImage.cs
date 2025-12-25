using ProniaTwo.Models.Common;

namespace ProniaTwo.Models;

public class ProductImage : BaseEntity 
{
    public int ProductId { get; set; }
    public Productt  Productt { get; set; }
    public string  ImagePath { get; set; }

   

}

