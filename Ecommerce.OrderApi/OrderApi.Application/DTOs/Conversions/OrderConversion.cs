using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.SharedLibrary.Responses;
using OrderApi.Domain.Entities;

namespace OrderApi.Application.DTOs.Conversions
{
    public class OrderConversion
    {
        public static Order ToEntity(CreateOrderDto orderDto) => new Order()
        {
            ProductId = orderDto.ProductId,
            ClientId = orderDto.ClientId,
            PurchaseQuantity = orderDto.PurchaseQuantity,
            OrderDate = orderDto.OrderDate
        };
        public static (OrderDto?, IEnumerable<OrderDto>?) FromEntity(Order order, IEnumerable<Order> clientOrders)
        {
            var dto = order is null
                ? null
                : new OrderDto(order.Id, order.ProductId, order.ClientId, order.PurchaseQuantity, order.OrderDate);

            var list = clientOrders?
                .Select(o => new OrderDto(o.Id, o.ProductId, o.ClientId, o.PurchaseQuantity, o.OrderDate))
                .ToList();

            return (dto, list);
        }


    }
}
