using System.ComponentModel.DataAnnotations;

namespace VirtualBazaar.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; }
}