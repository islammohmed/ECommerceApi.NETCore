using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs
{
    public record ProductDTO(
        int Id,
        [Required]
        string Name,
        [Required,DataType(DataType.Currency)]
        decimal Price,
        [Required,Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        int Quantity
    );
}
