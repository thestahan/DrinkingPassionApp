using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Entry(entity).Property(x => x.CreatedDate).IsModified = false;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateSpecifiedPropertiesAsync(T entity, params string[] properties)
        {
            _context.Entry(entity).State = EntityState.Unchanged;

            foreach (var property in properties)
            {
                _context.Entry(entity).Property(property).IsModified = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                var entity = new T() { Id = id };

                _context.Attach(entity).State = EntityState.Deleted;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Attach(entity).State = EntityState.Deleted;

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> EntityExistsAsync(int id)
        {
            return await _context.Set<T>().AnyAsync(x => x.Id == id);
        }

        public async Task<bool> EntityExistsWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).AnyAsync();
        }

        public async Task<object> GetSpecifiedEntityFieldsWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).Select(spec.Select).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        public async Task UpdateWithRelatedEntities(T entity)
        {
            _context.Set<T>().Update(entity);

            await _context.SaveChangesAsync();
        }
    }
}