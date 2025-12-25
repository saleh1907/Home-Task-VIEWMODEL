using Microsoft.EntityFrameworkCore;
using ProniaTwo.Models;

namespace ProniaTwo.Context;

public class AppDbContext : DbContext
{
  
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Shipping> Shippings { get; set; }
    public DbSet<Productt>Productts  { get; set; }
    public DbSet<Category>Categories { get; set; }
    public DbSet<ProductImage>ProductImages { get; set; }
    

}