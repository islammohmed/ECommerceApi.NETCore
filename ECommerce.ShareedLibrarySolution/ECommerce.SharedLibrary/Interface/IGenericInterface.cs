using System.Linq.Expressions;
using ECommerce.SharedLibrary.Responses;
namespace ECommerce.SharedLibrary.Interface
{
    public interface IGenericInterface<T,U> where T : class
    {
        Task<Response> CreateAsync(T entity);
        Task<Response> UpdateAsync(int Id , U entity );
        Task<Response> DeleteAsync(int Id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
