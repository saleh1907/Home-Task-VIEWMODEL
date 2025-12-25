using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaTwo.Context;
using ProniaTwo.Models;
using System.Threading.Tasks;

namespace ProniaTwo.Areas.Admin.Controllers;

[Area("Admin")]
[AutoValidateAntiforgeryToken]
public class ShippingController : Controller
{
    private readonly AppDbContext _context;

    public ShippingController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Shipping shipping)
    {
        if (ModelState.IsValid)
        {
        await _context.Shippings.AddAsync(shipping);
        await _context.SaveChangesAsync();

            return View(shipping);
        }

     
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Index()
    {
        var shippings = await _context.Shippings.ToListAsync();
        return View(shippings);
    }
    public async Task<IActionResult> Delete(int id)
    {
      var shipping= await _context.Shippings.FindAsync(id);

        if (shipping is null)
            return NotFound();
        _context.Shippings.Remove(shipping);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var shipping = await _context.Shippings.FindAsync(id);

       if(shipping is not { })
            return NotFound();
        return View(shipping);
    }
    [HttpPost]
   
    public async Task<IActionResult> Update(Shipping shipping)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        var existShipping = await _context.Shippings.FindAsync(shipping.Id);
    if (existShipping is null)
            return BadRequest();
    existShipping.Title= shipping.Title;
        existShipping.Description= shipping.Description;
        existShipping.Icon= shipping.Icon;
        _context.Shippings.Update(existShipping);
        await _context.SaveChangesAsync();
        
        return RedirectToAction(nameof(Index));
    }
}
   
