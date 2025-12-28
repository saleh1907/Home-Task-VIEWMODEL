using ProniaTwo.Models.Common;

namespace ProniaTwo.Models;

public class ProductTag : BaseEntity 
{
    public Productt Productt { get; set; } = null!;
  
    public int ProductId    { get; set; }
    public Tag Tag { get; set; }=null!;
    public int TagId { get; set; }

}