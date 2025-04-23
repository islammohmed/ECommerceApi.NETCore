using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.DTOs.Convenstions
{
    public class ProductConversion
    {
        public static Product ToEntity(CreateProductDto productDto) => new()
        {
                Name = productDto.Name,
                Price = productDto.Price,
                Quantity = productDto.Quantity
         };
       
        public static (ProductDto?, IEnumerable<ProductDto>?) FromEntity(Product product,IEnumerable<Product>?products){
                if(product is not null || products is null)
                {
                    var singleProduct = new ProductDto(
                        product!.Id,
                        product.Name!,
                        product.Quantity,
                        product.Price
                    );
                return (singleProduct, null);
            }
            if(products is not null || product is null)
            {
                var productList = products!.Select(p => new ProductDto(
                    p.Id,
                    p.Name!,
                    p.Quantity,
                    p.Price
                )).ToList();
                return (null, productList);
            }
            return (null,null);
        } 
    }
}
