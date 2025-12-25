using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaTwo.Context;
using ProniaTwo.Models;
using System.Threading.Tasks;

namespace ProniaTwo.Areas.Admin.Controllers;

[Area("Admin")]
public class ProducttController(AppDbContext _context, IWebHostEnvironment _envoriement) : Controller
{
    public async Task<IActionResult> Index()
    {
        var productts = await _context.Productts.Include(x => x.Category).ToListAsync();
        return View(productts);
    }
    public async Task<IActionResult> Create()
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(ViewModels.ProductViewModels.ProductCreateVM vm)
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;

        if (!ModelState.IsValid)


            return View(vm);
        var isExistCtaegory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);
        if (!isExistCtaegory)
        {
            ModelState.AddModelError("CategoryId", "Bele bir category movcud deyil");
        }
        if (vm.MainImage.ContentType.Contains("Images/"))
        {
            ModelState.AddModelError("MainImage", "Yalniz sekil formatinda data daxil ede bilersiz");
            return View(vm);
        }

        if (vm.MainImage.Length > 2 * 1024 * 1024)
        {
            ModelState.AddModelError("MainImage", "Maksimum olcu 2 mb olmalidir");
            return View(vm);
        }
        if (vm.HoverImage.ContentType.Contains("Images"))
        {
            ModelState.AddModelError("HoverImage", "Yalniz sekil formatinda data daxil ede bilersiz");
            return View(vm);
        }

        if (vm.HoverImage.Length > 2 * 1024 * 1024)
        {
            ModelState.AddModelError("HoverImage", "Maksimum olcu 2 mb olmalidir");
            return View(vm);
        }
        string uniqueMainImageName = Guid.NewGuid().ToString() + vm.MainImage.FileName;
        string mainImagePath = @$"{_envoriement.WebRootPath}/assets/images/website-images/{uniqueMainImageName}";
        using FileStream mainStream = new FileStream(mainImagePath, FileMode.Create);

        await vm.MainImage.CopyToAsync(mainStream);
        string uniqueHoverImageName = Guid.NewGuid().ToString() + vm.HoverImage.FileName;
        string HoverImagePath = @$"{_envoriement.WebRootPath}/assets/images/website-images/{uniqueHoverImageName}";
        using FileStream hoverStream = new FileStream(HoverImagePath, FileMode.Create);

        await vm.HoverImage.CopyToAsync(hoverStream);


        Productt productt = new()
        {
            Name = vm.Name,
            Description = vm.Description,
            CategoryId = vm.CategoryId,
            Price = vm.Price,
            MainImagePath = uniqueMainImageName,
            HoverImagePath = uniqueHoverImageName
        };

        await _context.Productts.AddAsync(productt);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }
    [HttpPost]
    public async Task<IActionResult> Deletet(int id)
    {
        var productt = await _context.Productts.FindAsync(id);
        if (productt is null)
            return NotFound();

        _context.Productts.Remove(productt);
        await _context.SaveChangesAsync();

        string folderPath = Path.Combine(_envoriement.WebRootPath, "assets", "images", "website-images");

        string mainImagePath = Path.Combine(folderPath, productt.MainImagePath);
        string HoverImagePath = Path.Combine(folderPath, productt.HoverImagePath);
        if (System.IO.File.Exists(mainImagePath))
            System.IO.File.Delete(mainImagePath);


        if (System.IO.File.Exists(HoverImagePath))
            System.IO.File.Delete(HoverImagePath);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        var product = await _context.Productts.FindAsync(id);
        if (product is null)
        {
            return NotFound();

        }
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
        return View(product);
    }
    [HttpPost]
    public async Task<IActionResult> Update(Productt productt)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
            return View(productt);
        }
        var existProductt = await _context.Productts.FindAsync(productt.Id);
        if (existProductt is null)
            return BadRequest();
        existProductt.Name = productt.Name;
        existProductt.Description = productt.Description;
        existProductt.Price = productt.Price;
        //existProductt.ImagePath = productt.ImagePath;
        _context.Productts.Update(existProductt);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var productt = await _context.Productts
            .FirstOrDefaultAsync(x => x.Id == id);

        if (productt == null)
        {
            return NotFound();
        }

        return View(productt);
    }

    [HttpPost]

    public async Task<IActionResult> Deletes(int id)
    {
        var productt = await _context.Productts.FindAsync(id);
        if (productt == null)
        {
            return NotFound();
        }

        _context.Productts.Remove(productt);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

}