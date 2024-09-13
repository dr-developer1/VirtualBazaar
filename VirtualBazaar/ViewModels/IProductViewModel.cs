using VirtualBazaar.Models;

namespace VirtualBazaar.ViewModels;

public class ProductViewModel
{
    public List<Product> Products { get; set; }
    public string SearchString { get; set; }
    public Category? SelectedCategory { get; set; }
}