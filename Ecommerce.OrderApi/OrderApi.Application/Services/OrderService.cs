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
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ProductDTO>();
            }
            else
            {
                throw new Exception("Error fetching product");
            }
        }
        public async Task<AppUserDTO>GetUser(int userId)
        {
            var getUser = await httpClient.GetAsync($"api/users/{userId}");
            if (getUser.IsSuccessStatusCode)
            {
                return await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            }
            else
            {
                throw new Exception("Error fetching user");
            }
        }
        public Task<IEnumerable<OrderDto>> GetOrderByClientID(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
            var order = await orderInterface.GetByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }
           var retryPipline = resiliencePipeline.GetPipeline("retry");
            var product = await retryPipline.ExecuteAsync(async token=>await GetProduct(order.ProductId));
            var user = await retryPipline.ExecuteAsync(async token =>await GetUser(order.ClientId));
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
            var orders = await orderInterface.GetAllAsync();
            if (orders == null)
            {
                return null;
            }
            var ClientOrder = orders.Where(o => o.ClientId == clientId);
            var(_,_orders) = OrderConversion.FromEntity(null, ClientOrder);
            return _orders!;

        }
    }
}
