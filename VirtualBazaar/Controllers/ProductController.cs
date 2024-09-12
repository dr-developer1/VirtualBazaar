using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualBazaar.Data;
using VirtualBazaar.Models;

namespace VirtualBazaar.Controllers;

public class ProductController(ApplicationDbContext context, ILogger<ProductController> logger)
    : Controller
{
    private const int PageSize = 10;

    public async Task<IActionResult> Index(string searchString, int? pageIndex)
    {
        logger.LogInformation("Retrieving all products");
        var products = from p in context.Products
            select p;

        if (!string.IsNullOrEmpty(searchString))
        {
            products = products.Where(s => s.Name.Contains(searchString));
        }

        var pageNumber = (pageIndex ?? 1);
        var pagedProducts = await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber, PageSize);

        ViewData["CurrentFilter"] = searchString;
        return View(pagedProducts);
    }
    
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await context.Products
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
    
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Price,CreatedDate,CategoryId")] Product product)
    {
        if (ModelState.IsValid)
        {
            context.Add(product);
            await context.SaveChangesAsync();
            logger.LogInformation("Created new product: {ProductName}", product.Name);
            return RedirectToAction(nameof(Index));
        }
        logger.LogWarning("Failed to create product due to invalid model state");
        return View(product);
    }
    
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,CreatedDate,CategoryId")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid) return View(product);
        try
        {
            context.Update(product);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(product.Id))
            {
                return NotFound();
            }

            throw;
        }

        return RedirectToAction(nameof(Index));

    }
    
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await context.Products
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await context.Products.FindAsync(id);
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
        return context.Products.Any(e => e.Id == id);
    }
}

public class PaginatedList<T> : List<T>
{
    private int PageIndex { get; set; }
    private int TotalPages { get; set; }

    private PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}