using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs
{
    public record OrderDto(
    int Id,

    [Required,Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0")]
    int ProductId,
    [Required,Range(1, int.MaxValue, ErrorMessage = "ClientId must be greater than 0")]
    int ClientId,
    [Required,Range(1, int.MaxValue, ErrorMessage = "PurchaseQuantity must be greater than 0")]
    int PurchaseQuantity,
    DateTime OrderDate 

        );
    public record CreateOrderDto(
   [Required,Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0")]
    int ProductId,
   [Required,Range(1, int.MaxValue, ErrorMessage = "ClientId must be greater than 0")]
    int ClientId,
   [Required,Range(1, int.MaxValue, ErrorMessage = "PurchaseQuantity must be greater than 0")]
    int PurchaseQuantity,
   DateTime OrderDate
       );
    public record UpdateOrderDto(
[Required,Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0")]
    int ProductId,
[Required,Range(1, int.MaxValue, ErrorMessage = "ClientId must be greater than 0")]
    int ClientId,
[Required,Range(1, int.MaxValue, ErrorMessage = "PurchaseQuantity must be greater than 0")]
    int PurchaseQuantity,
DateTime OrderDate
   );
}
