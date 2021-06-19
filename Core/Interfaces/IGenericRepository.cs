using Core.Entities;
using Core.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<object> GetSpecifiedEntityFieldsWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity);
        Task<int> CountAsync(ISpecification<T> spec);
        Task UpdateAsync(T entity);
        Task UpdateSpecifiedPropertiesAsync(T entity, params string[] properties);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteByIdAsync(int id);
        Task<bool> EntityExistsAsync(int id);
        Task<bool> EntityExistsWithSpecAsync(ISpecification<T> spec);
    }
}
