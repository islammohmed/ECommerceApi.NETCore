﻿using System.ComponentModel.DataAnnotations;
namespace OrderApi.Application.DTOs
{
    public record OrderDetailsDTO
    (
        [Required]
        int OrderId,
        [Required]
        int ProductId,
        [Required]
        int ClientId,
        [Required]
        string Name, 
        [Required,EmailAddress]
        String Email,
        [Required,EmailAddress]
        string Address,
        [Required]
        string TelephoneNumber,
        [Required]
        string ProductName,
        [Required]
        int ProductQuantity,
         
        [Required,DataType(DataType.Currency)] decimal UnitPrice,
        [Required, DataType(DataType.Currency)] decimal TotalPrice,
        [Required]
        DateTime OrderDate
        );
       
    
}
