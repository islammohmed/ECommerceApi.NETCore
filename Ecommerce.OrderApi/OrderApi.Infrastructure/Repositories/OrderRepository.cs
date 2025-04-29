using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerce.SharedLibrary.Logs;
using ECommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
using OrderApi.Infrastructure.Data;

namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrder
    {
        public async Task<Response> CreateAsync(Order entity)
        {
            try
            {
              var order = context.Add(entity).Entity;
                await context.SaveChangesAsync();
                return new Response(true, "Order Created Successfully");
            }

            catch(Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false,ex.Message);
            }

        }
        public async Task<Response> DeleteAsync(int Id)
        {
            try
            {
                var order = await GetByIdAsync(Id);
                if (order == null)
                {
                    return new Response(false, "Order Not Found");
                }
                context.Orders.Remove(order);
                await context.SaveChangesAsync();
                return new Response(true, "Order Deleted Successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, ex.Message);
            }
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            try
            {
                var orders = await context.Orders.AsNoTracking().ToListAsync();
                return orders is not null ? orders : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception ("Error Accured While Retrieving Orders");
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByClientId(int clientId)
        {
            try
            {
                var orderds = await context.Orders.Where(x => x.ClientId == clientId).ToListAsync();
                if (orderds == null)
                {
                    return null; // Return null if the order is not found  
                }
                return orderds;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error Accured While Retrieving Orders");
            }
        }

        public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.Where(predicate).FirstOrDefaultAsync();
                if (order == null)
                {
                    return null; // Return null if the order is not found  
                }
                return order;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error Accured While Retrieving Orders"); 
            }
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            try
            {
                var order = await context.Orders!.FindAsync(id);
                if (order == null)
                {
                    return null; // Return null if the order is not found  
                }
                return order;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error Accured While Retrieving Orders");
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                var order = await context.Orders.Where(predicate).ToListAsync();
                if (order == null)
                {
                    return null; // Return null if the order is not found  
                }
                return order;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error Accured While Retrieving Orders");
            }
        }

        public async Task<Response> UpdateAsync(int Id, Order entity)
        {
            try
            {
                var order = await context.Orders.FindAsync(Id);
                if (order == null)
                {
                    return new Response(false, "Order Not Found");
                }
                order.ProductId = entity.ProductId;
                order.ClientId = entity.ClientId;
                order.PurchaseQuantity = entity.PurchaseQuantity;
                order.OrderDate = entity.OrderDate;
                context.Orders.Update(order);
                await context.SaveChangesAsync();
                return new Response(true, "Order Updated Successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error Accured While Updating Order");
            }
        }
    }
}
