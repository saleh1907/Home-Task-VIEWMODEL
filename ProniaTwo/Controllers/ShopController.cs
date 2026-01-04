using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaTwo.Context;
using ProniaTwo.ViewModels.ProductViewModels;
using System.Threading.Tasks;

namespace ProniaTwo.Controllers
{
    public class ShopController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = await _context.Productts.ToListAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var product=await _context.Productts.Select(x=>new ProductGetVM()
            {
                Id = x.Id,
                Name = x.Name,
                Price=x.Price,
                Description = x.Description,
                AdditionalImagePaths=x.ProductImages.Select(x=>x.ImagePath).ToList(),
                CategoryName=x.Category.Name,
                HoverImagePath=x.HoverImagePath,
                MainImagePath=x.MainImagePath,
                TagNames=x.ProductTags.Select(x=>x.Tag.Name).ToList()
            }).FirstOrDefaultAsync(x=>x.Id==id);
            if (product is null)
                return NotFound();

            return View(product);
        }
    }
}
