using ProductApi.Domain.Entities;
using ProductApi.Application.Interfaces;
using ECommerce.SharedLibrary.Responses;
using System.Linq.Expressions;
using ProductApi.Infrastructure.Data;
using ECommerce.SharedLibrary.Logs;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.DTOs;
namespace ProductApi.Infrastructure.Repositories
{
    internal class ProductRepository( ProductDbContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                var getProduct = await GetByAsync(x => x.Name == entity.Name);
                if (getProduct != null && !string.IsNullOrEmpty(getProduct.Name))
                {
                    return new Response(false, "Product already exists");
                }
                var currentEntity = context.Products.Add(entity).Entity;
                await context.SaveChangesAsync();
                if (currentEntity != null && currentEntity.Id>0)
                {
                    return new Response(true, "Product created successfully");
                }
                else 
                {
                    return new Response(false, "Error creating product");
                }
            }
            catch (Exception ex)
            {
               LogException.LogExceptions(ex);
                return new Response(false, "Error creating product");
            }
        }

        public async Task<Response> DeleteAsync(int id)
        {
            try { 
                var product = await GetByIdAsync(id);
                if (product != null)
                {
                    context.Products.Remove(product);
                    await context.SaveChangesAsync();
                    return new Response(true, "Product deleted successfully");
                }
                else
                {
                    return new Response(false, "Product not found");
                }
        }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error deleting product");
            }
        }


        public  async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await context.Products.ToListAsync();
                if (products != null)
                {
                    return products;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error getting products");
            }
        }
        public Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            var product = context.Products.Where(predicate).FirstOrDefaultAsync();
            if (product != null)
            {
                return product;
            }
            else
            {
                return null;
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                if (product != null)
                {
                    return product;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error getting product");
            }
        }

        public async Task<Response> UpdateAsync(int id , UpdateProductDto dto)
        {
            try
            {
                var product = await GetByIdAsync(id);
                if(product is null)
                {
                    return new Response(false, "Product not found");
                }
                product.Name = dto.Name;
                product.Quantity = dto.Quantity;
                product.Price = dto.Price;
                context.Products.Update(product);
                await context.SaveChangesAsync();
                return new Response(true, "Product updated successfully");
            }
            catch(Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error updating product");
            }
            

        }

      
    }
}
