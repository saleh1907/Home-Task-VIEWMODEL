using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProniaTwo.Models;

namespace ProniaTwo.Context;

public class AppDbContext : IdentityDbContext<AppUser>
{
  
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Shipping> Shippings { get; set; }
    public DbSet<Productt>Productts  { get; set; }
    public DbSet<Category>Categories { get; set; }
    public DbSet<ProductImage>ProductImages { get; set; }

    public  DbSet<Tag>Tags { get; set; }
    public  DbSet<ProductTag>ProductTags { get; set; }

}