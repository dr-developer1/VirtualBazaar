using System.ComponentModel.DataAnnotations;

namespace VirtualBazaar.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Name { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    [Required] [Range(0.01, 10000.00)] 
    public float Price { get; set; }

    [EnumDataType(typeof(Category))] 
    public Category Category { get; set; } = Category.Electronics;

    public string ImageUrl { get; set; }
}