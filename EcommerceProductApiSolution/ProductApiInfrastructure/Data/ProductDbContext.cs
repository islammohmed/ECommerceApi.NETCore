﻿using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Product> Products { get; set; }
}
