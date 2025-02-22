using System;
using Microsoft.EntityFrameworkCore;
using products_api.Models;

namespace products_api.Data;

public class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(Entity => Entity.HasKey(p => p.Id));
        modelBuilder.Entity<Product>(Entity => Entity.Property(p => p.Name).IsRequired());
        modelBuilder.Entity<Product>(Entity => Entity.Property(p => p.Price).IsRequired());
        modelBuilder.Entity<Product>(Entity => Entity.Property(p => p.Brand).IsRequired());
        modelBuilder.Entity<Product>(Entity => Entity.Property(p => p.Stock).IsRequired());
    }
}
