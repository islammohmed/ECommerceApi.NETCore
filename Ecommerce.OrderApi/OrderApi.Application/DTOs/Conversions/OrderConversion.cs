using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Application.DTOs.Conversions
{
    public class OrderConversion
    {
        public static Domain.Entities.Order ToEntity(OrderDto orderDto) => new Domain.Entities.Order()
        {
            Id = orderDto.Id,
            ProductId = orderDto.ProductId,
            ClientId = orderDto.ClientId,
            PurchaseQuantity = orderDto.PurchaseQuantity,
            OrderDate = orderDto.OrderDate
        };
        public static (OrderDto?,IEnumerable<OrderDto>?) FromEntity(Domain.Entities.Order order, IEnumerable<Domain.Entities.Order> clientOrder) =>
            (order is null ? null : new OrderDto(order.Id, order.ProductId, order.ClientId, order.PurchaseQuantity, order.OrderDate), null);
    }
}
