
using ECommerce.SharedLibrary.Interface;
using ProductApi.Application.DTOs;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Interfaces
{
    public interface IProduct : IGenericInterface<Product,UpdateProductDto>
    {
    }
}
