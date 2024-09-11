using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtualBazaar.Validation;

namespace VirtualBazaar.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }

    [Required]
    [Range(0.01, 10000.00)]
    public decimal Price { get; set; }

    [DataType(DataType.Date)]
    [FutureDate(ErrorMessage = "Release date must be in the future")]
    public DateTime ReleaseDate { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    [Display(Name = "Product Image")]
    public string ImagePath { get; set; }

    [NotMapped]
    [Display(Name = "Upload Image")]
    public IFormFile ImageFile { get; set; }
}