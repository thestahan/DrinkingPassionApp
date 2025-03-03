using System.Collections.Generic;
using System.Threading.Tasks;

namespace DrinkingPassion.Api.Core.Interfaces
{
    public interface IGenericRepository<T> where T : Entities.BaseEntity
    {
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T> GetEntityWithSpec(Specifications.ISpecification<T> spec);

        Task<object> GetSpecifiedEntityFieldsWithSpecAsync(Specifications.ISpecification<T> spec);

        Task<IReadOnlyList<T>> ListAsync(Specifications.ISpecification<T> spec);

        Task<T> AddAsync(T entity);

        Task<int> CountAsync(Specifications.ISpecification<T> spec);

        Task UpdateAsync(T entity);

        Task UpdateWithRelatedEntities(T entity);

        Task UpdateSpecifiedPropertiesAsync(T entity, params string[] properties);

        Task<bool> DeleteAsync(T entity);

        Task<bool> DeleteByIdAsync(int id);

        Task<bool> EntityExistsAsync(int id);

        Task<bool> EntityExistsWithSpecAsync(Specifications.ISpecification<T> spec);
    }
}
