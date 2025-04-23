using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ProductApi.Application.DTOs
{
    public record ProductDto
    (
        int Id,
        [Required]
        string Name,
        [Required,Range(1,int.MaxValue)]
        int Quantity,
        [Required,DataType(DataType.Currency)]
        decimal Price
        );
    public record CreateProductDto
 (
     [Required]
        string Name,
     [Required, Range(1, int.MaxValue)]
        int Quantity,
     [Required, DataType(DataType.Currency)]
        decimal Price
 );
    public record UpdateProductDto
(
        string Name,
 [ Range(1, int.MaxValue)]
        int Quantity,
 [ DataType(DataType.Currency)]
        decimal Price
);
}
