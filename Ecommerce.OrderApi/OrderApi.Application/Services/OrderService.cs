using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using OrderApi.Application.DTOs;
using OrderApi.Application.DTOs.Conversions;
using OrderApi.Application.Interfaces;
using Polly;
using Polly.Registry;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrder orderInterface,HttpClient httpClient,ResiliencePipelineProvider<String> resiliencePipeline) : IOrderService
    {
        public async Task<ProductDTO> GetProduct(int productId)
        {
            var response = await httpClient.GetAsync($"api/products/{productId}");
            if(!response.IsSuccessStatusCode) return null!;
            var product = await response.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }
        public async Task<AppUserDTO>GetUser(int userId)
        {
            var getUser = await httpClient.GetAsync($"api/users/{userId}");
            if (!getUser.IsSuccessStatusCode) return null!;
            var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return user!;

        }
        public Task<IEnumerable<OrderDto>> GetOrderByClientID(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
            var order = await orderInterface.GetByIdAsync(orderId);
            if (order is null)
            {
                return null;
            }
           var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");
            var product = await retryPipeline.ExecuteAsync(async token=>await GetProduct(order.ProductId));
            var user = await retryPipeline.ExecuteAsync(async token =>await GetUser(order.ClientId));
            return new OrderDetailsDTO
                (
                    order.Id,
                    product.Id,
                    user.Id,
                    user.Name,
                    user.Email,
                    user.Address,
                    user.TelephoneNumber,
                    product.Name,
                    order.PurchaseQuantity,
                    product.Price,                     // UnitPrice
                    product.Price * order.PurchaseQuantity, // TotalPrice
                    order.OrderDate
                );
        }
        public async Task<IEnumerable<OrderDto>> GetOrdersByClientId(int clientId)
        {
            var orders = await orderInterface.GetOrdersAsync(x => x.ClientId == clientId);
            if(!orders.Any()) return null!;
            var (_,_orders) = OrderConversion.FromEntity(null, orders);
            return _orders!;

        }
    }
}
