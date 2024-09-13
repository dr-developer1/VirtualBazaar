using Microsoft.AspNetCore.Mvc;
using VirtualBazaar.Models;
using VirtualBazaar.ViewModels;

namespace VirtualBazaar.Controllers;

[Route("[controller]/[action]")]
public class ProductController(ILogger<ProductController> logger)
    : Controller
{
    private static readonly List<Product> Products = [];

    public IActionResult Index(string searchString, Category? category)
    {
        logger.LogInformation("Retrieving all products");

        var filteredItems = Products;

        if (!string.IsNullOrEmpty(searchString))
        {
            filteredItems = filteredItems.Where(i => i.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        if (category.HasValue)
        {
            filteredItems = filteredItems.Where(i => i.Category == category.Value).ToList();
        }

        var viewModel = new ProductViewModel
            { Products = filteredItems, SearchString = searchString, SelectedCategory = category };
        return View(viewModel);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product, IFormFile image)
    {
        logger.LogInformation("Creating a new product");
        if (image is { Length: > 0 })
        {
            var fileName = Path.GetFileName(image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }

            product.ImageUrl = "/images/" + fileName;
        }

        product.Id = Products.Count + 1;
        product.CreatedDate = DateTime.Now;
        Products.Add(product);
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = Products.FirstOrDefault(i => i.Id == id);

        return View(product);
    }

    [HttpPost]
    public IActionResult Edit(Product product, IFormFile image)
    {
        var existingItem = Products.FirstOrDefault(i => i.Id == product.Id);
        if (existingItem == null) return RedirectToAction("Index");
        existingItem.Name = product.Name;
        existingItem.Price = product.Price;
        existingItem.Category = product.Category;

        if (image is not { Length: > 0 }) return RedirectToAction("Index");
        var fileName = Path.GetFileName(image.FileName);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            image.CopyTo(stream);
        }

        existingItem.ImageUrl = "/images/" + fileName;
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = Products
            .FirstOrDefault(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        logger.LogInformation("Deleting a product" + id);
        var product = Products.FirstOrDefault(p => p.Id == id);
        if (product == null) return NotFound();
        Products.Remove(product);
        return RedirectToAction("Index");
    }
}