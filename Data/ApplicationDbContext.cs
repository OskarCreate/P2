using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using P2.Models;

namespace P2.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

public DbSet<Producto> Producto { get; set; }
public DbSet<CarritoItem> CarritoItem { get; set; }
public DbSet<Pago> Pago { get; set; }
}
