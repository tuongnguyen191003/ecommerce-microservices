using SharedLibrary.ApiResponses;
using System.Linq.Expressions;


namespace SharedLibrary.Interface
{
    public interface IGenericInterface<T> where T : class 
    {
        Task<ApiResponse<bool>> CreateAsync(T entity);
        Task<ApiResponse<bool>> UpdateAsync(T entity);
        Task<ApiResponse<bool>> DeleteAsync(T entity);
        Task<ApiResponse<IEnumerable<T>>> GetAllAsync();
        Task<ApiResponse<T>> FindByIdAsync(int id);
        Task<ApiResponse<T>> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
