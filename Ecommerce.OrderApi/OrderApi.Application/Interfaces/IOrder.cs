using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ECommerce.SharedLibrary.Interface;
using OrderApi.Domain.Entities;

namespace OrderApi.Application.Interfaces
{
    public interface IOrder : IGenericInterface<Domain.Entities.Order, Domain.Entities.Order>
    {
       Task<IEnumerable<Domain.Entities.Order>> GetAllOrdersByClientId(int clientId);
       Task<IEnumerable<Domain.Entities.Order>> GetOrdersAsync(Expression<Func<Order,bool>>predicate);

    }
    
    
}
