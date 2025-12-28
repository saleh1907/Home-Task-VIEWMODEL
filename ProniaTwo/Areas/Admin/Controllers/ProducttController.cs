using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaTwo.Context;
using ProniaTwo.Helpers;
using ProniaTwo.Models;
using ProniaTwo.ViewModels.ProductViewModels;
using System.Threading.Tasks;

namespace ProniaTwo.Areas.Admin.Controllers;

[Area("Admin")]
public class ProducttController(AppDbContext _context, IWebHostEnvironment _envoriement) : Controller
{
   public async Task<IActionResult> Index()
{
    List<ProductGetVM> vm = await _context.Productts
        .Include(x => x.Category)
        .Select(productts => new ProductGetVM
        {
            Id = productts.Id,
            Name = productts.Name,
            CategoryName = productts.Category.Name,
            HoverImagePath = productts.HoverImagePath,
            MainImagePath = productts.MainImagePath,
            Price = productts.Price
        }).ToListAsync();

    return View(vm);  
}

    public async Task<IActionResult> Create()
    {
      await SendItemsWithViewBag();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateVM vm)
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
        foreach (var tagId in vm.TagIds)
        {
            var isExistTag = await _context.Tags.AnyAsync(x => x.Id == tagId);

            if (!isExistTag)
            {
                await SendItemsWithViewBag();
                ModelState.AddModelError("TagIds", "bele bir Tag movcud deyil");
                return View(vm);
            }
        }
        if (!vm.MainImage.CheckType())
        {
            ModelState.AddModelError("MainImage", "Yalniz sekil formatinda data daxil ede bilersiz");
            return View(vm);
        }

        if (!vm.MainImage.CheckSize(2))
        {
            ModelState.AddModelError("MainImage", "Maksimum olcu 2 mb olmalidir");
            return View(vm);
        }
        if (!vm.HoverImage.CheckType())
        {
            ModelState.AddModelError("HoverImage", "Yalniz sekil formatinda data daxil ede bilersiz");
            return View(vm);
        }

        if (!vm.HoverImage.CheckSize(2))
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
            HoverImagePath = uniqueHoverImageName,
            ProductTags = []
        };
        foreach (var tagId in vm.TagIds)
        {
            ProductTag productTag = new()
            {
                TagId = tagId,
                Productt = productt
            };
            productt.ProductTags.Add(productTag);
        }
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
        ProductUpdateVm vm = new ProductUpdateVm()
        {
            Id = product.Id,
            Name = product.Name,
            CategoryId = product.CategoryId,
            Price = product.Price,
            MainImagePath = product.MainImagePath,
            HoverImagePath = product.HoverImagePath,
            TagIds = product.ProductTags.Select(x => x.TagId).ToList()
        };
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(ProductUpdateVm vm)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewBag.Categories = categories;
            return View(vm);
        }

        foreach (var tagId in vm.TagIds)
        {
            var isExistTag = await _context.Tags.AnyAsync(x => x.Id == tagId);

            if (!isExistTag)
            {
                await SendItemsWithViewBag();
                ModelState.AddModelError("TagIds", "bele bir Tag movcud deyil");
                return View(vm);
            }
        }


        if (!vm.MainImage?.CheckType() ?? false)
        {
            ModelState.AddModelError("MainImage", "Yalniz sekil formatinda data daxil ede bilersiz");
            return View(vm);
        }

        if (!vm.MainImage?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("MainImage", "Maksimum olcu 2 mb olmalidir");
            return View(vm);
        }
        if (!vm.HoverImage?.CheckType() ?? false)
        {
            ModelState.AddModelError("HoverImage", "Yalniz sekil formatinda data daxil ede bilersiz");
            return View(vm);
        }

        if (!vm.HoverImage?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("HoverImage", "Maksimum olcu 2 mb olmalidir");
            return View(vm);
        }



        var existProductt = await _context.Productts.Include(x=>x.ProductTags).FirstOrDefaultAsync(x=>x.Id==vm.Id);
        if (existProductt is null)
            return BadRequest();

        existProductt.Name = vm.Name;
        existProductt.Description = vm.Description;
        existProductt.Price = vm.Price;
        //existProductt.ImagePath = productt.ImagePath;
        existProductt.CategoryId = vm.CategoryId;

        existProductt.ProductTags = [];
        foreach (var tagId in vm.TagIds)
        {
            ProductTag productTag = new()
            {
                TagId = tagId,
                ProductId = existProductt.Id
            };
            existProductt.ProductTags.Add(productTag);
        }

        string folderpath = Path.Combine(_envoriement.WebRootPath, "assets", "images", "website-images");
        if (vm.MainImage is { })
        {
            string newMainImagePath = await vm.MainImage.SaveFileAsync(folderpath);

            string existMainImagePath = Path.Combine(folderpath, existProductt.MainImagePath);
            ExtensionMethods.DeleteFile(existMainImagePath);
            existProductt.MainImagePath = newMainImagePath;
        }
        if (vm.HoverImage is { })
        {
            string newHoverImagePath = await vm.HoverImage.SaveFileAsync(folderpath);

            string existHoverImagePath = Path.Combine(folderpath, existProductt.HoverImagePath);
            ExtensionMethods.DeleteFile(existHoverImagePath);
            existProductt.HoverImagePath = newHoverImagePath;
        }



        _context.Productts.Update(existProductt);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var productt = await _context.Productts
            .FirstOrDefaultAsync(x => x.Id == id);

        if (productt is null)

            return NotFound();


        return View(productt);
    }


[HttpGet]
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


    public async Task<IActionResult> Detail(int id)
    {
        var product = await _context.Productts.Include(x => x.Category).Select(product => new ProductGetVM()
        {
            Id = product.Id,
            Name = product.Name,
            CategoryName = product.Category.Name,
            Description = product.Description,
            HoverImagePath = product.HoverImagePath,
            MainImagePath = product.MainImagePath,
            Price = product.Price,
           
            TagNames=product.ProductTags.Select(x=>x.Tag.Name).ToList()
        }).FirstOrDefaultAsync(x=>x.Id == id);
            
            
        if (product is null)
            return NotFound();
        return View(product);
    }

    private async Task SendItemsWithViewBag()
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;


        var tags=await _context.Tags.ToListAsync();

        ViewBag.Tags = tags;
    }
}