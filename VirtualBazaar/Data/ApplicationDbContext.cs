using Microsoft.EntityFrameworkCore;
using VirtualBazaar.Models;

namespace VirtualBazaar.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}